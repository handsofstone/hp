namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTSNNameToNHLPlayer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NHL_PLAYER", "TSN_NAME", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NHL_PLAYER", "TSN_NAME");
        }
    }
}
