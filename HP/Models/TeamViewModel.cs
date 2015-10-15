using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HP.Models
{
    public class TeamViewModel
    { 
        public int TeamId { get; set; }
        public IList<SelectListItem> AvailablePlayers { get; set; }
        public IList<SelectListItem> RosterPlayersToAdd { get; set; }
        public IList<PlayerInterval> RosterPlayers { get; set; }
        public IList<SelectListItem> Intervals { get; set; }
        [Display(Name="Interval")]
        public int SelectedIntervalId { get; set; }
        public IList<PlayerInterval> PlayerIntervals { get; set; }
    }
}