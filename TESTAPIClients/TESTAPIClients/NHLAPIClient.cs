using NHLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace TestAPIClients
{
    class NHLAPIClient
    {
        public static Player GetPlayer(int playerId)
        {
            var sb = new StringBuilder();
            sb.Append("https://statsapi.web.nhl.com/api/v1/people/");
            sb.Append(playerId);            
            var url = sb.ToString();

            //var syncClient = new WebClient();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            //WebProxy wp = new WebProxy("kdcbchwbs01.bchydro.adroot.bchydro.bc.ca", 8080);
            //wp.UseDefaultCredentials = true;
            //webRequest.Proxy = wp;

            var p = new Player();
            try
            {
                WebResponse response = webRequest.GetResponse();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Player));
                using (var rs = response.GetResponseStream())
                {
                    p = (Player)serializer.ReadObject(rs);
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return p;
        }

        public static string getPlayerPosition(int playerId)
        {

            var p = GetPlayer(playerId).people.First<Person>();
            return p.primaryPosition.code;
        }

        public static SearchResult SearchPlayers(string searchString)
        {
            var sb = new StringBuilder();
            sb.Append("https://suggest.svc.nhl.com/svc/suggest/v1/minactiveplayers/");
            sb.Append(searchString);
            sb.Append("/99999");
            var url = sb.ToString();

            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            //WebProxy wp = new WebProxy("kdcbchwbs01.bchydro.adroot.bchydro.bc.ca", 8080);
            //wp.UseDefaultCredentials = true;
            //webRequest.Proxy = wp;

            var r = new SearchResult();
            try
            {
                WebResponse response = webRequest.GetResponse();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SearchResult));
                using (var rs = response.GetResponseStream())
                {
                    r = (SearchResult)serializer.ReadObject(rs);
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return r;
        }
    }
}
