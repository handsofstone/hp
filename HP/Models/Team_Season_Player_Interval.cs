namespace HP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nlpool.Team_Season_Player_Interval")]
    public partial class Team_Season_Player_Interval
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public int SeasonId { get; set; }

        public int IntervalId { get; set; }

        public int PlayerId { get; set; }

        public virtual Interval Interval { get; set; }

        public virtual Player Player { get; set; }

        public virtual Season Season { get; set; }

        public virtual Team Team { get; set; }
    }
}
