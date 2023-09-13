using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MusicPlatform.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]

        public string VideoURL { get; set; }
        [Required]

        public string AudioURL { get; set; }
        [Required]
        public string Lyrics { get; set; }

        [Required]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        [Required]
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }
    
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Listener> LikesFromListeners { get; set; }

        public int Likes { get; set; }
        public int Plays { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }

        public Song()
        {
            Playlists = new List<Playlist>();
            Comments = new List<Comment>();
            LikesFromListeners = new List<Listener>();
            Likes = 0;
            Plays = 0;
        }
    }
}