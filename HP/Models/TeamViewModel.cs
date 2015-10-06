using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HP.Models
{
    public class TeamViewModel
    {
    }
    public class RosterViewModel
    {
        public IList<SelectListItem> AvailablePlayers { get; set; }
        public IList<SelectListItem> RosterPlayers { get; set; }
        public IList<SelectListItem> Intervals { get; set; }
        public IList<Team_Season_Player_Interval> PlayerIntervals { get; set; }
    }
}