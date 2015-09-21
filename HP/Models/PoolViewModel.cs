using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HP.Models
{
    public class Standing
    {
        public String Name { get; set; }
        public String Gain { get; set; }
        public String Total { get; set; }
    }

    public class StandingsViewModel
    {
        public User User { get; set; }
        public ICollection<Team> Teams { get; set; }
        public List<Season> Seasons { get; set; }
        public List<Standing> Standings { get; set; }
    }
}