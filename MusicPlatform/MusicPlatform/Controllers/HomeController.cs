using Microsoft.AspNet.Identity;
using MusicPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MusicPlatform.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var topArtists = db.Artists.OrderByDescending(artist => artist.Followers.Count()*0.5 + artist.Plays*0.5).Take(10).ToList();
            var topSongs = db.Songs.OrderByDescending(song => song.Likes*0.5 + song.Plays*0.5).Take(10).ToList();
            var topAlbums = db.Albums.OrderByDescending(album => album.Likes*0.5 + album.Plays*0.5).Take(10).ToList();
            var popSongs = db.Songs.Where(song => song.Genre == "Pop").OrderByDescending(song => song.Likes * 0.5 + song.Plays * 0.5).Take(10).ToList();
            var rapSongs = db.Songs.Where(song => song.Genre == "Rap").OrderByDescending(song => song.Likes * 0.5 + song.Plays * 0.5).Take(10).ToList();
            var rbSongs = db.Songs.Where(song => song.Genre == "R&B").OrderByDescending(song => song.Likes * 0.5 + song.Plays * 0.5).Take(10).ToList();
            var rockSongs = db.Songs.Where(song => song.Genre == "Rock").OrderByDescending(song => song.Likes * 0.5 + song.Plays * 0.5).Take(10).ToList();
            var kpopSongs = db.Songs.Where(song => song.Genre == "K-pop").OrderByDescending(song => song.Likes * 0.5 + song.Plays * 0.5).Take(10).ToList();


            var bestOf = new BestOfViewModel();
            bestOf.TopAlbums = topAlbums;
            bestOf.TopArtists = topArtists;
            bestOf.TopSongs = topSongs;
            bestOf.PopSongs = popSongs;
            bestOf.RapSongs = rapSongs;
            bestOf.RBSongs = rbSongs;
            bestOf.RockSongs = rockSongs;
            bestOf.KPopSongs = kpopSongs;
            bestOf.Trending = topSongs.ElementAt(0);

            //var listener = db.Listeners.FirstOrDefault(u => u.Email == User.Identity.GetUserName());

            //var discover = db.Songs.Where(song => !listener.LikedSongs.Contains(song))
            //    .Where(song => listener.Following.Contains(song.Artist));
            //!!!songs the listener hasn't listened to made by artists they love, or having the same genre...
            
            //var mostPlayed = songs the listener has played the most
            //var yourTopArtists = artists whose songs you've played the most
            
            return View(bestOf);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}