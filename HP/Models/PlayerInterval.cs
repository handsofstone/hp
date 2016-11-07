using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace HP.Models
{
    public class PlayerInterval
    {
        [Display(Name = "#")]
        public int Number { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Eligible Positions")]
        public string EligiblePositions { get; set; }
        [Display(Name = "Positon")]
        public string Position { get; set; }
        [Display(Name = "Team")]
        public string Team { get; set; }
        [Display(Name = "Points")]
        public int Points { get; set; }
        [Display(Name = "")]
        public bool Active { get; set; }
        [Display(Name = "Schedule")]
        public string Schedule { get; set; }
        public int PlayerId { get; set; }
        public int? LineupPlayerId { get; set; }

        public PlayerInterval() { }

        public PlayerInterval(NHLPlayer player, String position = null)
        {
            LineupPlayerId = null;
            PlayerId = player.Id;
            Number = player.Number;
            Name = player.FullName;
            Position = position ?? player.EligiblePositionString.First().ToString();
            EligiblePositions = player.EligiblePositionString;
            Team = player.NHLTeamCode;
            Points = 0;
            Active = false;
        }

        public PlayerInterval(RosterPlayer player, int? intervalId = null)
        {
            LineupPlayerId = null;
            PlayerId = player.PlayerId;
            Number = player.Player.Number;
            Name = player.Player.FullName;
            Position = player.Position ?? player.Player.EligiblePositionString.First().ToString();
            EligiblePositions = player.Player.EligiblePositionString;
            Team = player.Player.NHLTeamCode;
            Points = 0;
            Active = false;
            Schedule = intervalId.HasValue ? ScheduleString(Team, (int)intervalId) : null;
        }



        public PlayerInterval(LineupPlayer player, int? intervalId = null)
        {
            LineupPlayerId = intervalId.HasValue ? (int?) null : player.Id;
            PlayerId = player.PlayerId;
            Number = player.Player.Number;
            Name = player.Player.FullName;
            Position = player.Position;
            EligiblePositions = player.Player.EligiblePositionString;
            Team = player.Player.NHLTeamCode;
            Points = player.Total != null ? player.Total.Gain : 0;
            Active = player.Active;
            Schedule = ScheduleString(Team, (int) ( intervalId.HasValue ? intervalId : player.IntervalId ));
        }
        private string ScheduleString(string teamCode, int intervalId)
        {
            List<string> opponents = new List<string>();
            using (var context = new ApplicationDbContext())
            {
                foreach (GameInfo g in context.GamesByTeamInterval(teamCode, intervalId))
                {
                    var dow = g.StartTime.Value.DayOfWeek.ToString().Substring(0, 3).ToUpper();

                    if (g.HomeCode == teamCode)
                        opponents.Add(String.Format("{0}({1})", g.VisitorCode, dow));
                    else
                        opponents.Add(String.Format("@{0}({1})", g.HomeCode, dow));
                }
            }
            return String.Join(", ", opponents);
        }
    }
    public class PlayerIntervalComparer : IComparer<PlayerInterval>
    {
        public int Compare(PlayerInterval x, PlayerInterval y)
        {
            var result = y.Active.CompareTo(x.Active);
            if (result == 0)
                result = PositionCompare(x.Position, y.Position);
            if (result == 0)
                result = x.Points.CompareTo(y.Points);
            return result;
        }

        private int PositionCompare(string x, string y)
        {
            List<string> order = new List<string>() { "C", "R", "L", "D", "G" };
            return order.IndexOf(x).CompareTo(order.IndexOf(y));
        }
    }
}