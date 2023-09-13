using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPlatform.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        public string PhotoUrl { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<Listener> Followers { get; set; }

        public int Plays { get; set; }

        public Artist()
        {
            Albums = new List<Album>();
            Songs = new List<Song>();
            Followers = new List<Listener>();
            Plays = 0;
        }
    }
}