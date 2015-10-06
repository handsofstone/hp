namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNHLPlayerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NHL_PLAYER",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        LAST_NAME = c.String(nullable: false, maxLength: 50),
                        FIRST_NAME = c.String(nullable: false, maxLength: 50),
                        TEAM = c.String(maxLength: 50),
                        Number = c.Int(nullable: false),
                        ELIGIBLE_POSITION = c.String(),
                        ACTIVE = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "nlpool.RosterPlayer",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.Player_Id })
                .ForeignKey("nlpool.Team", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.NHL_PLAYER", t => t.Player_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.Player_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("nlpool.RosterPlayer", "Player_Id", "dbo.NHL_PLAYER");
            DropForeignKey("nlpool.RosterPlayer", "Team_Id", "nlpool.Team");
            DropIndex("nlpool.RosterPlayer", new[] { "Player_Id" });
            DropIndex("nlpool.RosterPlayer", new[] { "Team_Id" });
            DropTable("nlpool.RosterPlayer");
            DropTable("dbo.NHL_PLAYER");
        }
    }
}
