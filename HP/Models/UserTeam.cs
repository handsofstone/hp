using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HP.Models
{
    [Table("nlpool.UserTeam")]
    public class UserTeam
    {
        public string UserId { get; set; }
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
                .HasForeignKey<int>(t => t.TeamId);
            this.HasRequired<User>(t => t.User)
                .WithMany(t => t.Teams)
                .HasForeignKey<string>(t => t.UserId);

        }
    }
}