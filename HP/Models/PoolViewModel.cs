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
        public List<Standing> Standings { get; set; }
        public List<StandingRow> StandingRows { get; set; }
    }

    public class TeamSeasonStanding
    {
        //[ForeignKey("Team")]
        public int TeamId { get; set; }
        public int SeasonId { get; set; }
        public int Total { get; set; }
        public Team Team { get; set; }
        public Season Season { get; set; }
    }
    public class TeamSeasonStandingMap : EntityTypeConfiguration<TeamSeasonStanding>
    {
        public TeamSeasonStandingMap()
        {
            this.HasKey(t => new { t.TeamId, t.SeasonId });
            this.ToTable("nlpool.TeamSeasonStanding");
            this.HasRequired(t => t.Team)
                .WithMany(t => t.Standings)
                .HasForeignKey(t => t.TeamId);
            this.HasRequired(t => t.Season)
                .WithMany()
                .HasForeignKey(t => t.SeasonId);
        }
    }
    public class StandingRow
    {

    }
}