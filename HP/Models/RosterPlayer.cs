using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HP.Models
{
    [Table("nlpool.RosterPlayer")]
    public class RosterPlayer
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }

        public virtual Team Team { get;set; }
        public virtual NHLPlayer Player { get; set; }
        public string Position { get; set; }
    }
}