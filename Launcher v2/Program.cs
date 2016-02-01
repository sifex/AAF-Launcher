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
            if(PersonalAccessKey.OpenKey() == "")
            {
                Application.Run(new KeyForm());
            }
            else if (PersonalAccessKey.OpenKey() == "invalid")
            {
                MessageBox.Show("Invalid Key File");
                Application.Run(new KeyForm());
            }
            else if(API.Request("user", "install", PersonalAccessKey.OpenKey()) == "")
            {
                Application.Run(new InstallForm(PersonalAccessKey.OpenKey()));
            }
            else
            {
                Application.Run(new Form1(PersonalAccessKey.OpenKey()));
            }
        }
        
        

    }
}
