using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HP.Models
{

    public class StandingsViewModel
    {
        public int SelectedPoolID { get; set; }
        [Display(Name="Season")]
        public int SelectedSeasonID { get; set; }
        public IList<SelectListItem> Seasons { get; set; }
        public List<StandingRow> StandingRows { get; set; }
    }

    public class TeamSeasonStanding
    {
        public int Rank { get; set; }
        public int TeamId { get; set; }
        public int PoolId { get; set; }
        public int SeasonId { get; set; }
        public int Total { get; set; }
        public virtual Team Team { get; set; }
        public virtual Season Season { get; set; }
        public virtual Pool Pool { get; set; }
    }
    public class TeamSeasonStandingMap : EntityTypeConfiguration<TeamSeasonStanding>
    {
        public TeamSeasonStandingMap()
        {
            this.HasKey(t => new { t.TeamId, t.SeasonId });
            this.ToTable("nlpool.TeamSeasonStanding");
            this.HasRequired(t => t.Team)
                .WithMany()
                .HasForeignKey(t => t.TeamId);
            this.HasRequired(t => t.Season)
                .WithMany()
                .HasForeignKey(t => t.SeasonId);
            this.HasRequired(t => t.Pool)
                .WithMany()
                .HasForeignKey(t => t.PoolId);
        }
    }

    public class TeamIntervalActiveTotal
    {
        public int TeamId { get; set; }
        public int IntervalId { get; set; }
        public int SeasonId { get; set; }
        public int IntervalTotal { get; set; }
        public virtual Team Team { get; set; }
        public virtual Interval Interval { get; set; }
        public virtual Season Season { get; set; }
    }

    public class TeamIntervalActiveTotalMap : EntityTypeConfiguration<TeamIntervalActiveTotal>
    {
        public TeamIntervalActiveTotalMap()
        {
            this.HasKey(t => new { t.TeamId, t.IntervalId });
            this.ToTable("nlpool.TeamIntervalActiveTotal");
            this.HasRequired(t => t.Team)
                .WithMany()
                .HasForeignKey(t => t.TeamId);
            this.HasRequired(t => t.Interval)
                .WithMany()
                .HasForeignKey(t => t.IntervalId);            
        }
    }
    public class StandingRow
    {
        [Display(Name = "Rank")]
        public int Rank { get; set; }
        [Display(Name = "Team Name")]
        public string Name { get; set; }
        [Display(Name = "Gain")]
        public int Gain { get; set; }
        [Display(Name = "Total")]
        public int Total { get; set; }


    }
    
}