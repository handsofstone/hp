using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace HP.Models
{
    [Table("nlpool.RosterPlayer")]
    public class RosterPlayer
    {
        public int TeamId { get; set; }
        public int PlayerId { get; set; }    
        public virtual Team Team { get;  set; }
        public virtual NHLPlayer Player { get; set; }
        public string Position { get; set; }
    }

    public class RosterPlayerMap : EntityTypeConfiguration<RosterPlayer>
    {
        public RosterPlayerMap()
        {
            this.HasKey(e => new { e.TeamId, e.PlayerId });
            this.ToTable("nlpool.RosterPlayer");
            this.HasRequired<Team>(t => t.Team)
                .WithMany(t => t.RosterPlayers)
                .HasForeignKey<int>(t => t.TeamId);
            this.HasRequired<NHLPlayer>(t => t.Player)
                .WithMany(t => t.RosterPlayers)
                .HasForeignKey<int>(t => t.PlayerId);            

        }
    }
}