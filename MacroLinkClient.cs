using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;

namespace tabsik12.MacroLink
{
    public class MacroLinkClient : IDisposable
    {
        private readonly Main _plugin;
        private ClientWebSocket? _ws;
        private CancellationTokenSource? _cts;

        public MacroLinkClient(Main plugin)
        {
            _plugin = plugin;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _ = Task.Run(() => RunLoop(_cts.Token));
        }

        // Zamyka biezace polaczenie i startuje od nowa - wywolywane po zmianie
        // hosta/portu w konfiguratorze.
        public void Restart()
        {
            StopInternal();
            Start();
        }

        private async Task RunLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await ConnectAndListen(token);
                }
                catch
                {
                    // Mod nie jest uruchomiony / gra jeszcze sie ladowala /
                    // polaczenie padlo - proba ponowienia nastapi ponizej.
                }

                if (token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        private async Task ConnectAndListen(CancellationToken token)
        {
            var uri = new Uri(GetConfiguredUri());

            using var ws = new ClientWebSocket();
            _ws = ws;

            await ws.ConnectAsync(uri, token);

            var buffer = new byte[8192];
            while (ws.State == WebSocketState.Open && !token.IsCancellationRequested)
            {
                using var ms = new MemoryStream();
                WebSocketReceiveResult result;
                do
                {
                    result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        return;
                    }
                    ms.Write(buffer, 0, result.Count);
                } while (!result.EndOfMessage);

                var json = Encoding.UTF8.GetString(ms.ToArray());
                HandleMessage(json);
            }
        }

        private string GetConfiguredUri()
        {
            var host = PluginConfiguration.GetValue(_plugin, "host") as string;
            var portRaw = PluginConfiguration.GetValue(_plugin, "port") as string;

            if (string.IsNullOrWhiteSpace(host))
            {
                host = "127.0.0.1";
            }

            if (string.IsNullOrWhiteSpace(portRaw) || !int.TryParse(portRaw, out var port))
            {
                port = 25599;
            }

            return $"ws://{host}:{port}";
        }

        private void HandleMessage(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                SetVar("mc_health", ReadRaw(root, "health"));
                SetVar("mc_max_health", ReadRaw(root, "maxHealth"));
                SetVar("mc_armor", ReadRaw(root, "armor"));
                SetVar("mc_hunger", ReadRaw(root, "hunger"));
                SetVar("mc_x", ReadRaw(root, "x"));
                SetVar("mc_y", ReadRaw(root, "y"));
                SetVar("mc_z", ReadRaw(root, "z"));
                SetVar("mc_dimension", StripNamespace(ReadString(root, "dimension")));
                SetVar("mc_biome", StripNamespace(ReadString(root, "biome")));
                // UWAGA: to na razie surowy licznik tickow gry (patrz README moda),
                // nie realna pora dnia 0-24000.
                SetVar("mc_game_time", ReadRaw(root, "timeOfDay"));
                SetVar("mc_air", ReadRaw(root, "air"));
                SetVar("mc_max_air", ReadRaw(root, "maxAir"));
                SetVar("mc_xp_level", ReadRaw(root, "xpLevel"));
            }
            catch
            {
                // Niepelny/uszkodzony pakiet JSON - ignorujemy, kolejny przyjdzie za chwile.
            }
        }

        private static string ReadRaw(JsonElement root, string property)
        {
            return root.TryGetProperty(property, out var el) ? el.ToString() : "";
        }

        private static string ReadString(JsonElement root, string property)
        {
            return root.TryGetProperty(property, out var el) ? (el.GetString() ?? "") : "";
        }

        // "minecraft:sulfur_caves" -> "sulfur_caves"
        private static string StripNamespace(string value)
        {
            var idx = value.IndexOf(':');
            return idx >= 0 ? value[(idx + 1)..] : value;
        }

        private void SetVar(string name, string value)
        {
            VariableManager.SetValue(name, value, VariableType.String, _plugin, false);
        }

        private void StopInternal()
        {
            _cts?.Cancel();
            try { _ws?.Abort(); } catch { /* ignorujemy - i tak zamykamy */ }
            _ws?.Dispose();
            _ws = null;
            _cts?.Dispose();
            _cts = null;
        }

        public void Dispose()
        {
            StopInternal();
        }
    }
}
