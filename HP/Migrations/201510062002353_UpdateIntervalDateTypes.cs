namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIntervalDateTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("nlpool.Interval", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("nlpool.Interval", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("nlpool.Interval", "EndDate", c => c.String(nullable: false));
            AlterColumn("nlpool.Interval", "StartDate", c => c.String(nullable: false));
        }
    }
}
