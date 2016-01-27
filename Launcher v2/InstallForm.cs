using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;

namespace AAF_Launcher
{
    public partial class InstallForm : Form
    {
        public string APIUserkey;

        public InstallForm(string key)
        {
            this.APIUserkey = key;
            InitializeComponent();
            
            var installURL = @"http://10.0.0.3/api/user/install/" + key;
            string installDir = (new WebClient()).DownloadString(installURL);

            // Defining Root String (ARMA 3 Root Direcotry)
            // Eg. "c:/program files (x86)/steam/steamapps/common/Arma 3"
            String Root = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath",null);

            if(Root == null)
            {
                return;
            }
            else
            {
                Root =  Root + "/steamapps/common/Arma 3";
            }

            textBox1.Text = Root;
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

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            
            textBox1.Text = fbd.SelectedPath + @"\@Mods_AAF";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NameValueCollection postData = new NameValueCollection();
            postData.Add("installDir", textBox1.Text);

            Post("http://10.0.0.3/api/user/install/" + APIUserkey + "/", postData);

            this.Hide();
            var myForm = new Form1(APIUserkey);
            myForm.Show();
        }
    }
}
