namespace HP.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NLPool : DbContext
    {
        public NLPool()
            : base("name=NLPool")
        {
        }

        public virtual DbSet<Interval> Intervals { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Pool_Standing> Pool_Standing { get; set; }
        public virtual DbSet<Pool> Pools { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Team_Season_Player_Interval> Team_Season_Player_Interval { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Interval>()
                .HasMany(e => e.Team_Season_Player_Interval)
                .WithRequired(e => e.Interval)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.Team_Season_Player_Interval)
                .WithRequired(e => e.Player)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pool>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Pool>()
                .HasMany(e => e.Pool_Standing)
                .WithRequired(e => e.Pool)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pool>()
                .HasMany(e => e.Teams)
                .WithRequired(e => e.Pool)
                .HasForeignKey(e => e.Pool_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pool>()
                .HasMany(e => e.Seasons)
                .WithMany(e => e.Pools)
                .Map(m => m.ToTable("PoolSeason", "nlpool").MapLeftKey("Pools_Id").MapRightKey("Seasons_Id"));

            modelBuilder.Entity<Season>()
                .HasMany(e => e.Intervals)
                .WithRequired(e => e.Season)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Season>()
                .HasMany(e => e.Pool_Standing)
                .WithRequired(e => e.Season)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Season>()
                .HasMany(e => e.Team_Season_Player_Interval)
                .WithRequired(e => e.Season)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Pool_Standing)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Team_Season_Player_Interval)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);
        }
    }
}
