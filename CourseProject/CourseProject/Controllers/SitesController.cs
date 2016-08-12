using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CourseProject.Models;
using CourseProject.Models.Entities;
using CourseProject.Environment;

namespace CourseProject.Controllers
{
    public class SitesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sites
        public ActionResult Index()
        {
            return View(db.Sites.ToList());
        }

        // GET: Sites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var site = db.Sites.Include(a => a.Author).FirstOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }
            ApplicationUser User = site.Author;
            return View(site);
        }

        // GET: Sites/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sites/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Rating")] Site site)
        {
            if (ModelState.IsValid)
            {
                site.Tags = TagsParser.Parse(db, Request.Form["Tags"]);
                site.AuthorId = User.Identity.GetUserId();
                db.Sites.Add(site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(site);
        }

        // GET: Sites/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // POST: Sites/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Rating")] Site site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }

        // GET: Sites/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // POST: Sites/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Site site = db.Sites.Find(id);
            db.Sites.Remove(site);
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
