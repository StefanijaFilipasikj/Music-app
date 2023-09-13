using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MusicPlatform.Models;

namespace MusicPlatform.Controllers
{
    public class PlaylistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Playlists
        public ActionResult Index()
        {
            var email = User.Identity.GetUserName();
            ViewBag.Email = email;

            // Retrieve the current user's "Liked Songs" playlist (if it exists)
            var currentUser = db.Listeners.FirstOrDefault(l => l.Email == email);
            
            var playlists = db.Playlists.Where(p => p.Title != "Liked Songs" || p.ListenerId == currentUser.Id);
            
            return View(playlists.ToList());
        }



        public ActionResult MyPlaylists()
        {
            var email = User.Identity.GetUserName();
            var playlists = db.Playlists.Include(p => p.Listener).Where(p => p.Listener.Email == email).ToList();
            ViewBag.User = db.Listeners.FirstOrDefault(u => u.Email == email).Name;

            return View(playlists.ToList());
        }

        // POST: Playlists/Like/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Like(int id)
        {
            var playlist = db.Playlists.Find(id);
            if (playlist != null)
            {
                var email = User.Identity.GetUserName();

                if (playlist.LikedByListeners.FirstOrDefault(l => l.Email == email) == null)
                {
                    playlist.LikedByListeners.Add(db.Listeners.FirstOrDefault(l => l.Email == email));
                    playlist.Likes++;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Playlists");
        }

        // POST: Playlists/Unlike/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unlike(int id)
        {
            var playlist = db.Playlists.Find(id);
            if (playlist != null)
            {
                var email = User.Identity.GetUserName();
                if (playlist.LikedByListeners.FirstOrDefault(l => l.Email == email) != null)
                {
                    playlist.LikedByListeners.Remove(db.Listeners.FirstOrDefault(l => l.Email == email));
                    playlist.Likes--;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Playlists");
        }

        // GET: Playlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Playlist playlist = db.Playlists.Find(id);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            var allSongs = db.Songs.ToList();
            var songsInPlaylist = playlist.Songs.Select(s => s.Id).ToList();
            var songsNotInPlaylist = allSongs.Where(s => !songsInPlaylist.Contains(s.Id)).ToList();

            ViewBag.Songs = new SelectList(songsNotInPlaylist, "Id", "Title");

            var email = User.Identity.GetUserName();
            ViewBag.Email = email;

            return View(playlist);
        }


        // POST: Playlists/AddSongToPlaylist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSongToPlaylist(int playlistId, int selectedSongId)
        {
            var playlist = db.Playlists.Find(playlistId);
            var songToAdd = db.Songs.Find(selectedSongId);

            if (playlist == null || songToAdd == null)
            {
                return HttpNotFound();
            }

            playlist.Songs.Add(songToAdd);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = playlistId });
        }

        // POST: Playlists/RemoveSongFromPlaylist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSongFromPlaylist(int playlistId, int selectedSongId)
        {
            var playlist = db.Playlists.Find(playlistId);
            var songToRemove = db.Songs.Find(selectedSongId);

            if (playlist == null || songToRemove == null)
            {
                return HttpNotFound();
            }

            playlist.Songs.Remove(songToRemove);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = playlistId });
        }

        // GET: Playlists/Create
        public ActionResult Create()
        {
            var email = User.Identity.GetUserName();
            var listenerId = db.Listeners.FirstOrDefault(l => l.Email == email).Id;

            Playlist playlist = new Playlist();
            playlist.ListenerId = listenerId;

            return View(playlist);
        }

        // POST: Playlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ListenerId")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Playlists.Add(playlist);
                db.SaveChanges();
                return RedirectToAction("Details", "Playlists", new { id = playlist.Id });
            }
            return View(playlist);
        }

        // GET: Playlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playlist playlist = db.Playlists.Find(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListenerId = new SelectList(db.Listeners, "Id", "Name", playlist.ListenerId);
            return View(playlist);
        }

        // POST: Playlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,ListenerId,Likes")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Playlists", new { id = playlist.Id });
            }
            ViewBag.ListenerId = new SelectList(db.Listeners, "Id", "Name", playlist.ListenerId);
            return View(playlist);
        }


        // GET: Playlists/Delete/5
        public ActionResult Delete(int id)
        {
            Playlist playlist = db.Playlists.Find(id);

            db.Playlists.Remove(playlist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
