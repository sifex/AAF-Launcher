using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using System.Net;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;

using MinimalJson;

using System.Diagnostics;
using WebSocketSharp.Server;

namespace Scarlet
{
    public class Scarlet : Form
    {
        public string Version = "1.0.1";
        public string scarletURL = "127.0.0.1";
        public string scarletPort = "1001";

        public string ip;
        public ScarletMetrics wsm;
        public WebSocket ws;

        // Change status codes to exceptions!
        public int status = 1;

        public string ClanID;
        public string Username;
        private string key;
        public string installDirectory;
        public string Server;
        public string ServerRepo = "http://mods.australianarmedforces.org/clans/2/repo";

        public string ModsDirName;
        public string ModsRoot;
        public string Root;

        // Statusus for Pushing on Mid-Download
        public double downloadPercentage;
        public string downloadStatus;
        public string downloadCurrentFile;

        // Tray
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu = new ContextMenu();

        // ASYNC Background Worker
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        delegate void UpdateDelegate(string text);

        public Scarlet()
        {
            // Prep for Startup
            preStartup();

            // Setup Download Linking + Routes
            Scarlet_WS_Initialise();

            // Initalise Form
            InitializeComponent();

            // Initalise Tray Icon
            TrayIcon();

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Scarlet";
            this.Load += new System.EventHandler(this.formLoad);
            this.ResumeLayout(true);

            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
        }

        public void preStartup()
        {
            ScarletUtil.testConnection(this);
            // ScarletUtil.checkVersion(this);
            ip = ScarletUtil.getExternalIP();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(onProcessExit);
        }

        public void formLoad(object sender, EventArgs e)
        {
            // ScarletUtil.openURL("http://" + scarletURL + "/");
        }
        
        public void Scarlet_WS_Initialise()
        {
            /* Scarlet Metrics */
            wsm = new ScarletMetrics();
            wsm.Send("Scarlet Service Started. (" + ip + ")");

            /* Server */
            var wssv = new WebSocketServer("ws://" + scarletURL + ":" + scarletPort);
            wssv.AddWebSocketService<Link>("/");
            wssv.Start();

            /* Client */
            ws = new WebSocket("ws://" + scarletURL + ":" + scarletPort);
            
            ws.Connect();

            ws.OnOpen += (sender, e) =>
            {
                ws.Send("Browser|Connected");
            };

            ws.OnMessage += (sender, e) =>
            {
                // Scarlet WS API
                // Words = (Browser/Updater)|(IP/*)|(Command)|(Perameters)
                string[] words = e.Data.Split('|');
                if (words[0] == "Updater")
                {
                    switch (words[1])
                    {
                        case ("browserConnect"):
                            Username = words[2];
                            key = ScarletAPI.Request("user", "info", words[2]);

                            if (backgroundWorker1.IsBusy == false)
                            {
                                ws.Send("Browser|browserConfirmation|free");
                            }
                            else
                            {
                                ws.Send("Browser|browserConfirmation|busy");
                            }
                            break;

                        case ("startDownload"):

                            installDirectory = words[2];
                            if (backgroundWorker1.IsBusy == false)
                            {
                                backgroundWorker1.RunWorkerAsync();
                            }
                            break;

                        case ("stopDownload"):

                            backgroundWorker1.CancelAsync();
                            break;

                        case ("locationChange"):
                            /*
                            installDirectory = words[2];
                            NameValueCollection postData = new NameValueCollection();
                            postData.Add("installDir", installDirectory);

                            ScarletAPI.PostRequest("user", "install", key, "", postData);
                            ws.Send("Browser|UpdateInstallLocation|" + installDirectory);
                            */
                            
                            ChooseFolder();

                            break;

                        case ("updateInstallLocation"):

                            installDirectory = words[2];
                            break;

                        case ("broadcast"):

                            MessageBox.Show(words[2], "Scarlet Updater", MessageBoxButtons.OK);
                            break;

                        case ("fetchStatus"):

                            pushStatusToBrowser();
                            break;

                        case ("quit"):

                            Application.Exit();
                            break;

                        case ("restart"):

                            Program.restarting = true;
                            Application.Restart();
                            break;
                    }
                }
            };

            ws.OnClose += (sender, e) =>
            {
                ws.Connect();
            };

        }

        public void reconnect(object sender, EventArgs e)
        {
            ws.Connect();
        }

        public void ChooseFolderTray(object sender, EventArgs e)
        {
            ChooseFolder();
        }

        public void pushStatusToBrowser()
        {
            updateStatus(downloadStatus = " ");
            updateFile(downloadCurrentFile);
            updateProgress(downloadPercentage);
        }

        /* Tray */
        public void TrayIcon()
        {
            trayMenu.MenuItems.Add("Change Install Directory", ChooseFolderTray);
            trayMenu.MenuItems.Add("Attempt Reconnect", reconnect);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Exit", ExitApplication);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Scarlet Updater";
            trayIcon.Icon = Properties.Resources.aaf;

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.DoubleClick += new EventHandler(this.trayIcon_Click);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void onProcessExit(object sender, EventArgs e)
        {
            wsm.Send("Scarlet Service Stopped. (" + ip + ")");
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            wsm.Send("Scarlet Service Stopped. (" + ip + ")");
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        protected void trayIcon_Click(object sender, EventArgs e)
        {
            ScarletUtil.openURL("http://staging.australianarmedforces.org/mods/download/");
        }

        /* Downloader */
        
        public void updateStatus(string status, string colour = null)
        {
            object[] o = new object[2];
            if (!String.IsNullOrEmpty(colour))
            {
                o[1] = colour;
            }
            o[0] = status;
            ws.Send("Browser|UpdateStatus|" + o[0] + "|" + o[1]);
        }

        public void updateFile(string file, string colour = null)
        {
            object[] o = new object[2];
            if (!String.IsNullOrEmpty(colour))
            {
                o[1] = colour;
            }
            o[0] = file;
            ws.Send("Browser|UpdateFile|" + o[0] + "|" + o[1]);

        }

        public void updateProgress(double progress, string colour = null)
        {
            object[] o = new object[2];
            if (!String.IsNullOrEmpty(colour))
            {
                o[1] = colour;
            }
            o[0] = progress;
            ws.Send("Browser|UpdateProgress|" + ((Math.Round((double)(progress * 100), 2).ToString())));

            // patchNotes.Document.InvokeScript("updateProgress", o);
        }

        public void ChooseFolder()
        {
            installDirectory = JsonObject.readFrom(ScarletAPI.Request("user", "info", Username)).get("installDir").asString();
            this.Invoke((Action) delegate {

                using (var owner = new Form()
                {
                    Width = 0,
                    Height = 0,
                    StartPosition = FormStartPosition.CenterScreen,
                    Text = "Browse for Folder"
                })
                {

                    var folderBrowser = new FolderBrowserDialog();
                    folderBrowser.Description = "Select Scarlet Installation Folder";
                    folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
                    folderBrowser.SelectedPath = installDirectory;
                    folderBrowser.ShowNewFolderButton = true;

                    owner.BringToFront();
                    if (folderBrowser.ShowDialog(this) == DialogResult.OK)
                    {
                        ws.Send("Browser|UpdateInstallLocation|" + folderBrowser.SelectedPath);
                        installDirectory = folderBrowser.SelectedPath;
                    }
                }
            });
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
            XDocument xmlRepo = XDocument.Load(this.ServerRepo + "/repo.xml");

            // Saves it to Scarlet's APPDATA Folder
            string AppDataScarlet = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Scarlet/";

            // Delete Lowercase One
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/scarlet/")) {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/scarlet/", true);
            }

            // Create Directory & Save Repo XML
            Directory.CreateDirectory(AppDataScarlet);
            xmlRepo.Save(AppDataScarlet + "repo.xml", SaveOptions.None);
            xmlRepo = XDocument.Load(AppDataScarlet + "repo.xml");

            status = 12;

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
                Vendor.FileOperationAPIWrapper.MoveToRecycleBin(dir);
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
                Vendor.FileOperationAPIWrapper.MoveToRecycleBin(file);
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
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    string str2 = xelement.Element("name").Value;
                    string a = xelement.Element("hash").Value;
                    string sUrlToReadFileFrom = this.ServerRepo + "/" + str2;
                    string str3 = Root + str2;
                    double percent = (double)filesDone / fileList;
                    if (Root != null)
                    {
                        using (MD5.Create())
                        {
                            if (System.IO.File.Exists(Root + str2))
                            {
                                this.Invoke(new Action(() => { downloadLbl_Controller(percent, 1, str2); }));
                                FileStream stream = System.IO.File.OpenRead(Root + str2);
                                string b = ScarletUtil.HashFile(stream);
                                stream.Close();
                                if (!string.Equals(a, b))
                                {
                                    File.Delete(str3);
                                    this.Invoke(new Action(() => { downloadLbl_Controller(percent, 0, str2); }));
                                    downloadFile(sUrlToReadFileFrom, str3, percent, fileList);
                                    wsm.Send("(" + ip + ") " + Username + " is Fetching File: " + str2 + " to");
                                }
                                else
                                {
                                    wsm.Send("(" + ip + ") " + Username + " Verified File: " + str2 + " to");
                                }
                            }
                            else
                            {
                                this.Invoke(new Action(() => { downloadLbl_Controller(percent, 0, str2); }));
                                downloadFile(sUrlToReadFileFrom, str3, percent, fileList);
                                wsm.Send("(" + ip + ") " + Username + " is Fetching File: " + str2 + " to");
                            }
                        }
                    }

                    filesDone++;
                    backgroundWorker1.ReportProgress((int)(percent * 710));
                }
            }
            if (filesDone == fileList)
            {
                status = 10;
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
                typer = "Verifying";
            }
            else
            {
                typer = "Downloading";
            }

            updateStatus(typer + " " /* + (Math.Round((double)(percentage * 100), 2).ToString()) + "%" */);
            updateFile(@currentFile);
            updateProgress(percentage);

            // Update Mid-Progress
            downloadPercentage = percentage;
            downloadStatus = typer;
            downloadCurrentFile = currentFile;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (status)
            {
                case 1:
                    updateStatus("Error Code: 000" + status + " - " + "Unknown Error Occurred. Please contact Server Admin.", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Unknown Error Occurred. Please contact Server Admin.");
                    break;
                case 2:
                    updateStatus("Error Code: 000" + status + " - " + "Check Version or IEChangeVersion Error.", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Check Version or IEChangeVersion Error.");
                    break;
                case 3:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to find Arma 3 Root Directory.", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Failed to find Arma 3 Root Directory.");
                    break;
                case 4:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to create " + ModsDirName + " directory. Are you sure you don't have it open?", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Failed to create " + ModsDirName + " directory. Are you sure you don't have it open?");
                    break;
                case 5:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to remove extra existing folders.", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Failed to remove extra existing folders.");
                    break;
                case 6:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to remove extra existing files.", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Failed to remove extra existing files.");
                    break;
                case 7:
                    updateStatus("Error Code: 000" + status + " - " + "Failed to successfully download mods.", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Failed to successfully download mods.");
                    break;
                case 8:
                    updateStatus("Error Code: 000" + status + " - " + "Cannot connect to update server", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Cannot connect to update server");
                    break;
                case 9:
                    updateStatus("Error Code: 000" + status + " - " + "Files Processed does not equal Files Retrieved. Please contact Server Admin.", "203, 76, 0");
                    wsm.Send("Error Code: 000" + status + " - " + "Files Processed does not equal Files Retrieved. Please contact Server Admin.");
                    break;
                case 11:
                    updateStatus("Error Code: 00" + status + " - " + "Failed to find ARMA 3 Directory. Looking for " + Root, "203, 76, 0");
                    wsm.Send("Error Code: 00" + status + " - " + "Failed to find ARMA 3 Directory. Looking for " + Root);
                    break;
                case 10:
                    updateStatus("Mods are up to date. Ready to Launch.", "100, 206, 63");
                    wsm.Send("Mods are up to date. Ready to Launch.");
                    ws.Send("Browser|Completed");
                    updateFile("");

                    updateProgress(1.0, "100, 206, 63");
                    break;
                default:
                    updateStatus("Unknown Error - Code: " + status, "203, 76, 0");
                    wsm.Send("Unknown Error - Code: " + status);
                    break;
            }
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
                    using (Stream stream2 = new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        byte[] buffer = new byte[contentLength];
                        int count;
                        while ((count = stream1.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream2.Write(buffer, 0, count);
                            num += count;
                            try { this.Invoke(new Action(() => { updateStatus("Downloading Mods (" + (num / 1024).ToString("N0") + " KB / " + (contentLength / 1024).ToString("N0") + " KB)"); })); }
                            catch (System.InvalidOperationException)
                            {

                            }
                        }
                        stream2.Close();
                    }
                    stream1.Close();
                }
            }
        }
    }
}
