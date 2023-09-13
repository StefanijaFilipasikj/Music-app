using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MusicPlatform.Models;

namespace MusicPlatform.Controllers
{
    public class AlbumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Albums
        public ActionResult Index()
        {
            var email = User.Identity.GetUserName();
            ViewBag.Email = email;

            if (User.IsInRole("Artist"))
            {
                var albums = db.Albums.Where(album => album.Artist.Email == email).ToList();
                ViewBag.ArtistId = db.Artists.FirstOrDefault(artist => artist.Email == email).Id; 
                return View(albums);
            }

            // For Listeners, show all albums
            var allAlbums = db.Albums.ToList();
            return View(allAlbums);
        }


        // POST: Albums/Like/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Like(int id)
        {
            var album = db.Albums.Find(id);
            if (album != null)
            {
                var email = User.Identity.GetUserName();

                if (album.LikesFromListeners.FirstOrDefault(l => l.Email == email) == null)
                {
                    album.LikesFromListeners.Add(db.Listeners.FirstOrDefault(l => l.Email == email));
                    album.Likes++;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Albums");
        }

        // POST: Albums/Unlike/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unlike(int id)
        {
            var album = db.Albums.Find(id);
            if (album != null)
            {
                var email = User.Identity.GetUserName();
                if (album.LikesFromListeners.FirstOrDefault(l => l.Email == email) != null)
                {
                    album.LikesFromListeners.Remove(db.Listeners.FirstOrDefault(l => l.Email == email));
                    album.Likes--;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Albums");
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create(int id)
        {
            Album album = new Album();
            album.ArtistId = id;
            return View(album);
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,AlbumArtURL,ArtistId")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = album.Id });
            }
            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", album.ArtistId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,AlbumArtURL,ArtistId,Likes")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(album);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int id)
        {
            Album album = db.Albums.Find(id);

            db.Albums.Remove(album);
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
