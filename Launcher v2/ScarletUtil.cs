using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Scarlet
{
    class ScarletUtil
    {
        // Check Current version on Server
        public static void testConnection()
        {
            try { ScarletAPI.Request(""); }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message.ToString(), "No connection to Scarlet Servers.");
                Environment.Exit(1);
            }
        }

        // Check Current version on Server
        public static void checkVersion(string Version)
        {
            var versionURL = "http://mods.australianarmedforces.org/version.txt";
            var versionNo = "";
            try
            {
                versionNo = (new WebClient()).DownloadString(versionURL);
            }
            catch (System.Net.WebException)
            {

            }
            if (versionNo != Version)
            {
                /* System.Windows.Forms.MessageBox.Show("This version is out of date, please download the updated version.");
                Process.Start("http://mods.australianarmedforces.org/?update");
                Environment.Exit(0); */
            }
        }

        // Generates MD5 Hash of File
        public static string HashFile(FileStream stream)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (stream != null)
            {
                stream.Seek(0L, SeekOrigin.Begin);
                foreach (byte num in MD5.Create().ComputeHash((Stream)stream))
                {
                    stringBuilder.Append(num.ToString("x2"));
                }
                stream.Seek(0L, SeekOrigin.Begin);
            }
            return stringBuilder.ToString();
        }

        // Changes the IE Version within the Registry
        public static void changeIEVersion(int version)
        {
            object regCheck = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", Process.GetCurrentProcess().ProcessName + ".exe", null);
            if (regCheck == null)
            {
                int RegVal;
                int BrowserVer = version;

                // Set the appropriate IE version
                if (BrowserVer >= 11)
                    RegVal = 11001;
                else if (BrowserVer == 10)
                    RegVal = 10001;
                else if (BrowserVer == 9)
                    RegVal = 9999;
                else if (BrowserVer == 8)
                    RegVal = 8888;
                else
                    RegVal = 7000;

                // set the actual key

                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
                RegistryKey Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

                Key.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);
                Key.Close();
            }
        }

        // First Character to Uppercase
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentException("ARGH!");
            }
            else
            {
                return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        // Gets all the mods in the downloaded folder
        public static string getModListForGameExec(string ModsRoot, string ModsDirName)
        {
            string stringReturn = "";
            foreach (string subdirectoryEntries in Directory.GetDirectories(ModsRoot))
            {
                stringReturn += ModsDirName + "/" + subdirectoryEntries.Replace(ModsRoot, "") + ";";
            }
            return stringReturn;
        }

        // Open new Web Browser with the following URL
        public static void openURL(string URL)
        {
            Process.Start(URL);
        }
    }
}
