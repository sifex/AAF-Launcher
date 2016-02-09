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
using System.Xaml;
using Microsoft.VisualBasic;
using System.Windows;

namespace AAF_Launcher
{
    public partial class Form1 : Form
    {
        // Set Server Variable. This should be where index.php, html and the mods folder should be.
        public string Server = "http://mods.australianarmedforces.org/";
        public string ScarletAPI = "http://scarlet.australianarmedforces.org/";
        public string Version = "0.7";
        public int status = 1;
        public string ModsDirName;
        public string ModsRoot;
        public string Root;
        public string key = Util.OpenKey();
        public string Username = API.Request("user", "info", Util.OpenKey(), "username");
        public string installDirectory;

        public bool WorkerSupportsCancellation = true;
        public const int WM_NCLBUTTONDOWN = 161;
        public const int HT_CAPTION = 2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        
        public Form1()
        {
            checkVersion();
            this.installDirectory = API.Request("user", "install", key);
            Util.changeIEVersion(11);

            status = 2;

            // UI Initialize
            InitializeComponent();
            postStartup();

            // Download progress

            // Disable Start Game Button for now
            // strtGameBtn.Enabled = false;

            this.strtGameBtn.Text = "Begin Update";
            this.strtGameBtn.Click += new System.EventHandler(this.update_Click);
        }

        public void postStartup()
        {
            // Set Username
            Username = API.Request("user", "info", key, "username");
            this.patchNotes.Refresh(WebBrowserRefreshOption.Completely);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            updateStatus("Hi " + Util.FirstCharToUpper(Username) + ".");
            updateFile("Current Install Directory is: " + installDirectory);
        }

        // On Update Button Click
        public void update_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            updateStatus("Fetching Repo");
            updateFile("");

            this.strtGameBtn.Click -= new System.EventHandler(this.update_Click);
            this.strtGameBtn.Click += new System.EventHandler(this.strtGameBtn_Click);
            this.strtGameBtn.Enabled = false;
        }

        // Check Current version on Server
        public void checkVersion()
        {
            var versionURL = Server + "version.txt";
            var versionNo = (new WebClient()).DownloadString(versionURL);

            if(versionNo != Version)
            {
                System.Windows.Forms.MessageBox.Show("This version is out of date, please download the updated version.");
                Process.Start("http://mods.australianarmedforces.org/?update");
                Environment.Exit(0);
            }
        }

        public void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if(e.Url.ToString() == @"http://australianarmedforces.org/" || e.Url.ToString() == @"http://development.australianarmedforces.org/projects/SCARLET/issues")
            {
                //cancel the current event
                e.Cancel = true;

                //this opens the URL in the user's default browser
                Process.Start(e.Url.ToString());
            }
            else
            {

            }
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
            System.Windows.Forms.Application.Exit();
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

        public void updateStatus(string status, string colour = null)
        {
            if (!String.IsNullOrEmpty(colour))
            {
                object[] o = new object[2];
                o[0] = status;
                o[1] = colour;
                patchNotes.Document.InvokeScript("updateStatus", o);
            }
            else
            {
                object[] o = new object[1];
                o[0] = status;
                patchNotes.Document.InvokeScript("updateStatus", o);
            }
        }

        public void updateFile(string file, string colour = null)
        {
            if(!String.IsNullOrEmpty(colour))
            {
                object[] o = new object[2];
                o[0] = file;
                o[1] = colour;
                patchNotes.Document.InvokeScript("updatefile", o);
            }
            else
            {
                object[] o = new object[1];
                o[0] = file;
                patchNotes.Document.InvokeScript("updatefile", o);
            }
        }
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            Root = this.installDirectory.Replace(@"\", @"/");

            if (!Directory.Exists(Root))
            {
                status = 11;
                return;
            }

            status = 8;

            // Loads Server Repos XML
            XDocument xmlRepo = XDocument.Load(this.Server + "/repo.xml");
            
            // Fetches ModRoot Directory from Server, or Defaults to /Server_Mods/
            // Don't use /Mods/ as a folder as your clan will bug you to ask where their custom 1337 mods have gone! 
            // Eg. Root + /Mods_AAF/
            ModsRoot = Root + "/@Server_Mods/";
            foreach (XElement xelement in xmlRepo.Descendants((XName)"modDir"))
            {
                ModsDirName = xelement.Element((XName)"name").Value;
                ModsRoot = Root + "/" + xelement.Element((XName)"name").Value + "/";
            }

            status = 4;

            // Create Directory if it doesn't already exist (Creates ModRoot)
             if (!Directory.Exists(ModsRoot))
            {
                Directory.CreateDirectory(ModsRoot);
            }

            status = 5;

            /////////////////////////////////////////////////////////////////////////////////
            // Deletes all Directories other than ones that exist in the Server Repos List //
            /////////////////////////////////////////////////////////////////////////////////
            
            // Fetches all Dirs from XML
            List<string> dbDirs = new List<string>();
            foreach (XElement dir in xmlRepo.Descendants((XName)"directory"))
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
                FileOperationAPIWrapper.MoveToRecycleBin(dir);
            }

            status = 6;

            ///////////////////////////////////////////////////////////////////////////
            // Deletes all Files other than ones that exist in the Server Repos List //
            ///////////////////////////////////////////////////////////////////////////

            // Fetches all Files from XML
            List<string> dbFiles = new List<string>();
            foreach (XElement file in xmlRepo.Descendants((XName)"file"))
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
                FileOperationAPIWrapper.MoveToRecycleBin(file);
            }

            status = 7;

            int filesDone = 0;
            int fileList = xmlRepo.Descendants("file").Count();
            foreach (XContainer xcontainer in xmlRepo.Descendants("directory"))
            {
                string str2 = xcontainer.Element("name").Value;
                Directory.CreateDirectory(Root + str2);
            }
            foreach (var xelement in xmlRepo.Descendants("file"))
            {
                if(backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                } 
                else
                {
                    string str2 = xelement.Element("name").Value;
                    string a = xelement.Element("hash").Value;
                    string sUrlToReadFileFrom = this.Server + str2;
                    string str3 = Root + str2;
                    double percent = (double)filesDone/fileList;
                    if (Root != null)
                    {
                        using (MD5.Create())
                        {
                            if (System.IO.File.Exists(Root + str2))
                            {
                                FileStream stream = System.IO.File.OpenRead(Root + str2);
                                string b = Util.HashFile(stream);
                                if (!string.Equals(a, b))
                                {
                                    stream.Close();
                                    FileOperationAPIWrapper.MoveToRecycleBin(str3);
                                    this.downloadFile(sUrlToReadFileFrom, str3, percent, fileList);
                                    this.Invoke(new Action(() => { downloadLbl_Controller(percent, 0, str2); }));
                                }
                                else
                                {
                                    this.Invoke(new Action(() => { downloadLbl_Controller(percent, 1, str2); }));
                                }
                            }
                            else
                            {
                                this.downloadFile(sUrlToReadFileFrom, str3, percent, fileList);
                                this.Invoke(new Action(() => { downloadLbl_Controller(percent, 0, str2); }));
                            }
                        }
                    } 
                    filesDone++;
                    backgroundWorker1.ReportProgress((int)(percent * 710));
                }
            }
            if(filesDone == fileList)
            {
                status = 10;
                this.strtGameBtn.Text = "Launch";

            }
            else
            {
                status = 9;
            }
        }

        public void downloadLbl_Controller(double percentage, int type, string currentFile)
        {
            string typer;
            if (type == 1)
            {
                typer = "Verifying Mods";
                updateStatus(typer);
                updateFile("");
            }
            else
            {
                typer = "Downloading Mods";
                updateStatus(typer);
                updateFile("");
            }
            
            updateStatus(typer + " - " + (Math.Round((double)(percentage * 100), 2).ToString()) + "%");
            updateFile(@currentFile);
        }

        public void downloadFile(string sUrlToReadFileFrom, string sFilePathToWriteFileTo, double percent, int fileList)
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
                            backgroundWorker1.ReportProgress((int)(percent * 710));
                        }
                        stream2.Close();
                    }
                    stream1.Close();
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pictureBox1.Size = new System.Drawing.Size(e.ProgressPercentage, 31);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.strtGameBtn.Enabled = true;
            switch (status)
            {
                case 1:
                    updateStatus("Error Code: 000" + status + " - " + "Unknown Error Occurred. Please contact Server Admin.", "203, 76, 0");
                    break;
                case 2:
                    updateStatus("Error Code: 000" + status + " - " + "Check Version or IEChangeVersion Error.", "203, 76, 0");
                    break;
                case 3:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to find Arma 3 Root Directory.", "203, 76, 0");
                    break;
                case 4:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to create " + ModsDirName + " directory. Are you sure you don't have it open?", "203, 76, 0");
                    break;
                case 5:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to remove extra existing folders.", "203, 76, 0");
                    break;
                case 6:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to remove extra existing files.", "203, 76, 0");
                    break;
                case 7:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to successfully download mods.", "203, 76, 0");
                    break;
                case 8:
                    updateStatus("Error Code: 000" + status + " - " + "Cannot connect to update server", "203, 76, 0");
                    break;
                case 9:
                    updateStatus("Error Code: 000" + status + " - " + "Files Processed does not equal Files Retrieved. Please contact Server Admin.", "203, 76, 0");
                    break;
                case 11:
                    updateStatus("Error Code: 00" + status + " - " + "Failed to find ARMA 3 Directory. Looking for " + Root, "203, 76, 0");
                    break;
                case 10:
                    updateStatus("Mods are up to date. Ready to Launch.", "100, 206, 63");
                    updateFile("");
                    this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(206)))), ((int)(((byte)(63))))); ;
                    break;
                default:
                    updateStatus("Unknown Error - Code: " + status, "203, 76, 0");
                    break;
            }
        }

        // Starts the game
        private void strtGameBtn_Click(object sender, EventArgs e)
        {
            Process.Start(Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath", @"C:\Program Files (x86)\Steam") + "/steamapps/common/Arma 3" + "/arma3.exe", "-nosplash -skipIntro");
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            // this.Hide();
            // var Form1 = new KeyForm();
            // Form1.Show();
        }
    }
}