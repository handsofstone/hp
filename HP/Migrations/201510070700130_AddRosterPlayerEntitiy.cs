namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRosterPlayerEntitiy : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("nlpool.RosterPlayer", "Team_Id", "nlpool.Team");
            DropForeignKey("nlpool.RosterPlayer", "Player_Id", "dbo.NHL_PLAYER");
            DropIndex("nlpool.RosterPlayer", new[] { "Team_Id" });
            DropIndex("nlpool.RosterPlayer", new[] { "Player_Id" });
            CreateTable(
                "nlpool.RosterPlayer",
                c => new
                    {
                        PlayerId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                        Position = c.String(),
                    })
                .PrimaryKey(t => new { t.PlayerId, t.TeamId })
                .ForeignKey("dbo.NHL_PLAYER", t => t.PlayerId)
                .ForeignKey("nlpool.Team", t => t.TeamId)
                .Index(t => t.PlayerId)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            CreateTable(
                "nlpool.RosterPlayer",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.Player_Id });
            
            DropForeignKey("nlpool.RosterPlayer", "TeamId", "nlpool.Team");
            DropForeignKey("nlpool.RosterPlayer", "PlayerId", "dbo.NHL_PLAYER");
            DropIndex("nlpool.RosterPlayer", new[] { "TeamId" });
            DropIndex("nlpool.RosterPlayer", new[] { "PlayerId" });
            DropTable("nlpool.RosterPlayer");
            CreateIndex("nlpool.RosterPlayer", "Player_Id");
            CreateIndex("nlpool.RosterPlayer", "Team_Id");
            AddForeignKey("nlpool.RosterPlayer", "Player_Id", "dbo.NHL_PLAYER", "ID", cascadeDelete: true);
            AddForeignKey("nlpool.RosterPlayer", "Team_Id", "nlpool.Team", "Id", cascadeDelete: true);
        }
    }
}
