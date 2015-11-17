namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDeleteOnUserTeam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("nlpool.UserTeam", "TeamId", "nlpool.Team");
            DropForeignKey("nlpool.UserTeam", "UserId", "nlpool.User");
            AddForeignKey("nlpool.UserTeam", "TeamId", "nlpool.Team", "Id", cascadeDelete: true);
            AddForeignKey("nlpool.UserTeam", "UserId", "nlpool.User", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("nlpool.UserTeam", "UserId", "nlpool.User");
            DropForeignKey("nlpool.UserTeam", "TeamId", "nlpool.Team");
            AddForeignKey("nlpool.UserTeam", "UserId", "nlpool.User", "Id");
            AddForeignKey("nlpool.UserTeam", "TeamId", "nlpool.Team", "Id");
        }
    }
}
