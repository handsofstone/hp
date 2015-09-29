namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedStandingModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "nlpool.Pool_Standing", newName: "Standing");
        }
        
        public override void Down()
        {
            RenameTable(name: "nlpool.Standing", newName: "Pool_Standing");
        }
    }
}
