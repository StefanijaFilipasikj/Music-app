using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MusicPlatform.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string AlbumArtURL { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual ICollection<Listener> LikesFromListeners { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
        public int Likes { get; set; }
        public int Plays { get; set; }

        public Album()
        {
            Songs = new List<Song>();
            LikesFromListeners = new List<Listener>();
            Likes = 0;
            Plays = 0;
        }
    }
}