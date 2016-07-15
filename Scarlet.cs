using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;

namespace Scarlet
{
    public partial class Scarlet : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public Scarlet()
        {

            var hostConfiguration = new HostConfiguration();
            var UrlReservations = new UrlReservations() { CreateAutomatically = true };

            var host = new NancyHost(new Uri("http://127.0.0.1:9000"));
            host.Start();

            TrayIcon();

        }

        public void TrayIcon()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", ExitApplication);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Scarlet Updater";
            trayIcon.Icon = Properties.Resources.aaf;

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.Click += new EventHandler(this.trayIcon_Click);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        protected void trayIcon_Click(object sender, EventArgs e)
        {
            ScarletUtil.openURL("http://localhost:8787/");
        }
    }
}
