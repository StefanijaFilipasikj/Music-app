namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update5 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PlaylistSongs", newName: "SongPlaylists");
            DropForeignKey("dbo.Playlists", "ListenerId", "dbo.Listeners");
            DropPrimaryKey("dbo.SongPlaylists");
            AddColumn("dbo.Listeners", "Playlist_Id", c => c.Int());
            AddColumn("dbo.Playlists", "Listener_Id", c => c.Int());
            AddColumn("dbo.Playlists", "Listener_Id1", c => c.Int());
            AddPrimaryKey("dbo.SongPlaylists", new[] { "Song_Id", "Playlist_Id" });
            CreateIndex("dbo.Listeners", "Playlist_Id");
            CreateIndex("dbo.Playlists", "Listener_Id");
            CreateIndex("dbo.Playlists", "Listener_Id1");
            AddForeignKey("dbo.Listeners", "Playlist_Id", "dbo.Playlists", "Id");
            AddForeignKey("dbo.Playlists", "Listener_Id", "dbo.Listeners", "Id");
            AddForeignKey("dbo.Playlists", "Listener_Id1", "dbo.Listeners", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Playlists", "Listener_Id1", "dbo.Listeners");
            DropForeignKey("dbo.Playlists", "Listener_Id", "dbo.Listeners");
            DropForeignKey("dbo.Listeners", "Playlist_Id", "dbo.Playlists");
            DropIndex("dbo.Playlists", new[] { "Listener_Id1" });
            DropIndex("dbo.Playlists", new[] { "Listener_Id" });
            DropIndex("dbo.Listeners", new[] { "Playlist_Id" });
            DropPrimaryKey("dbo.SongPlaylists");
            DropColumn("dbo.Playlists", "Listener_Id1");
            DropColumn("dbo.Playlists", "Listener_Id");
            DropColumn("dbo.Listeners", "Playlist_Id");
            AddPrimaryKey("dbo.SongPlaylists", new[] { "Playlist_Id", "Song_Id" });
            AddForeignKey("dbo.Playlists", "ListenerId", "dbo.Listeners", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.SongPlaylists", newName: "PlaylistSongs");
        }
    }
}
