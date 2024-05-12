using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MusicPlatform.Models;

namespace MusicPlatform.Controllers
{
    public class ArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artists
        public ActionResult Index()
        {
            ViewBag.Email = User.Identity.GetUserName();
            return View(db.Artists.ToList());
        }

        // POST: Artists/Follow/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Follow(int id)
        {
            var artist = db.Artists.Find(id);
            if (artist != null)
            {
                var email = User.Identity.GetUserName();

                if (artist.Followers.FirstOrDefault(l => l.Email == email) == null)
                {
                    artist.Followers.Add(db.Listeners.FirstOrDefault(l => l.Email == email));
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Artists");
        }

        // POST: Artists/Unfollow/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unfollow(int id)
        {
            var artist = db.Artists.Find(id);
            if (artist != null)
            {
                var email = User.Identity.GetUserName();
                if (artist.Followers.FirstOrDefault(l => l.Email == email) != null)
                {
                    artist.Followers.Remove(db.Listeners.FirstOrDefault(l => l.Email == email));
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Artists");
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            Artist artist;
            if (User.IsInRole("Artist"))
            {
                var email = User.Identity.GetUserName();
                artist = db.Artists.FirstOrDefault(a => a.Email == email);
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                artist = db.Artists.Find(id);
            }

            if (artist == null)
            {
                return HttpNotFound("I wasn't found");
            }
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create(string email)
        {
            var artist = new Artist { Email = email };
            return View(artist);
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PhotoUrl,Email")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Details", "Artists", new { id = artist.Id });
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit()
        {
            var email = User.Identity.GetUserName();
            var artist = db.Artists.FirstOrDefault(a => a.Email == email);

            if (artist == null)
            {
                return HttpNotFound();
            }

            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PhotoUrl,Email")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = artist.Id });
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
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
