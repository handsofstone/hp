namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGainToStanding : DbMigration
    {
        public override void Up()
        {
            AddColumn("nlpool.Standing", "Gain", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("nlpool.Standing", "Gain");
        }
    }
}
