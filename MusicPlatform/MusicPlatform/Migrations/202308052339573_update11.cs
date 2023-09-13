namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "Lyrics", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "Lyrics");
        }
    }
}
