using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NHLData
{
    class NHLData
    {
    }
    public class CurrentTeam
    {
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
    }

    public class PrimaryPosition
    {
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string abbreviation { get; set; }
    }

    public class Person
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public string link { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string primaryNumber { get; set; }
        public string birthDate { get; set; }
        public int currentAge { get; set; }
        public string birthCity { get; set; }
        public string birthCountry { get; set; }
        public string nationality { get; set; }
        public string height { get; set; }
        public int weight { get; set; }
        public bool active { get; set; }
        public bool alternateCaptain { get; set; }
        public bool captain { get; set; }
        public bool rookie { get; set; }
        public string shootsCatches { get; set; }
        public string rosterStatus { get; set; }
        public CurrentTeam currentTeam { get; set; }
        public PrimaryPosition primaryPosition { get; set; }
    }

    [DataContract]
    public class Player
    {
        [DataMember]
        public string copyright { get; set; }
        [DataMember]
        public List<Person> people { get; set; }
    }


    public class SearchResult
    {
        public List<string> suggestions { get; set; }
    }

}
