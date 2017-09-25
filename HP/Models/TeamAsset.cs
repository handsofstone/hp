using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace HP.Models
{
    [Table("nlpool.TeamAsset")]
    public class TeamAsset
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int AssetId { get; set; }
        public string AssetType { get; set; }
        [NotMapped]
        public virtual RosterPlayer RosterPlayer
        {
            get
            {
                if (AssetType == "roster")
                {
                    using (var context = new ApplicationDbContext())
                        return (RosterPlayer)context.RosterPlayers.Where(p => p.Id == AssetId);
                }
                return null;
            }
        }
        public virtual Team Team { get; set; }
    }
    public class TeamAssetMap : EntityTypeConfiguration<TeamAsset>
    {
        public TeamAssetMap()
        {
            this.HasKey(t => t.Id);
            this.ToTable("nlpool.TeamAsset");
            this.HasRequired<Team>(t => t.Team)
                .WithMany(t => t.Assets)
                .HasForeignKey<int>(t => t.TeamId);
            //.WithOptionalDependent(t => t.Asset);
            //(t.AssetId && t.AssetType == 'roster'));
            //.WithOptionalPrincipal(t => (t.AssetId && t.AssetType =='roster'));

        }
    }
}