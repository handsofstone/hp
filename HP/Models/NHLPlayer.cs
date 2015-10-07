using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace HP.Models
{
    [Table("dbo.NHL_PLAYER")]
    public partial class NHLPlayer
    {
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string TSNName { get; set; }

        public string NHLTeam { get; set; }

        public int Number { get; set; }

        public string EligiblePositionString { get; set; }

        public bool Active { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName;  }
        }
        public string LexicalName
        {
            get { return LastName + ", " + FirstName; }
        }
    }

    public class PlayerMap : EntityTypeConfiguration<NHLPlayer>
    {
        public PlayerMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.ToTable("dbo.NHL_PLAYER");
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.LastName).HasColumnName("LAST_NAME").IsRequired().HasMaxLength(50);
            this.Property(t => t.FirstName).HasColumnName("FIRST_NAME").IsRequired().HasMaxLength(50);
            this.Property(t => t.NHLTeam).HasColumnName("TEAM").HasMaxLength(50);
            this.Property(t => t.Number).HasColumnName("PLAYER_NUMBER");
            this.Property(t => t.TSNName).HasColumnName("TSN_NAME");
            this.Property(t => t.EligiblePositionString).HasColumnName("ELIGIBLE_POSITION");
            this.Property(t => t.Active).HasColumnName("ACTIVE");


        }
    }
}