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

        public string NHLTeamCode { get; set; }
        public NHLTeam NHLTeam { get; set; }
        public string Team { get; set; }
        public int Number { get; set; }

        public string EligiblePositionString { get; set; }

        public bool Active { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string LexicalName
        {
            get { return LastName + ", " + FirstName + " " + NHLTeamCode; }
        }
        public virtual ICollection<RosterPlayer> RosterPlayers { get; set; }
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
            this.Property(t => t.Number).HasColumnName("PLAYER_NUMBER");
            this.Property(t => t.TSNName).HasColumnName("TSN_NAME");
            this.Property(t => t.EligiblePositionString).HasColumnName("ELIGIBLE_POSITION");
            this.Property(t => t.Active).HasColumnName("ACTIVE");
            this.Property(t => t.Team).HasColumnName("TEAM").HasMaxLength(50); ;
            this.HasOptional<NHLTeam>(t => t.NHLTeam)
                .WithMany(t => t.Players)
                .HasForeignKey(t => t.NHLTeamCode);


        }
    }

    [Table("dbo.NHLTeam")]
    public partial class NHLTeam
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<NHLPlayer> Players { get; set; }
    }

    public class NHLTeamMap : EntityTypeConfiguration<NHLTeam>
    {
        public NHLTeamMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).HasMaxLength(5).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.ToTable("dbo.NHLTeam");
            this.Property(t => t.Name).HasMaxLength(50);
            this.HasMany<NHLPlayer>(t => t.Players)
                .WithOptional(t => t.NHLTeam)
                .HasForeignKey(t => t.NHLTeamCode);
        }
    }
}