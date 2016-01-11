
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
            string regCheck = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", Process.GetCurrentProcess().ProcessName + ".exe", null);
            if (regCheck == null) 
            {
                int RegVal;
                int BrowserVer = 11;

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
                Process.Start(Application.ExecutablePath); // to start new instance of application
                this.Close(); //to turn off current app
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            // Defining Root String
            // Eg. "c:/program files (x86)/steam/steamapps/common/Arma 3"
            string Root = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath", @"C:\Program Files (x86)\Steam") + "/steamapps/common/Arma 3";

            changeIEVersion();

            XDocument xdocument = XDocument.Load(this.Server + "/index.php");

            string ModsRoot = Root + "/Server_Mods/";

            foreach (XElement xelement in xdocument.Descendants((XName)"modDir"))
            {
                ModsRoot = Root + "/" + xelement.Element((XName)"name").Value + "/";
            }
            if (!Directory.Exists(ModsRoot))
            {
                Directory.CreateDirectory(ModsRoot);
            }
            List<string> dbDirs = new List<string>();
            foreach (XElement xelement in xdocument.Descendants((XName)"directory"))
            {
                dbDirs.Add(Root + xelement.Element((XName)"name").Value);
            }

            List<string> allDirs = new List<string>();
            foreach (string str2 in Directory.GetDirectories(ModsRoot, "*", SearchOption.AllDirectories))
                allDirs.Add(str2.Replace("\\", "/"));
            foreach (string path2 in Enumerable.Except<string>((IEnumerable<string>)allDirs, (IEnumerable<string>)dbDirs, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    Directory.Delete(path2, true);
                }
                catch (IOException ex)
                {
                }
            }


            List<string> list3 = new List<string>();
            foreach (XElement xelement in xdocument.Descendants((XName)"file"))
                list3.Add(Root + xelement.Element((XName)"name").Value);
            List<string> list4 = new List<string>();
            foreach (string str2 in Directory.GetFiles(ModsRoot, "*", SearchOption.AllDirectories))
                list4.Add(str2.Replace("\\", "/"));
            foreach (string path2 in Enumerable.Except<string>((IEnumerable<string>)list4, (IEnumerable<string>)list3, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    System.IO.File.Delete(path2);
                }
                catch (IOException ex)
                {
                }
            }
            foreach (XContainer xcontainer in xdocument.Descendants((XName)"directory"))
            {
                string str2 = xcontainer.Element((XName)"name").Value;
                Directory.CreateDirectory(Root + str2);
            }
            foreach (XElement xelement in xdocument.Descendants((XName)"file"))
            {
                XName name1 = (XName)"name";
                string str2 = xelement.Element(name1).Value;
                XName name2 = (XName)"hash";
                string a = xelement.Element(name2).Value;
                string sUrlToReadFileFrom = this.Server + str2;
                string str3 = Root + str2;
                if (Root != null)
                {
                    using (MD5.Create())
                    {
                        if (System.IO.File.Exists(Root + str2))
                        {
                            FileStream stream = System.IO.File.OpenRead(Root + str2);
                            string b = this.HashFile(stream);
                            if (!string.Equals(a, b))
                            {
                                stream.Close();
                                Form1.deleteFile(str3);
                                this.downloadFile(sUrlToReadFileFrom, str3);
                            }
                        }
                        else
                            this.downloadFile(sUrlToReadFileFrom, str3);
                    }
                }
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
                    using (Stream stream2 = (Stream)new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        byte[] buffer = new byte[contentLength];
                        int count;
                        while ((count = stream1.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream2.Write(buffer, 0, count);
                            num += (long)count;
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