using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HP.Models
{
    public class LineupModel
    {
        public int IntervalId { get; set; }
        public int TeamId { get; set; }
        public List<LineupPlayerInput> Players { get; set; }
        public LineupModel() { }
    }

    public class LineupPlayerInput
    {
        public int? LineupPlayerId { get; set; }
        public int PlayerId { get; set; }
        public bool Active { get; set; }
        public string Position { get;set; }
        public LineupPlayerInput() { }
    }
}