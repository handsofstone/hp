namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColumnPlayer_NumberInTableNHLPlayer : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.NHL_PLAYER", name: "Number", newName: "PLAYER_NUMBER");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.NHL_PLAYER", name: "PLAYER_NUMBER", newName: "Number");
        }
    }
}
