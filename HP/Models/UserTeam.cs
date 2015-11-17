using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;

namespace HP.Models
{
    [Table("nlpool.UserTeam")]
    public class UserTeam
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int TeamId { get; set; }
        public virtual User User { get; set; }
        public virtual Team Team { get; set; }
        public string Role { get; set; }
    }
    public class UserTeamMap : EntityTypeConfiguration<UserTeam>
    {
        public UserTeamMap()
        {
            this.HasKey(e => new { e.UserId, e.TeamId });
            this.ToTable("nlpool.UserTeam");
            this.HasRequired<Team>(t => t.Team)
                .WithMany(t => t.Users)
                .HasForeignKey<int>(t => t.TeamId)
                .WillCascadeOnDelete(true);
            this.HasRequired<User>(t => t.User)
                .WithMany(t => t.Teams)
                .HasForeignKey<string>(t => t.UserId)
                .WillCascadeOnDelete(true);

        }
    }
}