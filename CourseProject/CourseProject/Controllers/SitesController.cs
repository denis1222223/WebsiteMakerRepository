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
using System.IO;

namespace CourseProject.Controllers
{
    public class SitesController : BaseController
    {
        Dictionary<string, bool> horizontalMenuCheck = new Dictionary<string, bool>()
        {
            { "Vertical", false},
            { "Horizontal", true},
            { "VerticalAndHorizontal", true},
            { "None", false}
        };

        Dictionary<string, bool> verticalMenuCheck = new Dictionary<string, bool>()
        {
            { "Vertical", true},
            { "Horizontal", false},
            { "VerticalAndHorizontal", true},
            { "None", false}
        };

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sites
        public ActionResult Index(string username)
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

        private void FillViewBag(int siteId)
        {
            ViewBag.Id = siteId;
            ViewBag.Theme = Request.Form["Theme"];
            ViewBag.horizontalMenu = horizontalMenuCheck[Request.Form["Menu"]];
            ViewBag.verticalMenu = verticalMenuCheck[Request.Form["Menu"]];
            ViewBag.Template = Request.Form["Template"];
        }

        private void FillSiteModel(Site site)
        {
            TagsParser parser = new TagsParser(db);
            site.Tags = parser.Parse(Request.Form["Tags"]);
            site.AuthorId = User.Identity.GetUserId();
            SitesRepository.Add(site, false);
        }

        // POST: Sites/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")]Site site)
        {
            if (ModelState.IsValid)
            {
                FillSiteModel(site);
                FillViewBag(site.Id);
                return View("GenerateHtml");
            }
            return View(site);
        }

        private string GetHtml()
        {
            string htmlCode;
            using (var reader = new StreamReader(Request.InputStream))
            {
                htmlCode = reader.ReadToEnd();
            }
            return htmlCode;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Save(int? id)
        {
            Site site = SitesRepository.GetSite(id);
            if (site == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            site.HtmlCode = GetHtml();
            if(SitesRepository.Exists((int)id))
            {
                UpdateSite(site);
            }
            else
            {
                CreateSite(site);
            }
            SitesRepository.Remove((int)id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void CreateSite(Site site)
        {
            db.Sites.Add(site);
            db.SaveChanges();
        }

        private void UpdateSite(Site site)
        {
            db.Entry(site).State = EntityState.Modified;
            db.SaveChanges();
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
