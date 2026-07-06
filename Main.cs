using SuchByte.MacroDeck.Plugins;

namespace tabsik12.MacroLink
{
    public class Main : MacroDeckPlugin
    {
        private MacroLinkClient? _client;

        public Main()
        {
            // Dane opisowe bierzemy z ExtensionManifest.json.
        }

        public override void Enable()
        {
            _client = new MacroLinkClient(this);
            _client.Start();
        }

        public override void OpenConfigurator()
        {
            using (var configurator = new MacroLinkConfigurator(this))
            {
                configurator.ShowDialog();
            }
        }

        internal void ReloadConnection()
        {
            _client?.Restart();
        }
    }
}
