using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlatform.Models
{
    public class BestOfViewModel
    {
        public Song Trending { get; set; }
        public List<Song> TopSongs { get; set; }
        public List<Artist> TopArtists { get; set; }
        public List<Album> TopAlbums { get; set; }
        public List<Song> PopSongs { get; set; }
        public List<Song> RapSongs { get; set; }
        public List<Song> RBSongs { get; set; }
        public List<Song> RockSongs { get; set; }
        public List<Song> KPopSongs { get; set; }
        public BestOfViewModel()
        {
            TopSongs = new List<Song>();
            TopArtists = new List<Artist>();
            TopAlbums = new List<Album>();
            PopSongs = new List<Song>();
            RapSongs = new List<Song>();
            RBSongs = new List<Song>();
            RockSongs = new List<Song>();
            KPopSongs = new List<Song>();
        }
    }
}