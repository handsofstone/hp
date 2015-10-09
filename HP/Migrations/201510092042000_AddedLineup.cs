namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLineup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "nlpool.Lineup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        IntervalId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Points = c.Int(nullable: false),
                        Position = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("nlpool.Interval", t => t.IntervalId)
                .ForeignKey("dbo.NHL_PLAYER", t => t.PlayerId)
                .ForeignKey("nlpool.Team", t => t.TeamId)
                .Index(t => t.PlayerId)
                .Index(t => t.IntervalId)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("nlpool.Lineup", "TeamId", "nlpool.Team");
            DropForeignKey("nlpool.Lineup", "PlayerId", "dbo.NHL_PLAYER");
            DropForeignKey("nlpool.Lineup", "IntervalId", "nlpool.Interval");
            DropIndex("nlpool.Lineup", new[] { "TeamId" });
            DropIndex("nlpool.Lineup", new[] { "IntervalId" });
            DropIndex("nlpool.Lineup", new[] { "PlayerId" });
            DropTable("nlpool.Lineup");
        }
    }
}
