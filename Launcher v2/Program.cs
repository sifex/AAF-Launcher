using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;

namespace AAF_Launcher
{
    static class Program
    {
        public static string ScarletAPI = "http://scarlet.australianarmedforces.org/";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(OpenKey() == "")
            {
                Application.Run(new KeyForm());
            }
            else if (OpenKey() == "invalid")
            {
                MessageBox.Show("Invalid Key File");
                Application.Run(new KeyForm());
            }
            else if(API.Request("user", "install", OpenKey()) == "")
            {
                Application.Run(new InstallForm(OpenKey()));
            }
            else
            {
                Application.Run(new Form1(OpenKey()));
            }
        }
        
        public static string OpenKey()
        {
            // Grab Steam Location
            String Root = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath", null);
            String Key;
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
                    Key = File.ReadAllText(Root + @"/scarlet_config.cfg").Replace(System.Environment.NewLine, "");
                }
                else
                {
                    return "";
                }
            }

            if(API.Request("user", "info", Key, "id") == "") {
                return "invalid";
            }
            else
            {
                return Key;
            }
            
        }

    }
}
