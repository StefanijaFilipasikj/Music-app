namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Song_Id", "dbo.Songs");
            DropIndex("dbo.Comments", new[] { "Song_Id" });
            RenameColumn(table: "dbo.Comments", name: "Song_Id", newName: "SongId");
            CreateTable(
                "dbo.ListenerAlbums",
                c => new
                    {
                        Listener_Id = c.Int(nullable: false),
                        Album_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Listener_Id, t.Album_Id })
                .ForeignKey("dbo.Listeners", t => t.Listener_Id, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_Id, cascadeDelete: true)
                .Index(t => t.Listener_Id)
                .Index(t => t.Album_Id);
            
            AlterColumn("dbo.Comments", "SongId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "SongId");
            AddForeignKey("dbo.Comments", "SongId", "dbo.Songs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "SongId", "dbo.Songs");
            DropForeignKey("dbo.ListenerAlbums", "Album_Id", "dbo.Albums");
            DropForeignKey("dbo.ListenerAlbums", "Listener_Id", "dbo.Listeners");
            DropIndex("dbo.ListenerAlbums", new[] { "Album_Id" });
            DropIndex("dbo.ListenerAlbums", new[] { "Listener_Id" });
            DropIndex("dbo.Comments", new[] { "SongId" });
            AlterColumn("dbo.Comments", "SongId", c => c.Int());
            DropTable("dbo.ListenerAlbums");
            RenameColumn(table: "dbo.Comments", name: "SongId", newName: "Song_Id");
            CreateIndex("dbo.Comments", "Song_Id");
            AddForeignKey("dbo.Comments", "Song_Id", "dbo.Songs", "Id");
        }
    }
}
