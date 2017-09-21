using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HP.Helpers
{
    static public class HTMLHelper
    {
        static public String Origin { get; set; }
        static public Stream GetResponseStream(string html)
        {
            string PROXY_SERVER = ConfigurationManager.AppSettings["PROXY_HOST"];            
            WebRequest wr = (WebRequest) WebRequest.Create(html);

            if (PROXY_SERVER != "")
            {
                int PROXY_PORT = Int32.Parse(ConfigurationManager.AppSettings["PROXY_PORT"]);
                WebProxy wp = new WebProxy(PROXY_SERVER, PROXY_PORT);
                wp.UseDefaultCredentials = true;
                wr.Proxy = wp;
            }
            wr.Headers.Add("Origin", Origin);           
            return wr.GetResponse().GetResponseStream();
        }

    }
}
