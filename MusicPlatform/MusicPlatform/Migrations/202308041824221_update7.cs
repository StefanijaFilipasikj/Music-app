namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "SongId", "dbo.Songs");
            DropIndex("dbo.Comments", new[] { "SongId" });
            RenameColumn(table: "dbo.Comments", name: "SongId", newName: "Song_Id1");
            AddColumn("dbo.Comments", "Song_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "Song_Id1", c => c.Int());
            CreateIndex("dbo.Comments", "Song_Id1");
            AddForeignKey("dbo.Comments", "Song_Id1", "dbo.Songs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Song_Id1", "dbo.Songs");
            DropIndex("dbo.Comments", new[] { "Song_Id1" });
            AlterColumn("dbo.Comments", "Song_Id1", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "Song_Id");
            RenameColumn(table: "dbo.Comments", name: "Song_Id1", newName: "SongId");
            CreateIndex("dbo.Comments", "SongId");
            AddForeignKey("dbo.Comments", "SongId", "dbo.Songs", "Id", cascadeDelete: true);
        }
    }
}
