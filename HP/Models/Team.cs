namespace HP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nlpool.Team")]
    public partial class Team
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Team()
        {
            Pool_Standing = new HashSet<Pool_Standing>();
            Team_Season_Player_Interval = new HashSet<Team_Season_Player_Interval>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Pool_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pool_Standing> Pool_Standing { get; set; }

        public virtual Pool Pool { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Team_Season_Player_Interval> Team_Season_Player_Interval { get; set; }
        public User User { get; set; }

        [StringLength(128)]
        public string User_Id { get; set; }

    }
}
