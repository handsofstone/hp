namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColumnsInPoolSeason : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "nlpool.PoolSeason", name: "PoolsId", newName: "PoolId");
            RenameColumn(table: "nlpool.PoolSeason", name: "SeasonsId", newName: "SeasonId");
            RenameIndex(table: "nlpool.PoolSeason", name: "IX_PoolsId", newName: "IX_PoolId");
            RenameIndex(table: "nlpool.PoolSeason", name: "IX_SeasonsId", newName: "IX_SeasonId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "nlpool.PoolSeason", name: "IX_SeasonId", newName: "IX_SeasonsId");
            RenameIndex(table: "nlpool.PoolSeason", name: "IX_PoolId", newName: "IX_PoolsId");
            RenameColumn(table: "nlpool.PoolSeason", name: "SeasonId", newName: "SeasonsId");
            RenameColumn(table: "nlpool.PoolSeason", name: "PoolId", newName: "PoolsId");
        }
    }
}
