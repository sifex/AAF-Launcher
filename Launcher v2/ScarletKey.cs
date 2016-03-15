using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Scarlet
{
    class ScarletKey
    {
        private string key;

        // Opens Existing Keyfile (if file exists)
        public string OpenKey()
        {
            
            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = folder + "/scarlet";

            // Check if folder exists and if not, create it
            if (Directory.Exists(specificFolder))
            {
                if (File.Exists(specificFolder + @"/scarlet_config.cfg"))
                {
                    this.key = File.ReadAllText(specificFolder + @"/scarlet_config.cfg").Replace(System.Environment.NewLine, "");

                    return this.key;

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
        public bool SaveKey(string key)
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


            if (string.Equals(ScarletAPI.Request("user", "info", key, "id"), ""))
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
        public void DeleteKey()
        {
            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = folder + "/scarlet";

            File.Delete(specificFolder + "/scarlet_config.cfg");
        }

        public string Key
        {
            get
            {
                try {
                    key = OpenKey();
                    try { ScarletAPI.Request("user", "info", key, "id"); }
                    catch
                    {
                        key = "";
                    }
                }
                catch
                {
                    key = "";
                }
                return key;
            }
            set
            {
                SaveKey(value);
            }
        }
    }

}
