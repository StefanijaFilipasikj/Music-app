namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update17 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PlaylistSongs", newName: "SongPlaylists");
            DropForeignKey("dbo.CommentListeners", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.CommentListeners", "Listener_Id", "dbo.Listeners");
            DropIndex("dbo.CommentListeners", new[] { "Comment_Id" });
            DropIndex("dbo.CommentListeners", new[] { "Listener_Id" });
            DropPrimaryKey("dbo.SongPlaylists");
            AddPrimaryKey("dbo.SongPlaylists", new[] { "Song_Id", "Playlist_Id" });
            DropColumn("dbo.Comments", "Likes");
            DropTable("dbo.CommentListeners");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CommentListeners",
                c => new
                    {
                        Comment_Id = c.Int(nullable: false),
                        Listener_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Comment_Id, t.Listener_Id });
            
            AddColumn("dbo.Comments", "Likes", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.SongPlaylists");
            AddPrimaryKey("dbo.SongPlaylists", new[] { "Playlist_Id", "Song_Id" });
            CreateIndex("dbo.CommentListeners", "Listener_Id");
            CreateIndex("dbo.CommentListeners", "Comment_Id");
            AddForeignKey("dbo.CommentListeners", "Listener_Id", "dbo.Listeners", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CommentListeners", "Comment_Id", "dbo.Comments", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.SongPlaylists", newName: "PlaylistSongs");
        }
    }
}
