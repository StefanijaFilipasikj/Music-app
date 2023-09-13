namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SongPlaylists", newName: "PlaylistSongs");
            DropPrimaryKey("dbo.PlaylistSongs");
            CreateTable(
                "dbo.SongListeners",
                c => new
                    {
                        Song_Id = c.Int(nullable: false),
                        Listener_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_Id, t.Listener_Id })
                .ForeignKey("dbo.Songs", t => t.Song_Id, cascadeDelete: true)
                .ForeignKey("dbo.Listeners", t => t.Listener_Id, cascadeDelete: true)
                .Index(t => t.Song_Id)
                .Index(t => t.Listener_Id);
            
            AddPrimaryKey("dbo.PlaylistSongs", new[] { "Playlist_Id", "Song_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SongListeners", "Listener_Id", "dbo.Listeners");
            DropForeignKey("dbo.SongListeners", "Song_Id", "dbo.Songs");
            DropIndex("dbo.SongListeners", new[] { "Listener_Id" });
            DropIndex("dbo.SongListeners", new[] { "Song_Id" });
            DropPrimaryKey("dbo.PlaylistSongs");
            DropTable("dbo.SongListeners");
            AddPrimaryKey("dbo.PlaylistSongs", new[] { "Song_Id", "Playlist_Id" });
            RenameTable(name: "dbo.PlaylistSongs", newName: "SongPlaylists");
        }
    }
}
