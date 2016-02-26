using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace AAF_Launcher
{
    partial class Util : Form1
    {
        // Opens Existing Keyfile (if file exists)
        public static string OpenKey()
        {
            string key;
            
            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = folder + "/scarlet";

            // Check if folder exists and if not, create it
            if (Directory.Exists(specificFolder))
            {
                if (File.Exists(specificFolder + @"/scarlet_config.cfg"))
                {
                    key = File.ReadAllText(specificFolder + @"/scarlet_config.cfg").Replace(System.Environment.NewLine, "");
                    
                    if (API.Request("user", "info", key, "id") == "")
                    {
                        throw new WebException("Key doesn't exist in Scarlet Database");
                    }
                    else
                    {
                        return key;
                    }

                }
                else
                {
                    throw new FileNotFoundException("No Key Found");
                }
            }
            else
            {
                throw new FileNotFoundException("No Key Found");
            }

        }

        // Saves Key to App File
        public static bool SaveKey(string key)
        {

            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = folder + "/scarlet";

            // Check if folder exists and if not, create it
            if (!Directory.Exists(specificFolder))
            {
                Directory.CreateDirectory(specificFolder);
            }


            if (string.Equals(API.Request("user", "info", key, "id"), ""))
            {
                return false;
            }
            else
            {
                // Compose a string that consists of three lines.
                string lines = key;

                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(specificFolder + "/scarlet_config.cfg");
                file.WriteLine(lines);

                file.Close();
                return true;
            }

            
        }

        // Saves Key to App File
        public static void DeleteKey()
        {
            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = folder + "/scarlet";

            File.Delete(specificFolder + "/scarlet_config.cfg");
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
        
        private static bool ServerStatus(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                int num1 = 30000;
                httpWebRequest.Timeout = num1;
                int num2 = 0;
                httpWebRequest.AllowAutoRedirect = num2 != 0;
                string str = "HEAD";
                httpWebRequest.Method = str;
                using (httpWebRequest.GetResponse())
                    return true;
            }
            catch
            {
                return false;
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

    }

}
