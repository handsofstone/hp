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
            Team_Season_Player_Interval = new HashSet<Team_Season_Player_Interval>();
        }

        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int SeasonId { get; set; }

        public virtual Season Season { get; set; }

        public int PoolId { get; set; }
        public virtual Pool Pool { get; set; }

        public string Name { get { return StartDate.ToString("MMM d") + " - " + EndDate.ToString("MMM d"); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Team_Season_Player_Interval> Team_Season_Player_Interval { get; set; }
    }
}
