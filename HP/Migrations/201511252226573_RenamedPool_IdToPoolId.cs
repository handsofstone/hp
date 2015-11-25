namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedPool_IdToPoolId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "nlpool.Team", name: "Pool_Id", newName: "PoolId");
            RenameIndex(table: "nlpool.Team", name: "IX_Pool_Id", newName: "IX_PoolId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "nlpool.Team", name: "IX_PoolId", newName: "IX_Pool_Id");
            RenameColumn(table: "nlpool.Team", name: "PoolId", newName: "Pool_Id");
        }
    }
}
