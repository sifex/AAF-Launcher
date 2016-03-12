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
        
        // Checks and assigns the key to whatevers in the key file & checks validity
        public ScarletKey(string key)
        {
            this.key = key;

            if(key == "")
            {

            }
        }

        // Opens Existing Keyfile (if file exists)
        public string OpenKey()
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
                    
                    if (ScarletAPI.Request("user", "info", key, "id") == "")
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
                return key;
            }
            set
            {
                key = value;
            }
        }
    }

}
