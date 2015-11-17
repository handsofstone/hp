namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserTeamTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("nlpool.Team", new[] { "User_Id" });
            CreateTable(
                "nlpool.UserTeam",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        TeamId = c.Int(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => new { t.UserId, t.TeamId })
                .ForeignKey("nlpool.Team", t => t.TeamId)
                .Index(t => t.UserId)
                .Index(t => t.TeamId);
            DropForeignKey("nlpool.Team", "User_Id", "nlpool.User");
            DropColumn("nlpool.Team", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("nlpool.Team", "User_Id", c => c.String(maxLength: 128));
            AddForeignKey("nlpool.Team", "User_Id", "nlpool.User");
            DropForeignKey("nlpool.UserTeam", "TeamId", "nlpool.Team");
            DropIndex("nlpool.UserTeam", new[] { "TeamId" });
            DropIndex("nlpool.UserTeam", new[] { "UserId" });
            DropTable("nlpool.UserTeam");
            CreateIndex("nlpool.Team", "User_Id");
        }
    }
}
