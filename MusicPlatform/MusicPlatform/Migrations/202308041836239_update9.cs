namespace MusicPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update9 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SongPlaylists", newName: "PlaylistSongs");
            DropPrimaryKey("dbo.PlaylistSongs");
            CreateTable(
                "dbo.CommentListeners",
                c => new
                    {
                        Comment_Id = c.Int(nullable: false),
                        Listener_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Comment_Id, t.Listener_Id })
                .ForeignKey("dbo.Comments", t => t.Comment_Id, cascadeDelete: true)
                .ForeignKey("dbo.Listeners", t => t.Listener_Id, cascadeDelete: true)
                .Index(t => t.Comment_Id)
                .Index(t => t.Listener_Id);
            
            AddPrimaryKey("dbo.PlaylistSongs", new[] { "Playlist_Id", "Song_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentListeners", "Listener_Id", "dbo.Listeners");
            DropForeignKey("dbo.CommentListeners", "Comment_Id", "dbo.Comments");
            DropIndex("dbo.CommentListeners", new[] { "Listener_Id" });
            DropIndex("dbo.CommentListeners", new[] { "Comment_Id" });
            DropPrimaryKey("dbo.PlaylistSongs");
            DropTable("dbo.CommentListeners");
            AddPrimaryKey("dbo.PlaylistSongs", new[] { "Song_Id", "Playlist_Id" });
            RenameTable(name: "dbo.PlaylistSongs", newName: "SongPlaylists");
        }
    }
}
