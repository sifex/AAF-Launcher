using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;

namespace AAF_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
