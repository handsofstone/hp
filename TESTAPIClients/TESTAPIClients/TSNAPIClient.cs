using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using TSNData;

namespace TestAPIClients
{
    class TSNAPIClient
    {
        public static PlayerHeader GetPlayerHeader(string SeoId)
        {
            var sb = new StringBuilder();
            sb.Append("http://stats.tsn.ca/HGET/urn:tsn:nhl:player:");
            sb.Append(SeoId);
            sb.Append("/header?type=json");
            var url = sb.ToString();

            var syncClient = new WebClient();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            //var content = syncClient.DownloadString(url);

            //webRequest.Headers.Add("Origin", "http://www.tsn.ca");
            //webRequest.Host = "stats.tsn.ca";
            //webRequest.KeepAlive = true;
            //webRequest.Connection = "keep-alive";
            //webRequest.Headers.Add("Pragma", "no-cache");
            //webRequest.Headers.Add("Cache-Control", "no-cache");
            //webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
            webRequest.Referer = "http://www.tsn.ca/nhl/player-bio";
            //webRequest.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            //webRequest.Headers.Add("Accept-Language", "en-US,en;q-0.8");
            webRequest.Method = "GET";
            WebProxy wp = new WebProxy("kdcbchwbs01.bchydro.adroot.bchydro.bc.ca", 8080);
            wp.UseDefaultCredentials = true;
            webRequest.Proxy = wp;
            var ph = new PlayerHeader();
            ph.PositionAcronym = "";
            try
            {
                WebResponse response = webRequest.GetResponse();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PlayerHeader));
                using (var rs = response.GetResponseStream())
                {
                    ph = (PlayerHeader)serializer.ReadObject(rs);
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return ph;
        }
        public static char[] GetPlayerPositions(string SeoId)
        {
            var ph = GetPlayerHeader(SeoId);
            var positions = new List<Char>();
            foreach (string pos in ph.PositionAcronym.Split('/'))
            {
                if (pos.Length > 0)
                    if (pos[0].Equals('W'))
                    {
                        positions.Add('R');
                        positions.Add('L');
                    }
                    else
                        positions.Add(pos[0]);
            }
            return positions.ToArray();
        }

    }
}
