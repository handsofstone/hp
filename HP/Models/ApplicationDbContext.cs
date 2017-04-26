using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        public virtual DbSet<Pool> Pools { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<RosterPlayer> RosterPlayers { get; set; }
        public virtual DbSet<LineupPlayer> LineupPlayers { get; set; }
        public virtual DbSet<LineupPlayerTotal> LineupPlayerTotals { get; set; }
        public virtual DbSet<TeamSeasonStanding> TeamSeasonStanding { get; set; }
        public virtual DbSet<TeamIntervalActiveTotal> TeamIntervalActiveTotal { get; set; }
        public virtual DbSet<UserTeam> UserTeams { get; set; }
        public virtual DbSet<GameInfo> Games { get; set; }
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

            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole", "nlpool");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin", "nlpool");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim", "nlpool");
            modelBuilder.Entity<IdentityRole>().ToTable("Role", "nlpool");

            modelBuilder.Configurations.Add(new PlayerMap());

            modelBuilder.Entity<Pool>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Pool>()
                .HasMany(e => e.Teams)
                .WithRequired(e => e.Pool)
                .HasForeignKey(e => e.PoolId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pool>()
                .HasMany(e => e.Seasons)
                .WithMany(e => e.Pools)
                .Map(m => m.ToTable("PoolSeason", "nlpool").MapLeftKey("PoolId").MapRightKey("SeasonId"));

            modelBuilder.Entity<Season>()
                .HasMany(e => e.Intervals)
                .WithRequired(e => e.Season)
                .WillCascadeOnDelete(false);

            modelBuilder.Configurations.Add(new RosterPlayerMap());
            modelBuilder.Configurations.Add(new UserTeamMap());
            modelBuilder.Configurations.Add(new LineupPlayerMap());
            modelBuilder.Configurations.Add(new NHLTeamMap());
            modelBuilder.Configurations.Add(new LineupPlayerTotalMap());
            modelBuilder.Configurations.Add(new TeamSeasonStandingMap());
            modelBuilder.Configurations.Add(new TeamIntervalActiveTotalMap());
            modelBuilder.Configurations.Add(new GameInfoMap());

        }

        public IList<Team> TeamsByPoolID(int PoolId)
        {
            return Teams.Where(t => t.PoolId == PoolId).ToList<Team>();
        }

        public IList<NHLPlayer> PlayersByPoolID(int PoolId)
        {
            return TeamsByPoolID(PoolId).SelectMany(t => t.RosterPlayers).Select(p => p.Player).ToList<NHLPlayer>();
        }
        public IList<NHLPlayer> AvailablePlayers(int PoolId)
        {
            return Players.Where(p => p.Active).ToList().Except(PlayersByPoolID(PoolId)).ToList();
        }

        public IList<Interval> IntervalsByPoolSeason(int poolId, int seasonId)
        {
            return Intervals.Where(i => i.PoolId == poolId && i.SeasonId == seasonId).ToList();
        }

        public IEnumerable<GameInfo> GamesByTeamInterval(string teamCode, int intervalId)
        {
            Interval interval = Intervals.Find(intervalId);
            var intervalEnd = interval.EndDate.AddDays(1);
            return from g in Games
                   where g.StartTime >= interval.StartDate && g.StartTime <= intervalEnd &&
                   (g.HomeCode == teamCode || g.VisitorCode == teamCode)
                   select g;

        }
        public IQueryable<int> GainForTeamInterval(int intervalId, int teamId)
        {
            return from ti in TeamIntervalActiveTotal
                   where ti.IntervalId == intervalId && ti.TeamId == teamId
                   select ti.IntervalTotal;
        }

        public string LineupView(int teamId, int intervalId)
        {
            return Database.SqlQuery<String>("select nlpool.LineupView(@teamId, @intervalId)",
                new SqlParameter("teamId", teamId), new SqlParameter("intervalId", intervalId)).FirstOrDefault();
        }
        public string LineupDashboard(string userId, int teamId, int intervalId)
        {
            return Database.SqlQuery<String>(
                "select nlpool.LineupDashboard(@userId, @teamId, @intervalId)",
                new SqlParameter("userId", userId ?? (object)DBNull.Value),
                new SqlParameter("teamId", teamId),
                new SqlParameter("intervalId", intervalId))
                .FirstOrDefault();
        }
        public string RosterDashboard(/*string userId,*/ int teamId)
        {
            return Database.SqlQuery<String>(
                "select nlpool.RosterDashboard(@teamId)",
                new SqlParameter("teamId", teamId))
                .FirstOrDefault();
        }

        public string TradeDashboard(/*string userId,*/ int teamId)
        {
            return Database.SqlQuery<String>(
                "select nlpool.TradeDashboard(@teamId)",
                new SqlParameter("teamId", teamId))
                .FirstOrDefault();
        }

        public string Assets(/*string userId,*/ int teamId)
        {
            return Database.SqlQuery<String>(
                "select nlpool.TeamAssets(@teamId)",
                new SqlParameter("teamId", teamId))
                .FirstOrDefault();
        }
    }
}
