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
        public IList<LineupRow> LineupRows { get; set; }
        public bool CanSubmit { get; set; }
        public bool CanSave { get; set; }
        public bool CanTrade { get; set; }
        public string SelectedStartTime { get; set; }
    }
    public class AddPlayersViewModel
    {
        public int TeamId { get; set; }
        public List<int> PlayerIds { get; set; }
    }

    public class RosterPlayerViewModel
    {
        public int PlayerId { get; set; }
        public string Position { get; set; }
    }
    
    public class LineupPlayerViewModel
    {
        public int? LineupPlayerId { get; set; }
        public int PlayerId { get; set; }
        public bool Active { get; set; }
        public string Position { get; set; }
        public LineupPlayerViewModel() { }
    }

    public class SaveRosterViewModel
    {
        public int TeamId { get; set; }
        public List<RosterPlayerViewModel> Players { get; set; }
        public SaveRosterViewModel() { }
    }

    public class SaveLineupViewModel
    {
        public int IntervalId { get; set; }
        public int TeamId { get; set; }
        public List<LineupPlayerViewModel> Players { get; set; }
        public SaveLineupViewModel() { }
    }

    public class LineupViewModel
    {
        public IList<PlayerInterval> PlayerIntervals { get; set; }
    }

    public class IntervalViewModel
    {
        public int SelectedIntervalId { get; set; }
        public IList<PlayerInterval> PlayerIntervals { get; set; }
        public bool CanSubmit { get; set; }
    }

    public class Offer
    {
        public int fromTeamId { get; set; }
        public int toTeamId { get; set; }
        public int[] Offering { get; set; }
        public int[] Requesting { get; set; }
    }
}