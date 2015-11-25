namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CleanUpTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("nlpool.Standing", "Team_Id", "nlpool.Team");
            DropForeignKey("nlpool.Standing", "SeasonId", "nlpool.Season");
            DropForeignKey("nlpool.TeamSeasonPlayerInterval", "PlayerId", "nlpool.Player");
            DropForeignKey("nlpool.TeamSeasonPlayerInterval", "TeamId", "nlpool.Team");
            DropForeignKey("nlpool.TeamSeasonPlayerInterval", "SeasonId", "nlpool.Season");
            DropForeignKey("nlpool.Standing", "PoolId", "nlpool.Pool");
            DropForeignKey("nlpool.TeamSeasonPlayerInterval", "IntervalId", "nlpool.Interval");
            DropIndex("nlpool.Standing", new[] { "SeasonId" });
            DropIndex("nlpool.Standing", new[] { "PoolId" });
            DropIndex("nlpool.Standing", new[] { "Team_Id" });
            DropIndex("nlpool.TeamSeasonPlayerInterval", new[] { "TeamId" });
            DropIndex("nlpool.TeamSeasonPlayerInterval", new[] { "SeasonId" });
            DropIndex("nlpool.TeamSeasonPlayerInterval", new[] { "IntervalId" });
            DropIndex("nlpool.TeamSeasonPlayerInterval", new[] { "PlayerId" });
            DropTable("nlpool.Standing");
            DropTable("nlpool.TeamSeasonPlayerInterval");
            DropTable("nlpool.Player");
        }
        
        public override void Down()
        {
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
                "nlpool.TeamSeasonPlayerInterval",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        SeasonId = c.Int(nullable: false),
                        IntervalId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "nlpool.Standing",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeasonId = c.Int(nullable: false),
                        PoolId = c.Int(nullable: false),
                        PointTotal = c.Short(nullable: false),
                        Rank = c.Short(nullable: false),
                        Gain = c.Short(nullable: false),
                        Team_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("nlpool.TeamSeasonPlayerInterval", "PlayerId");
            CreateIndex("nlpool.TeamSeasonPlayerInterval", "IntervalId");
            CreateIndex("nlpool.TeamSeasonPlayerInterval", "SeasonId");
            CreateIndex("nlpool.TeamSeasonPlayerInterval", "TeamId");
            CreateIndex("nlpool.Standing", "Team_Id");
            CreateIndex("nlpool.Standing", "PoolId");
            CreateIndex("nlpool.Standing", "SeasonId");
            AddForeignKey("nlpool.TeamSeasonPlayerInterval", "IntervalId", "nlpool.Interval", "Id");
            AddForeignKey("nlpool.Standing", "PoolId", "nlpool.Pool", "Id");
            AddForeignKey("nlpool.TeamSeasonPlayerInterval", "SeasonId", "nlpool.Season", "Id");
            AddForeignKey("nlpool.TeamSeasonPlayerInterval", "TeamId", "nlpool.Team", "Id");
            AddForeignKey("nlpool.TeamSeasonPlayerInterval", "PlayerId", "nlpool.Player", "Id");
            AddForeignKey("nlpool.Standing", "SeasonId", "nlpool.Season", "Id");
            AddForeignKey("nlpool.Standing", "Team_Id", "nlpool.Team", "Id");
        }
    }
}
