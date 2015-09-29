namespace HP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nlpool.Standing")]
    public partial class Standing
    {
        public int Id { get; set; }

        public int SeasonId { get; set; }

        public int PoolId { get; set; }
        [Display(Name ="Total")]
        public short PointTotal { get; set; }
        [Display(Name ="Rank")]
        public short Rank { get; set; }
        [Display(Name ="Gain")]
        public short Gain { get; set; }

        public virtual Pool Pool { get; set; }

        public virtual Team Team { get; set; }

        public virtual Season Season { get; set; }
    }
}
