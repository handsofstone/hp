using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HP.Models
{
    [Table("nlpool.PoolSeason")]
    public class PoolSeason
    {
        [Key, Column(Order = 0)]
        public int PoolId { get; set; }
        [Key, Column(Order = 1)]
        public int  SeasonId { get; set; }
        public virtual Pool Pool { get; set; }
        public virtual Season Season { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}