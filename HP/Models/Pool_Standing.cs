namespace HP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nlpool.Pool_Standing")]
    public partial class Pool_Standing
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public int SeasonId { get; set; }

        public int PoolId { get; set; }

        public short PointTotal { get; set; }

        public short Rank { get; set; }

        public virtual Pool Pool { get; set; }

        public virtual Team Team { get; set; }

        public virtual Season Season { get; set; }
    }
}
