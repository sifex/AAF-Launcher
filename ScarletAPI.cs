using System;
using System.Net;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Scarlet
{
    public static class ScarletAPI
    {
        public static string ScarletURL = "https://scarlet.australianarmedforces.org/";

        public static string APIURL(string type = "", string method = "", string identifyer = "", string returnValue = "")
        {
            var apiURL = (ScarletURL + @"api");
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
                throw new WebException("Unable to connect to Scarlet Servers. Please try again later.");
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
