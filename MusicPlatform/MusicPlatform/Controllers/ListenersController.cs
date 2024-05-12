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
    public class ListenersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Listeners
        public ActionResult Index()
        {
            return View(db.Listeners.ToList());
        }

        // GET: Listeners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listener listener = db.Listeners.Find(id);
            if (listener == null)
            {
                return HttpNotFound();
            }
            return View(listener);
        }

        // GET: Listeners/Create
        public ActionResult Create(string email)
        {
            var listener = new Listener { Email = email };
            return View(listener);
        }

        // POST: Listeners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PhotoUrl,Email")] Listener listener)
        {
            if (ModelState.IsValid)
            {
                db.Listeners.Add(listener);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(listener);
        }

        // GET: Listeners/Edit/5
        public ActionResult Edit()
        {
            var email = User.Identity.GetUserName();
            var listener = db.Listeners.FirstOrDefault(l => l.Email == email);

            if (listener == null)
            {
                return HttpNotFound();
            }

            return View(listener);
        }

        // POST: Listeners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PhotoUrl,Email")] Listener listener)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listener).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(listener);
        }

        // GET: Listeners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listener listener = db.Listeners.Find(id);
            if (listener == null)
            {
                return HttpNotFound();
            }
            return View(listener);
        }

        // POST: Listeners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Listener listener = db.Listeners.Find(id);
            db.Listeners.Remove(listener);
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
