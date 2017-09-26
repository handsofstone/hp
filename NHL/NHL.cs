using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NHL
{
    public class NHL
    {
        static string PlayerUrl = "https://statsapi.web.nhl.com/api/v1/people/{0}?expand=person.currentTeam";

        public string getEligiblePositions()
        {
            return null;
        }

        public static string getPlayerProfile(int playerId)
        {
            //HTMLHelper.Origin = "www.tsn.com";
            //HTMLHelper.Referer = "http://www.tsn.ca/nhl/player-bio";
            string html = String.Format(PlayerUrl, playerId);
            StreamReader reader = new StreamReader(HTMLHelper.GetResponseStream(html));
            string searchResults = reader.ReadToEnd();
            return searchResults;

        }
    }
    static internal class HTMLHelper
    {
        static public String Origin { get; set; }
        static public String Referer { get; set; }
        static public Stream GetResponseStream(string html)
        {
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

}
