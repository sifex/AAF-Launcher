using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Hosting.Self;

namespace Scarlet
{
    public class ScarletWeb : Nancy.NancyModule
    {
        public ScarletWeb()
        {
            Get["/"] = parameters =>
            {
                return Negotiate.WithView("download").WithHeader("Access-Control-Allow-Origin", "*");
            };
        }
    }
}
