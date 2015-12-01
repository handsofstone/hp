namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataTypeToDateInInterval : DbMigration
    {
        public override void Up()
        {
            AlterColumn("nlpool.Interval", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("nlpool.Interval", "EndDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("nlpool.Interval", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("nlpool.Interval", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
