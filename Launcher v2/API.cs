using System;
using System.Net;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace AAF_Launcher
{
    public static class API
    {
        public static string ScarletAPI = "http://scarlet.australianarmedforces.org/";

        public static string APIURL(string type = "", string method = "", string identifyer = "", string returnValue = "")
        {
            var apiURL = (ScarletAPI + @"api");
            if (type != "")
            {
                apiURL += "/" + type;
            }
            if (method != "")
            {
                apiURL += "/" + method;
            }
            if (identifyer != "")
            {
                apiURL += "/" + identifyer;
            }
            if (returnValue != "")
            {
                apiURL += "/" + returnValue;
            }

            return (apiURL + "/");
        }

        public static string Request(string type = "", string method = "", string identifyer = "", string returnValue = "")
        {
            try {
                return (new WebClient()).DownloadString(APIURL(type, method, identifyer, returnValue)).Replace(System.Environment.NewLine, "");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Scarlet Servers down. Please try again later", "");
                Environment.Exit(1);
                return "";
            }
        }

        public static byte[] PostRequest(string type = "", string method = "", string identifyer = "", string returnValue = "", NameValueCollection pairs = null)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(APIURL(type, method, identifyer, returnValue), pairs);
            }
            return response;
        }
    }
}
