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
        public KeyForm()
        {
            InitializeComponent();
            
            this.textBox1.Text = OpenKey();
        }        
        
        // Delete File
        static bool deleteFile(string f)
        {
            try
            {
                File.Delete(f);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
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
            deleteFile(Root + @"/scarlet_config.cfg");

            System.IO.StreamWriter file = new System.IO.StreamWriter(Root + @"/scarlet_config.cfg");
            file.WriteLine(key.Replace(System.Environment.NewLine, ""));

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
            string key = textBox1.Text.Replace(System.Environment.NewLine, "");
            SaveKey(key);
            string userID = API.Request("user", "info", key, "id");
            if (userID != "") 
            {
                this.Hide();
                var myForm = new InstallForm(key);
                myForm.Show();
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
