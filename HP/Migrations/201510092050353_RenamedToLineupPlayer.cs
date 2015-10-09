namespace HP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedToLineupPlayer : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "nlpool.Lineup", newName: "LineupPlayer");
        }
        
        public override void Down()
        {
            RenameTable(name: "nlpool.LineupPlayer", newName: "Lineup");
        }
    }
}
