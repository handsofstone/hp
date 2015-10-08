using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HP.Models
{
    public class PlayerInterval
    {
        [Display(Name="#")]
        public int Number { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Eligible Positions")]
        public string EligiblePositions { get; set; }
        [Display(Name = "Positon")]
        public string Position { get; set; }
        [Display(Name = "Points")]
        public int Points { get; set; }
        [Display(Name="")]
        public bool Active { get; set; }

        public PlayerInterval (NHLPlayer player, String position = null)
        {
            Number = player.Number;
            Name = player.FullName;
            Position = position ?? player.EligiblePositionString.First().ToString();
            EligiblePositions = player.EligiblePositionString;
            Points = 0;
            Active = false;
        }
        public PlayerInterval(RosterPlayer player)
        {
            Number = player.Player.Number;
            Name = player.Player.FullName;
            Position = player.Position ?? player.Player.EligiblePositionString.First().ToString();
            EligiblePositions = player.Player.EligiblePositionString;
            Points = 0;
            Active = false;
        }
    }
}