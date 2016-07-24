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
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Windows;
using System.Security.Permissions;

namespace Scarlet
{
    public static class Program
    {
        private static Mutex m_Mutex;
        public static bool restarting = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool createdNew;
            m_Mutex = new Mutex(true, "Scarlet", out createdNew);
            if (createdNew || restarting)
            {
                Application.Run(new Scarlet());
            }
            else
            {
                MessageBox.Show("The application is already running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
