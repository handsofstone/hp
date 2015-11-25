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
        public Team() { }

        public int Id { get; set; }

        [Required]
        [Display(Name="Team Name")]
        public string Name { get; set; }

        public int PoolId { get; set; }

        public virtual Pool Pool { get; set; }

        //public User User { get; set; }
        public virtual ICollection<UserTeam> Users { get; set; }

        //[StringLength(128)]
        //public string User_Id { get; set; }

        public virtual ICollection<RosterPlayer> RosterPlayers { get; set; }        

        public virtual ICollection<TeamSeasonStanding> Standings { get; set; }
             
    }
}
