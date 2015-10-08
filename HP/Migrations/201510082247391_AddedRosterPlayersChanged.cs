namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRosterPlayersChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("nlpool.RosterPlayer", "NHLPlayer_Id", c => c.Int());
            CreateIndex("nlpool.RosterPlayer", "NHLPlayer_Id");
            AddForeignKey("nlpool.RosterPlayer", "NHLPlayer_Id", "dbo.NHL_PLAYER", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("nlpool.RosterPlayer", "NHLPlayer_Id", "dbo.NHL_PLAYER");
            DropIndex("nlpool.RosterPlayer", new[] { "NHLPlayer_Id" });
            DropColumn("nlpool.RosterPlayer", "NHLPlayer_Id");
        }
    }
}
