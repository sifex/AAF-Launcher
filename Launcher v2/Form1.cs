
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Launcher_v2
{
    public partial class Form1 : Form
    {
        // Set Server Variable. This should be where index.php, html and the mods folder should be.
        // Eg: http://exmaple.com/Arma3Updater/
        public string Server = "http://mods.australianarmedforces.org/";

        public const int WM_NCLBUTTONDOWN = 161;
        public const int HT_CAPTION = 2;
        
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            changeIEVersion();

            InitializeComponent();

            // Download progress
            backgroundWorker1.RunWorkerAsync();

            // Disable Start Game Button for now
            strtGameBtn.Enabled = false;
        }

        // Makes the main window (Form1) dragable
        private void titleBarPanel_Mousedown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // Close Button
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeBtn_MouseEnter(object sender, EventArgs e)
        {
            closeBtn.BackgroundImage = Properties.Resources.close1;
        }

        private void closeBtn_MouseLeave(object sender, EventArgs e)
        {
            closeBtn.BackgroundImage = Properties.Resources.close2;
        }

        // Minimize Button
        private void minimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void minimizeBtn_MouseEnter(object sender, EventArgs e)
        {
            minimizeBtn.BackgroundImage = Properties.Resources.minimize1;
        }

        private void minimizeBtn_MouseLeave(object sender, EventArgs e)
        {
            minimizeBtn.BackgroundImage = Properties.Resources.minimize2;
        }

        //Delete File
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

        public string HashFile(FileStream stream)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (stream != null)
            {
                stream.Seek(0L, SeekOrigin.Begin);
                foreach (byte num in MD5.Create().ComputeHash((Stream)stream))
                    stringBuilder.Append(num.ToString("x2"));
                stream.Seek(0L, SeekOrigin.Begin);
            }
            return stringBuilder.ToString();
        }

        private bool ServerStatus(string url)
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

        private void changeIEVersion()
        {
            object regCheck = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", Process.GetCurrentProcess().ProcessName + ".exe", null);
            if (regCheck == null) 
            {
                int RegVal;
                int BrowserVer = 9;

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
                RegistryKey Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                Key.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);
                Key.Close();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            // Defining Root String (ARMA 3 Root Direcotry)
            // Eg. "c:/program files (x86)/steam/steamapps/common/Arma 3"
            string Root = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath", @"C:\Program Files (x86)\Steam") + "/steamapps/common/Arma 3";
           
            // Loads Server Repos XML
            XDocument xdocument = XDocument.Load(this.Server + "/index.php");
            
            // Fetches ModRoot Directory from Server, or Defaults to /Server_Mods/
            // Don't use /Mods/ as a folder as your clan will bug you to ask where their custom 1337 mods have gone! 
            // Eg. Root + /Mods_AAF/
            string ModsRoot = Root + "/Server_Mods/";
            foreach (XElement xelement in xdocument.Descendants((XName)"modDir"))
            {
                ModsRoot = Root + "/" + xelement.Element((XName)"name").Value + "/";
            }

            // Create Directory if it doesn't already exist (Creates ModRoot)
            if (!Directory.Exists(ModsRoot))
            {
                Directory.CreateDirectory(ModsRoot);
            }

            /////////////////////////////////////////////////////////////////////////////////
            // Deletes all Directories other than ones that exist in the Server Repos List //
            /////////////////////////////////////////////////////////////////////////////////

            // Fetches all Dirs from XML
            List<string> dbDirs = new List<string>();
            foreach (XElement dir in xdocument.Descendants((XName)"directory"))
            {
                dbDirs.Add(Root + dir.Element((XName)"name").Value);
            }
            // Fetches all Dirs from Local
            List<string> allDirs = new List<string>();
            foreach (string dir in Directory.GetDirectories(ModsRoot, "*", SearchOption.AllDirectories))
            {
                allDirs.Add(dir.Replace("\\", "/"));
            }
            // Delete the difference
            foreach (string dir in Enumerable.Except<string>((IEnumerable<string>)allDirs, (IEnumerable<string>)dbDirs, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase))
            {
                Directory.Delete(dir, true);
            }

            ///////////////////////////////////////////////////////////////////////////
            // Deletes all Files other than ones that exist in the Server Repos List //
            ///////////////////////////////////////////////////////////////////////////

            // Fetches all Files from XML
            List<string> dbFiles = new List<string>();
            foreach (XElement file in xdocument.Descendants((XName)"file"))
            {
                dbFiles.Add(Root + file.Element((XName)"name").Value);
            }
            // Fetches all Files from Local
            List<string> allFiles = new List<string>();
            foreach (string file in Directory.GetFiles(ModsRoot, "*", SearchOption.AllDirectories))
            {
                allFiles.Add(file.Replace("\\", "/"));
            }
            // Delete the difference
            foreach (string file in Enumerable.Except<string>((IEnumerable<string>)allFiles, (IEnumerable<string>)dbFiles, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase))
            {
                System.IO.File.Delete(file);
            }



            foreach (XContainer xcontainer in xdocument.Descendants("directory"))
            {
                string str2 = xcontainer.Element("name").Value;
                Directory.CreateDirectory(Root + str2);
            }
            foreach (XElement xelement in xdocument.Descendants("file"))
            {
                string str2 = xelement.Element("name").Value;
                string a = xelement.Element("hash").Value;
                string sUrlToReadFileFrom = this.Server + str2;
                string str3 = Root + str2;
                if (Root != null)
                {
                    using (MD5.Create())
                    {
                        if (System.IO.File.Exists(Root + str2))
                        {
                            FileStream stream = System.IO.File.OpenRead(Root + str2);
                            string b = HashFile(stream);
                            if (!string.Equals(a, b))
                            {
                                stream.Close();
                                Form1.deleteFile(str3);
                                this.downloadFile(sUrlToReadFileFrom, str3);
                            }
                        }
                        else
                        {
                            this.downloadFile(sUrlToReadFileFrom, str3);
                        }
                    }
                }
                progressBar2.PerformStep();
            }
        }

        public void downloadFile(string sUrlToReadFileFrom, string sFilePathToWriteFileTo)
        {
            HttpWebResponse httpWebResponse = (HttpWebResponse)WebRequest.Create(new Uri(sUrlToReadFileFrom)).GetResponse();
            httpWebResponse.Close();
            long contentLength = httpWebResponse.ContentLength;
            long num = 0;
            using (WebClient webClient = new WebClient())
            {
                using (Stream stream1 = webClient.OpenRead(new Uri(sUrlToReadFileFrom)))
                {
                    using (Stream stream2 = new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        byte[] buffer = new byte[contentLength];
                        int count;
                        while ((count = stream1.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream2.Write(buffer, 0, count);
                            num += count;
                            this.backgroundWorker1.ReportProgress((int)((double)num / (double)buffer.Length * 100.0));
                        }
                        stream2.Close();
                    }
                    stream1.Close();
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            this.downloadLbl.ForeColor = System.Drawing.Color.Silver;
            this.downloadLbl.Text = "Downloading Updates";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.strtGameBtn.Enabled = true;
            if (this.ServerStatus(this.Server))
            {
                this.downloadLbl.ForeColor = System.Drawing.Color.FromArgb(0, 121, 203);
                this.downloadLbl.Text = "Mods are up to date";
            }
            else
            {
                this.downloadLbl.ForeColor = System.Drawing.Color.FromArgb(203, 76, 0);
                this.downloadLbl.Text = "Cannot connect to update server";
            }
        }

        //Starts the game
        private void strtGameBtn_Click(object sender, EventArgs e)
        {
            Process.Start("SFrame.exe", "/auth_ip: 127.0.0.1 /locale:ASCII /country:US /cash /commercial_shop");
            this.Close();
        }
    }
}