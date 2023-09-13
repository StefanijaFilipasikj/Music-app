using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPlatform.Models
{
    public class Listener
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string Email { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<Artist> Following { get; set; }
        public virtual ICollection<Song> LikedSongs { get; set; }
        public virtual ICollection<Album> LikedAlbums { get; set; }
        public virtual ICollection<Playlist> LikedPlaylists { get; set; }

        public Listener()
        {
            Playlists = new List<Playlist>();
            Following = new List<Artist>();
            LikedSongs = new List<Song>();
            LikedAlbums = new List<Album>();
            LikedPlaylists = new List<Playlist>();
        }
    }
}