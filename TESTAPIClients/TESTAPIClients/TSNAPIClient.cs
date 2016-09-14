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
        public static void GetPlayerHeader(string SeoId)
        {
            var url = "http://stats.tsn.ca/HGET/urn:tsn:nhl:player:justin-abdelkader/header?type=json";

            var syncClient = new WebClient();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            //var content = syncClient.DownloadString(url);

            webRequest.Headers.Add("Origin", "http://www.tsn.ca");
            webRequest.Host = "stats.tsn.ca";
            //webRequest.KeepAlive = true;
            //webRequest.Connection = "keep-alive";
            //webRequest.Headers.Add("Host", );
            //webRequest.Headers.Add("Connection", "keep-alive");
            //webRequest.Headers.Add("Pragma", "no-cache");
            //webRequest.Headers.Add("Cache-Control", "no-cache");
            //webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
            //webRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36");
            webRequest.Referer = "http://www.tsn.ca/nhl/player-bio/justin-abdelkader";
            //webRequest.Headers.Add("Referer", "http://www.tsn.ca/nhl/player-bio/justin-abdelkader");
            //webRequest.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            //webRequest.Headers.Add("Accept-Language", "en-US,en;q-0.8");
            webRequest.Method = "GET";
            WebResponse response = webRequest.GetResponse();

            //StreamReader sr = new StreamReader();

           
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PlayerHeader));
            using (var rs = response.GetResponseStream())
            {
                var headerData = (PlayerHeader)serializer.ReadObject(rs);
            }
        }
    }
}
