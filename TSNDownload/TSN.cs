using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TSN
{
    static internal class HTMLHelper
    {
        static public String Origin { get; set; }
        static public String Referer { get; set; }
        static public Stream GetResponseStream(string html)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string PROXY_SERVER = ConfigurationManager.AppSettings["PROXY_HOST"];
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(html);

            if (PROXY_SERVER != "")
            {
                int PROXY_PORT = Int32.Parse(ConfigurationManager.AppSettings["PROXY_PORT"]);
                WebProxy wp = new WebProxy(PROXY_SERVER, PROXY_PORT);
                wp.UseDefaultCredentials = true;
                wr.Proxy = wp;
            }
            wr.Headers.Add("Origin", Origin);
            wr.Referer = Referer;
            return wr.GetResponse().GetResponseStream();
        }

    }

    public class TSN
    {
        static string HeaderUrl = "http://stats.tsn.ca/HGET/urn:tsn:nhl:player:{0}/header?type=json";
        static string ListingUrl = "http://stats.tsn.ca/GET/urn:tsn:nhl:players?type=json";
        WebProxy proxy;

        public string getEligiblePositions()
        {
            return null;
        }

        public static string getPlayerHeader(string seoId)
        {
            //HTMLHelper.Origin = "www.tsn.com";
            HTMLHelper.Referer = "http://www.tsn.ca/nhl/player-bio";
            string html = String.Format(HeaderUrl, seoId);
            StreamReader reader = new StreamReader(HTMLHelper.GetResponseStream(html));
            string searchResults = reader.ReadToEnd();
            return searchResults;

        }

        public static string getPlayerListing()
        {
            //string html = String.Format(suggestURL, searchString);
            //HTMLHelper.Origin = "www.tsn.com";
            HTMLHelper.Referer = "http://www.tsn.ca/nhl/players";
            StreamReader reader = new StreamReader(HTMLHelper.GetResponseStream(ListingUrl));
            //return HTMLHelper.GetResponseStream(ListingUrl);
            string searchResults = reader.ReadToEnd();
            return searchResults;
        }
    }
}
