using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace AAF_Launcher
{
    class Launch
    {
        public static void LaunchScarlet()
        {
            if (OpenKey() != "")
            {
                Application.Run(new Form1(OpenKey()));
            }
            else
            {
                Application.Run(new KeyForm());
            }
        }

        public static string OpenKey()
        {
            // Grab Steam Location
            String Root = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath", null);

            if (Root == null)
            {
                return "";
            }
            else
            {
                // Append Arma 3 Location
                Root = Root + "/steamapps/common/Arma 3";
                if (File.Exists(Root + @"/scarlet_config.cfg"))
                {
                    return File.ReadAllText(Root + @"/scarlet_config.cfg");
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
