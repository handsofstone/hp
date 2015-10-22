namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserTeamTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("nlpool.Team", "User_Id", "nlpool.User");
            DropForeignKey("nlpool.TeamSeasonStanding", "SeasonId", "nlpool.Season");
            DropForeignKey("nlpool.TeamSeasonStanding", "TeamId", "nlpool.Team");
            DropIndex("nlpool.Team", new[] { "User_Id" });
            DropIndex("nlpool.TeamSeasonStanding", new[] { "TeamId" });
            DropIndex("nlpool.TeamSeasonStanding", new[] { "SeasonId" });
            CreateTable(
                "nlpool.UserTeam",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.TeamId })
                .ForeignKey("nlpool.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("nlpool.Team", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TeamId);
            
            DropTable("nlpool.TeamSeasonStanding");
        }
        
        public override void Down()
        {
            CreateTable(
                "nlpool.TeamSeasonStanding",
                c => new
                    {
                        TeamId = c.Int(nullable: false),
                        SeasonId = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeamId, t.SeasonId });
            
            DropForeignKey("nlpool.UserTeam", "TeamId", "nlpool.Team");
            DropForeignKey("nlpool.UserTeam", "UserId", "nlpool.User");
            DropIndex("nlpool.UserTeam", new[] { "TeamId" });
            DropIndex("nlpool.UserTeam", new[] { "UserId" });
            DropTable("nlpool.UserTeam");
            CreateIndex("nlpool.TeamSeasonStanding", "SeasonId");
            CreateIndex("nlpool.TeamSeasonStanding", "TeamId");
            CreateIndex("nlpool.Team", "User_Id");
            AddForeignKey("nlpool.TeamSeasonStanding", "TeamId", "nlpool.Team", "Id");
            AddForeignKey("nlpool.TeamSeasonStanding", "SeasonId", "nlpool.Season", "Id");
            AddForeignKey("nlpool.Team", "User_Id", "nlpool.User", "Id");
        }
    }
}
