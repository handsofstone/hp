namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JoinGameAndTeams : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GAME_INFO", "HomeCode", c => c.String(maxLength: 5));
            AddColumn("dbo.GAME_INFO", "VisitorCode", c => c.String(maxLength: 5));
            CreateIndex("dbo.GAME_INFO", "HomeCode");
            CreateIndex("dbo.GAME_INFO", "VisitorCode");
            AddForeignKey("dbo.GAME_INFO", "HomeCode", "dbo.NHLTeam", "Code");
            AddForeignKey("dbo.GAME_INFO", "VisitorCode", "dbo.NHLTeam", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GAME_INFO", "VisitorCode", "dbo.NHLTeam");
            DropForeignKey("dbo.GAME_INFO", "HomeCode", "dbo.NHLTeam");
            DropIndex("dbo.GAME_INFO", new[] { "VisitorCode" });
            DropIndex("dbo.GAME_INFO", new[] { "HomeCode" });
            DropColumn("dbo.GAME_INFO", "VisitorCode");
            DropColumn("dbo.GAME_INFO", "HomeCode");
        }
    }
}
