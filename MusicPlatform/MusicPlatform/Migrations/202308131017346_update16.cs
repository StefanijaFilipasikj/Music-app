namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "Genre", c => c.String());
            AddColumn("dbo.Songs", "Language", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "Language");
            DropColumn("dbo.Songs", "Genre");
        }
    }
}
