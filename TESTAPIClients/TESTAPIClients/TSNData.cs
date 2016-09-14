using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TSNData
{

    public class Statistic
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Value { get; set; }
    }

    public class TeamColours
    {
        public string DarkColour { get; set; }
        public string LightColour { get; set; }
    }

    [DataContract]
    public class PlayerHeader
    {
        [DataMember]
        public string Shoots { get; set; }
        [DataMember]
        public List<Statistic> Statistics { get; set; }
        [DataMember]
        public object PositionName { get; set; }
        [DataMember]
        public string JerseyNumber { get; set; }
        [DataMember]
        public string DateOfBirth { get; set; }
        [DataMember]
        public string PlaceOfBirth { get; set; }
        [DataMember]
        public string SmallFlagImage { get; set; }
        [DataMember]
        public string Height { get; set; }
        [DataMember]
        public string Weight { get; set; }
        [DataMember]
        public string Age { get; set; }
        [DataMember]
        public string RosterStatus { get; set; }
        [DataMember]
        public string RosterStatusDesc { get; set; }
        [DataMember]
        public string Seasons { get; set; }
        [DataMember]
        public string Drafted { get; set; }
        [DataMember]
        public string TeamImage { get; set; }
        [DataMember]
        public string TeamSeo { get; set; }
        [DataMember]
        public string TeamName { get; set; }
        [DataMember]
        public TeamColours TeamColours { get; set; }
        [DataMember]
        public string SeoId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PositionAcronym { get; set; }
        [DataMember]
        public string Image { get; set; }
    }

}
