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
using System.Security.Permissions;

namespace Scarlet
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Scarlet : Form
    {
        public string Version = "Macaw";

        // Change status codes to exceptions!
        public int status = 1;

        // Set as ClanAPI -> URL -> (User API -> Key -> Clanid)
        public string ClanID;
        public string Username;
        public string installDirectory;
        public string Server;
        public string ServerRepo;

        public string ModsDirName;
        public string ModsRoot;
        public string Root;

        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                ClanID = ScarletAPI.Request("user", "info", value, "clanid");
                Username = ScarletAPI.Request("user", "info", value, "username");
                installDirectory = ScarletAPI.Request("user", "info", value, "installDir");
                Server = "http://mods.australianarmedforces.org/clans/" + ClanID + "";
                ServerRepo = Server + "/repo/";
                
            }
        }

        public const int WM_NCLBUTTONDOWN = 161;
        public const int HT_CAPTION = 2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        
        public Scarlet()
        {
            // Prep for Startup
            preStartup();

            // UI Initialize
            InitializeComponent();

            // Post for Startup (UI Configuration)
            postStartup();
        }

        public void preStartup()
        {
            //  Test Connection to API Scarlet Servers 
            //      Does:       Sends a blank API Request to the Scarlet Servers
            //      Returns:    void or Application Exit
            ScarletUtil.testConnection();

            //  Check's Application Version
            //      Does:       Downloads and reads version.txt, compares with imbedded version string
            //      Returns:    void or Application Exit
            ScarletUtil.checkVersion(Version);
            
            //  Scarlet Change Internet Explorer Version
            //      Does:       Changes the registry value for the application to use a more modern version of IE
            //      Returns:    void
            ScarletUtil.changeIEVersion(11);

            //  ScarletKey Object
            //      Does:       Sets key to the existing key file, deletes if doesn't exist.
            //      Returns:    void
            //      $Key:       Set to the Users Key
            //      $noKey:     Set if Util.OpenKey() returns a FileNotFoundException
            ScarletKey ScarKey = new ScarletKey();
            if(ScarKey.Key != "")
            {
                Key = ScarKey.Key;
            }
        }

        public void saveKeyAndRestart(string input)
        {
            ScarletKey ScarKey = new ScarletKey();
            ScarKey.Key = input;
            if (ScarKey.Key == input)
            {
                Key = input;
                this.patchNotes.Url = new System.Uri(Server + "/html/?scarletKey=" + Key, System.UriKind.Absolute);
            }
        }

        public void postStartup()
        {
            if(Key == null)
            {
                this.patchNotes.Url = new System.Uri(ScarletAPI.ScarletURL + "/key/?authKey=" + Key, System.UriKind.Absolute);
            }
            else
            {
                this.patchNotes.Url = new System.Uri(Server + "/html/?scarletKey=" + Key, System.UriKind.Absolute);
            }

            this.patchNotes.ObjectForScripting = this;
            this.patchNotes.Refresh(WebBrowserRefreshOption.Completely);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try {
                if (patchNotes.Url == new System.Uri(Server + "/html/?scarletKey=" + Key, System.UriKind.Absolute))
                {
                    refreshStatus();
                }
            }
            catch
            {

            }
        }

        // On Update Button Click
        public void update_Click()
        {
            backgroundWorker1.RunWorkerAsync(patchNotes.Document.Body.InnerText);
            updateStatus("Fetching Repo");
            updateFile("");
            patchNotes.Document.InvokeScript("disableConfig");
            patchNotes.Document.InvokeScript("disableStart");
        }

        // Makes the main window (Form1) dragable
        public void drag()
        {
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        // Close Button
        public void closeBtn_Click()
        {
            System.Windows.Forms.Application.Exit();
        }

        // Minimize Button
        public void minimizeBtn_Click()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Refresh the Status of Welcome Message
        public void refreshStatus()
        {
            installDirectory = ScarletAPI.Request("user", "info", Key, "installDir");
            updateStatus("Hi " + ScarletUtil.FirstCharToUpper(Username) + ".");
            updateFile("Current Install Directory is: " + installDirectory);
        }

        public void updateStatus(string status, string colour = null)
        {
            object[] o = new object[2];
            if (!String.IsNullOrEmpty(colour))
            {
                o[1] = colour;
           }
            o[0] = status;
            patchNotes.Document.InvokeScript("updateStatus", o);
        }

        public void updateFile(string file, string colour = null)
        {
            object[] o = new object[2];
            if (!String.IsNullOrEmpty(colour))
            { 
                o[1] = colour;
            }
            o[0] = file;
            patchNotes.Document.InvokeScript("updatefile", o);
            
        }

        public void updateProgress(double progress, string colour = null)
        {
            object[] o = new object[2];
            if (!String.IsNullOrEmpty(colour))
            {
                o[1] = colour;
            }
            o[0] = progress;
            patchNotes.Document.InvokeScript("updateProgress", o);
        }

        public void ChooseFolder()
        {
            // New FolderBrowserDialog instance
            FolderBrowserDialog Fld = new FolderBrowserDialog();

            // Show the "Make new folder" button
            Fld.ShowNewFolderButton = true;

            if (Fld.ShowDialog() == DialogResult.OK)
            {
                // Select successful
                object[] o = new object[1];
                o[0] = Fld.SelectedPath + "";
                patchNotes.Document.InvokeScript("fillPath", o);
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
            XDocument xmlRepo = XDocument.Load(this.ServerRepo + "/main/repo.xml");
            
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
                if(backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                } 
                else
                {
                    string str2 = xelement.Element("name").Value;
                    string a = xelement.Element("hash").Value;
                    string sUrlToReadFileFrom = this.ServerRepo + "/main/" + str2;
                    string str3 = Root + str2;
                    double percent = (double)filesDone/fileList;
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
                                }
                            }
                            else
                            {
                                this.Invoke(new Action(() => { downloadLbl_Controller(percent, 0, str2); }));
                                this.downloadFile(sUrlToReadFileFrom, str3, percent, fileList);
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
            updateProgress(percentage);
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
                            try { this.Invoke(new Action(() => { updateStatus("Downloading Mods (" + (num / 1024).ToString("N0") + " KB / " + (contentLength / 1024).ToString("N0") + " KB)"); })); }
                            catch(System.InvalidOperationException)
                            {

                            }
                        }
                        stream2.Close();
                    }
                    stream1.Close();
                }
            }
        } 

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            patchNotes.Document.InvokeScript("enableConfig");
            patchNotes.Document.InvokeScript("enableStart");
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
                    patchNotes.Document.InvokeScript("completed");

                    updateProgress(1.0, "100, 206, 63");
                    break;
                default:
                    updateStatus("Unknown Error - Code: " + status, "203, 76, 0");
                    break;
            }
        }

        // Starts the game
        public void strtGameBtn_Click()
        {
            Process.Start(Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath", @"C:\Program Files (x86)\Steam") + "/steamapps/common/Arma 3" + "/arma3.exe", "-nosplash -skipIntro -connect=58.162.184.102 -password=diggers -port=2302 -mod=" + ScarletUtil.getModListForGameExec(ModsRoot, ModsDirName));
            this.Close();
        }
    }
}
