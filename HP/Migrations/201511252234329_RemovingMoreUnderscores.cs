namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingMoreUnderscores : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "nlpool.Team_Season_Player_Interval", newName: "TeamSeasonPlayerInterval");
            RenameColumn(table: "nlpool.PoolSeason", name: "Pools_Id", newName: "PoolsId");
            RenameColumn(table: "nlpool.PoolSeason", name: "Seasons_Id", newName: "SeasonsId");
            RenameIndex(table: "nlpool.PoolSeason", name: "IX_Pools_Id", newName: "IX_PoolsId");
            RenameIndex(table: "nlpool.PoolSeason", name: "IX_Seasons_Id", newName: "IX_SeasonsId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "nlpool.PoolSeason", name: "IX_SeasonsId", newName: "IX_Seasons_Id");
            RenameIndex(table: "nlpool.PoolSeason", name: "IX_PoolsId", newName: "IX_Pools_Id");
            RenameColumn(table: "nlpool.PoolSeason", name: "SeasonsId", newName: "Seasons_Id");
            RenameColumn(table: "nlpool.PoolSeason", name: "PoolsId", newName: "Pools_Id");
            RenameTable(name: "nlpool.TeamSeasonPlayerInterval", newName: "Team_Season_Player_Interval");
        }
    }
}
