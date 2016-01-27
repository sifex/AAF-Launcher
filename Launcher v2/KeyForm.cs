using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using Microsoft.Win32;

namespace AAF_Launcher
{
    public partial class KeyForm : Form
    {
        public KeyForm(bool force = false)
        {
            if(OpenKey() != "" || force == false)
            {
                this.Hide();
                var Form1 = new Form1(OpenKey());
                Form1.Show(); 
            }
            else
            {
                InitializeComponent();
            }
        }

        public static byte[] Post(string uri, NameValueCollection pairs)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(uri, pairs);
            }
            return response;
        }

        public void SaveKey(string key)
        {
            // Grab Steam Location
            String Root = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath",null);

            if(Root == null)
            {
                return;
            }
            else
            {   
                // Append Arma 3 Location
                Root =  Root + "/steamapps/common/Arma 3";
            }

            // Save Key
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter(Root + @"/scarlet_config.cfg");
            file.WriteLine(key);

            file.Close();
        }

        public string OpenKey()
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
                if(File.Exists(Root + @"/scarlet_config.cfg"))
                {
                    return File.ReadAllText(Root + @"/scarlet_config.cfg");
                }
                else
                {
                    return "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            string key = textBox1.Text;
            var userIDURL = @"http://10.0.0.3/api/user/info/" + key + "/id/";
            string userID = (new WebClient()).DownloadString(userIDURL);
            if (userID != "") 
            {
                this.Hide();
                var installURL = @"http://10.0.0.3/api/user/install/" + key + "/";
                string installDir = (new WebClient()).DownloadString(installURL);

                // Saves Key to ARMA 3 Local
                SaveKey(key);

                if (installDir == "")
                {
                    var myForm = new InstallForm(key);
                    myForm.Show();
                }
                else
                {
                    var myForm = new Form1(key);
                    myForm.Show();
                }
            }
            else
            {
                MessageBox.Show("Key Does not exist");
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeBtn_MouseEnter(object sender, EventArgs e)
        {
            closeBtn.BackgroundImage = Properties.Resources.close1;
        }

        private void closeBtn_MouseLeave(object sender, EventArgs e)
        {
            closeBtn.BackgroundImage = Properties.Resources.close2;
        }
    }
}
