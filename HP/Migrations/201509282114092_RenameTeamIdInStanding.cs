namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTeamIdInStanding : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("nlpool.Standing", "Team_Id", "nlpool.Team");
            DropIndex("nlpool.Standing", new[] { "TeamId" });
            DropIndex("nlpool.Standing", new[] { "Team_Id" });
            DropColumn("nlpool.Standing", "Team_Id");
            RenameColumn(table: "nlpool.Standing", name: "TeamId", newName: "Team_Id");
            AlterColumn("nlpool.Standing", "Team_Id", c => c.Int(nullable: false));
            CreateIndex("nlpool.Standing", "Team_Id");
        }
        
        public override void Down()
        {
            DropIndex("nlpool.Standing", new[] { "Team_Id" });
            AlterColumn("nlpool.Standing", "Team_Id", c => c.Int());
            RenameColumn(table: "nlpool.Standing", name: "Team_Id", newName: "TeamId");
            AddColumn("nlpool.Standing", "Team_Id", c => c.Int());
            CreateIndex("nlpool.Standing", "Team_Id");
            CreateIndex("nlpool.Standing", "TeamId");
            AddForeignKey("nlpool.Standing", "Team_Id", "nlpool.Team", "Id");
        }
    }
}
