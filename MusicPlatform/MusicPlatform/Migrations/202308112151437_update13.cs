namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "Plays", c => c.Int(nullable: false));
            AddColumn("dbo.Artists", "Plays", c => c.Int(nullable: false));
            AddColumn("dbo.Songs", "Plays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "Plays");
            DropColumn("dbo.Artists", "Plays");
            DropColumn("dbo.Albums", "Plays");
        }
    }
}
