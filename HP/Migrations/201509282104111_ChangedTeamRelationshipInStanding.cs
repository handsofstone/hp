namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTeamRelationshipInStanding : DbMigration
    {
        public override void Up()
        {
            AddColumn("nlpool.Standing", "Team_Id", c => c.Int());
            CreateIndex("nlpool.Standing", "Team_Id");
            AddForeignKey("nlpool.Standing", "Team_Id", "nlpool.Team", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("nlpool.Standing", "Team_Id", "nlpool.Team");
            DropIndex("nlpool.Standing", new[] { "Team_Id" });
            DropColumn("nlpool.Standing", "Team_Id");
        }
    }
}
