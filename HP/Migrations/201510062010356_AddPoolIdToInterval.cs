namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPoolIdToInterval : DbMigration
    {
        public override void Up()
        {
            AddColumn("nlpool.Interval", "PoolId", c => c.Int(nullable: false));
            CreateIndex("nlpool.Interval", "PoolId");
            AddForeignKey("nlpool.Interval", "PoolId", "nlpool.Pool", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("nlpool.Interval", "PoolId", "nlpool.Pool");
            DropIndex("nlpool.Interval", new[] { "PoolId" });
            DropColumn("nlpool.Interval", "PoolId");
        }
    }
}
