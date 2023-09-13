using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlatform.Models
{
    public class PlaylistDetailsViewModel
    {
        public Playlist Playlist { get; set; }
        public List<Song> SongsOfPlaylist { get; set; }
    }
}