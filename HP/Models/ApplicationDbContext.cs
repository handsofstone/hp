using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HP.Models
{

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("name=NLPool")
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Identity and Authorisation
        //public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<IdentityUserClaim> UserClaims { get; set; }
        public virtual DbSet<IdentityUserLogin> UserLogins { get; set; }
        public virtual DbSet<IdentityUserRole> UserRoles { get; set; }

        // Custom DbSets
        public virtual DbSet<Interval> Intervals { get; set; }
        public virtual DbSet<NHLPlayer> Players { get; set; }
        public virtual DbSet<Standing> Standings { get; set; }
        public virtual DbSet<Pool> Pools { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Team_Season_Player_Interval> Team_Season_Player_Interval { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<RosterPlayer> RosterPlayers { get; set; }
        public virtual DbSet<LineupPlayer> LineupPlayers { get; set; }
        public virtual DbSet<LineupPlayerTotal> LineupPlayerTotals { get; set; }
        public virtual DbSet<TeamSeasonStanding> TeamSeasonStanding { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>().ToTable("User", "nlpool");
            modelBuilder.Entity<User>()
                .HasMany(e => e.Claims);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Logins);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.Teams)
            //    .WithMany(e => e.Users)
            //    .Map(m => m.ToTable("UserTeam", "nlpool").MapLeftKey("UserId").MapRightKey("TeamId"));
            modelBuilder.Entity<User>()
            .HasMany(e => e.Teams)
            .WithOptional(e => e.User)
            .HasForeignKey(e => e.User_Id);
            
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole", "nlpool");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin", "nlpool");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim", "nlpool");
            modelBuilder.Entity<IdentityRole>().ToTable("Role", "nlpool");

            modelBuilder.Entity<Interval>()
                 .HasMany(e => e.Team_Season_Player_Interval)
                 .WithRequired(e => e.Interval)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.Team_Season_Player_Interval)
                .WithRequired(e => e.Player)
                .WillCascadeOnDelete(false);

            modelBuilder.Configurations.Add(new PlayerMap());

            modelBuilder.Entity<Pool>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Pool>()
                .HasMany(e => e.Standings)
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
                .HasMany(e => e.Standings)
                .WithRequired(e => e.Season)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Season>()
                .HasMany(e => e.Team_Season_Player_Interval)
                .WithRequired(e => e.Season)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Standing>()
                .HasRequired(e => e.Team)
                .WithMany()
                .WillCascadeOnDelete(false);
            //modelBuilder.Entity<Team>()
            //    .HasMany(e => e.Standings)
            //    .WithRequired(e => e.Team)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Team>()
            //    .HasMany(e => e.Team_Season_Player_Interval)
            //    .WithRequired(e => e.Team)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Team>()
            //    .HasMany(e => e.RosterPlayers)
            //    .WithMany()
            //    .Map(m => m.ToTable("RosterPlayer", "nlpool").MapLeftKey("Team_Id").MapRightKey("Player_Id"));
            //modelBuilder.Entity<Team>()
            //    .HasMany<TeamSeasonStanding>(e => e.Standings)
            //    .WithRequired(e => e.Team)
            //    .Map(m => m.ToTable("TeamSeasonStanding", "nlpool").MapKey("TeamId"));

            modelBuilder.Configurations.Add(new RosterPlayerMap());
            modelBuilder.Configurations.Add(new LineupPlayerMap());
            modelBuilder.Configurations.Add(new NHLTeamMap());
            modelBuilder.Configurations.Add(new LineupPlayerTotalMap());
            modelBuilder.Configurations.Add(new TeamSeasonStandingMap());

        }

        public IList<Team> TeamsByPoolID(int PoolId)
        {
            return Teams.Where(t => t.Pool_Id == PoolId).ToList<Team>();
        }

        public IList<NHLPlayer> PlayersByPoolID(int PoolId)
        {
            return TeamsByPoolID(PoolId).SelectMany(t => t.RosterPlayers).Select(p=>p.Player).ToList<NHLPlayer>();            
        }
        public IList<NHLPlayer> AvailablePlayers(int PoolId)
        {
            return Players.Where(p => p.Active).ToList().Except(PlayersByPoolID(PoolId)).ToList();
        }

        public IList<Interval> IntervalsByPoolSeason(int poolId, int seasonId)
        {
            return Intervals.Where(i => i.PoolId == poolId && i.SeasonId == seasonId).ToList();
        }

    }
}