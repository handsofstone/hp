namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "nlpool.Interval",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.String(nullable: false),
                        EndDate = c.String(nullable: false),
                        SeasonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("nlpool.Season", t => t.SeasonId)
                .Index(t => t.SeasonId);
            
            CreateTable(
                "nlpool.Season",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "nlpool.Standing",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        SeasonId = c.Int(nullable: false),
                        PoolId = c.Int(nullable: false),
                        PointTotal = c.Short(nullable: false),
                        Rank = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("nlpool.Pool", t => t.PoolId)
                .ForeignKey("nlpool.Team", t => t.TeamId)
                .ForeignKey("nlpool.Season", t => t.SeasonId)
                .Index(t => t.TeamId)
                .Index(t => t.SeasonId)
                .Index(t => t.PoolId);
            
            CreateTable(
                "nlpool.Pool",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "nlpool.Team",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Pool_Id = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("nlpool.User", t => t.User_Id)
                .ForeignKey("nlpool.Pool", t => t.Pool_Id)
                .Index(t => t.Pool_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "nlpool.Team_Season_Player_Interval",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        SeasonId = c.Int(nullable: false),
                        IntervalId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("nlpool.Player", t => t.PlayerId)
                .ForeignKey("nlpool.Team", t => t.TeamId)
                .ForeignKey("nlpool.Season", t => t.SeasonId)
                .ForeignKey("nlpool.Interval", t => t.IntervalId)
                .Index(t => t.TeamId)
                .Index(t => t.SeasonId)
                .Index(t => t.IntervalId)
                .Index(t => t.PlayerId);
            
            CreateTable(
                "nlpool.Player",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "nlpool.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "nlpool.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("nlpool.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "nlpool.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("nlpool.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "nlpool.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("nlpool.User", t => t.UserId)
                .ForeignKey("nlpool.Role", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "nlpool.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "nlpool.PoolSeason",
                c => new
                    {
                        Pools_Id = c.Int(nullable: false),
                        Seasons_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Pools_Id, t.Seasons_Id })
                .ForeignKey("nlpool.Pool", t => t.Pools_Id, cascadeDelete: true)
                .ForeignKey("nlpool.Season", t => t.Seasons_Id, cascadeDelete: true)
                .Index(t => t.Pools_Id)
                .Index(t => t.Seasons_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("nlpool.UserRole", "RoleId", "nlpool.Role");
            DropForeignKey("nlpool.Team_Season_Player_Interval", "IntervalId", "nlpool.Interval");
            DropForeignKey("nlpool.Team_Season_Player_Interval", "SeasonId", "nlpool.Season");
            DropForeignKey("nlpool.Standing", "SeasonId", "nlpool.Season");
            DropForeignKey("nlpool.Team", "Pool_Id", "nlpool.Pool");
            DropForeignKey("nlpool.Team", "User_Id", "nlpool.User");
            DropForeignKey("nlpool.UserRole", "UserId", "nlpool.User");
            DropForeignKey("nlpool.UserLogin", "UserId", "nlpool.User");
            DropForeignKey("nlpool.UserClaim", "UserId", "nlpool.User");
            DropForeignKey("nlpool.Team_Season_Player_Interval", "TeamId", "nlpool.Team");
            DropForeignKey("nlpool.Team_Season_Player_Interval", "PlayerId", "nlpool.Player");
            DropForeignKey("nlpool.Standing", "TeamId", "nlpool.Team");
            DropForeignKey("nlpool.PoolSeason", "Seasons_Id", "nlpool.Season");
            DropForeignKey("nlpool.PoolSeason", "Pools_Id", "nlpool.Pool");
            DropForeignKey("nlpool.Standing", "PoolId", "nlpool.Pool");
            DropForeignKey("nlpool.Interval", "SeasonId", "nlpool.Season");
            DropIndex("nlpool.PoolSeason", new[] { "Seasons_Id" });
            DropIndex("nlpool.PoolSeason", new[] { "Pools_Id" });
            DropIndex("nlpool.Role", "RoleNameIndex");
            DropIndex("nlpool.UserRole", new[] { "RoleId" });
            DropIndex("nlpool.UserRole", new[] { "UserId" });
            DropIndex("nlpool.UserLogin", new[] { "UserId" });
            DropIndex("nlpool.UserClaim", new[] { "UserId" });
            DropIndex("nlpool.User", "UserNameIndex");
            DropIndex("nlpool.Team_Season_Player_Interval", new[] { "PlayerId" });
            DropIndex("nlpool.Team_Season_Player_Interval", new[] { "IntervalId" });
            DropIndex("nlpool.Team_Season_Player_Interval", new[] { "SeasonId" });
            DropIndex("nlpool.Team_Season_Player_Interval", new[] { "TeamId" });
            DropIndex("nlpool.Team", new[] { "User_Id" });
            DropIndex("nlpool.Team", new[] { "Pool_Id" });
            DropIndex("nlpool.Standing", new[] { "PoolId" });
            DropIndex("nlpool.Standing", new[] { "SeasonId" });
            DropIndex("nlpool.Standing", new[] { "TeamId" });
            DropIndex("nlpool.Interval", new[] { "SeasonId" });
            DropTable("nlpool.PoolSeason");
            DropTable("nlpool.Role");
            DropTable("nlpool.UserRole");
            DropTable("nlpool.UserLogin");
            DropTable("nlpool.UserClaim");
            DropTable("nlpool.User");
            DropTable("nlpool.Player");
            DropTable("nlpool.Team_Season_Player_Interval");
            DropTable("nlpool.Team");
            DropTable("nlpool.Pool");
            DropTable("nlpool.Standing");
            DropTable("nlpool.Season");
            DropTable("nlpool.Interval");
        }
    }
}
