using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace HP.Models
{
    public class LineupModel
    {
        public int IntervalId { get; set; }
        public int TeamId { get; set; }
        public List<LineupPlayerInput> Players { get; set; }
        public LineupModel() { }
    }

    public class LineupPlayerInput
    {
        public int? LineupPlayerId { get; set; }
        public int PlayerId { get; set; }
        public bool Active { get; set; }
        public string Position { get;set; }
        public LineupPlayerInput() { }
    }
    public class LineupPlayer
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int IntervalId { get; set; }
        public int TeamId { get; set; }
        public bool Active { get; set; }
        public int Points { get; set; }
        public string Position { get; set; }
        public virtual NHLPlayer Player { get; set; }
        public virtual Team Team { get; set; }
        public virtual Interval Interval { get; set; }
        public virtual LineupPlayerTotal Total { get; set; }
        public virtual Season Season
        {
            get
            {
                return Interval.Season;
            }
        }
    }

    public class LineupPlayerMap : EntityTypeConfiguration<LineupPlayer>
    {
        public LineupPlayerMap()
        {
            this.HasKey(t => t.Id);
            this.ToTable("nlpool.LineupPlayer");
            this.HasRequired<Team>(t => t.Team)
                .WithMany()
                .HasForeignKey<int>(t => t.TeamId);
            this.HasRequired<NHLPlayer>(t => t.Player)
                .WithMany()
                .HasForeignKey<int>(t => t.PlayerId);
            this.HasRequired<Interval>(t => t.Interval)
                .WithMany()
                .HasForeignKey<int>(t => t.IntervalId);
            this.HasOptional<LineupPlayerTotal>(t => t.Total)
                .WithRequired(t => t.LineupPlayer);
        }
    }
    public class LineupPlayerTotal
    {
        [Key, ForeignKey("LineupPlayer")]
        public int Id { get; set; }
        public int? W { get; set; }
        public int? L { get; set; }
        public int? OT { get; set; }
        public int? P { get; set; }
        public int Gain { get; set; }
        public LineupPlayer LineupPlayer { get; set; }        
    }
    public class LineupPlayerTotalMap : EntityTypeConfiguration<LineupPlayerTotal>
    {
        public LineupPlayerTotalMap()
        {
            this.HasKey(t => t.Id);
            this.ToTable("nlpool.LineupPlayerTotal");
        }
    }
}