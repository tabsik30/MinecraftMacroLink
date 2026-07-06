using System;
using System.Drawing;
using System.Windows.Forms;
using SuchByte.MacroDeck.Plugins;

namespace tabsik12.MacroLink
{
    public class MacroLinkConfigurator : Form
    {
        private readonly Main _plugin;
        private TextBox _txtHost = null!;
        private TextBox _txtPort = null!;

        public MacroLinkConfigurator(Main plugin)
        {
            _plugin = plugin;
            InitializeComponent();
            LoadConfig();
        }

        private void InitializeComponent()
        {
            Text = "MacroLink - konfiguracja";
            Width = 340;
            Height = 220;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var lblHost = new Label
            {
                Text = "Adres moda (host):",
                Location = new Point(20, 20),
                AutoSize = true
            };

            _txtHost = new TextBox
            {
                Location = new Point(20, 45),
                Width = 280
            };

            var lblPort = new Label
            {
                Text = "Port:",
                Location = new Point(20, 80),
                AutoSize = true
            };

            _txtPort = new TextBox
            {
                Location = new Point(20, 105),
                Width = 280
            };

            var btnSave = new Button
            {
                Text = "Zapisz",
                Location = new Point(20, 145),
                Width = 100
            };
            btnSave.Click += BtnSave_Click;

            var btnCancel = new Button
            {
                Text = "Anuluj",
                Location = new Point(200, 145),
                Width = 100
            };
            btnCancel.Click += BtnCancel_Click;

            Controls.Add(lblHost);
            Controls.Add(_txtHost);
            Controls.Add(lblPort);
            Controls.Add(_txtPort);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
        }

        private void LoadConfig()
        {
            var host = PluginConfiguration.GetValue(_plugin, "host") as string;
            var port = PluginConfiguration.GetValue(_plugin, "port") as string;

            _txtHost.Text = string.IsNullOrWhiteSpace(host) ? "127.0.0.1" : host;
            _txtPort.Text = string.IsNullOrWhiteSpace(port) ? "25599" : port;
        }

        private void SaveAndClose()
        {
            PluginConfiguration.SetValue(_plugin, "host", _txtHost.Text.Trim());
            PluginConfiguration.SetValue(_plugin, "port", _txtPort.Text.Trim());

            _plugin.ReloadConnection();
            Close();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            SaveAndClose();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
