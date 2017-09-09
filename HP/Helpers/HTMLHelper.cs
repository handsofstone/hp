using System;
using System.Collections.Generic;
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
            WebRequest wr = (WebRequest) WebRequest.Create(html);
            wr.Headers.Add("Origin", Origin);
            return wr.GetResponse().GetResponseStream();
        }

    }
}
