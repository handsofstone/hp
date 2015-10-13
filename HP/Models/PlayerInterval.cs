using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        [Display(Name = "Points")]
        public int Points { get; set; }
        [Display(Name = "")]
        public bool Active { get; set; }
        public int PlayerId { get; set; }
        public int? LineupPlayerId { get; set; }

        public PlayerInterval(NHLPlayer player, String position = null)
        {
            LineupPlayerId = null;
            PlayerId = player.Id;
            Number = player.Number;
            Name = player.FullName;
            Position = position ?? player.EligiblePositionString.First().ToString();
            EligiblePositions = player.EligiblePositionString;
            Points = 0;
            Active = false;
        }
        public PlayerInterval(RosterPlayer player)
        {
            LineupPlayerId = null;
            PlayerId = player.PlayerId;
            Number = player.Player.Number;
            Name = player.Player.FullName;
            Position = player.Position ?? player.Player.EligiblePositionString.First().ToString();
            EligiblePositions = player.Player.EligiblePositionString;
            Points = 0;
            Active = false;
        }
        public PlayerInterval(LineupPlayer player)
        {
            LineupPlayerId = player.Id;
            PlayerId = player.PlayerId;
            Number = player.Player.Number;
            Name = player.Player.FullName;
            Position = player.Position;
            EligiblePositions = player.Player.EligiblePositionString;
            Points = player.Points;
            Active = player.Active;
        }
    }
    public class PlayerIntervalComparer : IComparer<PlayerInterval>
    {
        public int Compare(PlayerInterval x, PlayerInterval y)
        {
            var result = y.Active.CompareTo(x.Active);
            if (result == 0)
                result = PositionCompare(x.Position,y.Position);
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