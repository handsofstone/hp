﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Standing> Standings { get; set; }
        public virtual DbSet<Pool> Pools { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Team_Season_Player_Interval> Team_Season_Player_Interval { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

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

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Team_Season_Player_Interval)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);
        }

        public IList<Team> TeamsByPoolID(int PoolId)
        {
            var query = Teams.Where(t => t.Pool_Id == PoolId);
            return query.ToList<Team>();
        }
    }
}