using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MusicPlatform.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Author")]
        public int ListenerId { get; set; }
        public virtual Listener Listener { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<Listener> LikedByListeners { get; set; }

        public int Likes { get; set; }

        public Playlist()
        {
            Songs = new List<Song>();
            LikedByListeners = new List<Listener>();
            Likes = 0;
        }
    }
}