namespace HP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nlpool.Interval")]
    public partial class Interval
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Interval()
        {
        }

        public int Id { get; set; }

        [Required]
        [Column(TypeName= "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public int SeasonId { get; set; }

        public virtual Season Season { get; set; }

        public int PoolId { get; set; }
        public virtual Pool Pool { get; set; }

        public string Name { get { return StartDate.ToString("MMM d") + " - " + EndDate.ToString("MMM d"); } }

    }
}
