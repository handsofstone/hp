namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNHLTeamTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NHLTeam",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 5),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.NHL_PLAYER", "NHLTeamCode", c => c.String(maxLength: 5));
            CreateIndex("dbo.NHL_PLAYER", "NHLTeamCode");
            AddForeignKey("dbo.NHL_PLAYER", "NHLTeamCode", "dbo.NHLTeam", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NHL_PLAYER", "NHLTeamCode", "dbo.NHLTeam");
            DropIndex("dbo.NHL_PLAYER", new[] { "NHLTeamCode" });
            DropColumn("dbo.NHL_PLAYER", "NHLTeamCode");
            DropTable("dbo.NHLTeam");
        }
    }
}
