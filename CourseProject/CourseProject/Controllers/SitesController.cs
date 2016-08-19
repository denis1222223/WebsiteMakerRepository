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
using Resources;
using Newtonsoft.Json.Linq;
using CourseProject.Models.JsonModels;
using Newtonsoft.Json;

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
        private const string mainPageUrl = "main";

        [Route("rating")]
        public ActionResult Rating()
        {
            return View(db.Sites.ToList());
        }

        private bool CheckCurrentUser(string userName)
        {
            if (userName == User.Identity.GetUserName() || User.IsInRole("Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private UserSitesViewModel CreateSitesListViewModel(ApplicationUser user)
        {
            UserSitesViewModel userSites = new UserSitesViewModel();
            if (CheckCurrentUser(user.UserName))
            {
                userSites.IsAuthor = true;
            }
            else
            {
                userSites.IsAuthor = false;
            }
            userSites.Sites = user.Sites.ToList();
            userSites.UserName = user.UserName;
            return userSites;
        }

        private ApplicationUser FindUserInDb(string userName)
        {
            return db.Users.Where(user => user.UserName == userName)
                    .Include(user => user.Sites)
                    .AsEnumerable()
                    .FirstOrDefault(user => user.UserName == userName);
        }

        public ActionResult Index(string userName)
        {
            ApplicationUser requiredUser = FindUserInDb(userName);
            if (requiredUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var userSites = CreateSitesListViewModel(requiredUser);
                return View(userSites);
            }
        }

        public ActionResult Details(string userName, string siteUrl, string pageUrl)
        {
            if (String.IsNullOrEmpty(siteUrl))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var id = 0;
            var requiredSite = db.Sites.Include(a => a.Author).FirstOrDefault(s => s.Id == id);
            if (requiredSite == null)
            {
                return HttpNotFound();
            }
            ApplicationUser User = requiredSite.Author;
            return View(siteUrl);
        }

        // GET: create
        [Authorize]
        [Route("create")]
        public ActionResult CreateSite()
        {
            return View();
        }

        private void FillSiteModel(Site site)
        {
            TagsParser parser = new TagsParser(db);
            site.Tags = parser.Parse(Request.Form["Tags"]);
            site.AuthorId = User.Identity.GetUserId();
            SitesRepository.Add(site, false, User.Identity.Name);
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSite([Bind(Include = "Name,Url,Theme")]Site site)
        {
            if (ModelState.IsValid)
            {
                GenerateJson(site);
                FillSiteModel(site);
                return new RedirectResult(User.Identity.Name +
                    '/' + site.Url + '/' + mainPageUrl + '/' + "edit");
            }
            return View(site);
        }

        private void GenerateJson(Site site)
        {
            GenerateMenuJson(site);
            CreateMainPage(site);
        }

        private void CreateMainPage(Site site)
        {
            Page mainPage = new Page { Name = Resource.Home, Url = mainPageUrl };
            GenerateContentJson(mainPage);
            site.Pages.Add(mainPage);
        }

        private void GenerateContentJson(Page page)
        {
            ContentJsonModel content = new ContentJsonModel();
            content.ContentTemplate = Request.Form["Template"];
            page.ContentJson = JsonConvert.SerializeObject(content);
        }

        private void GenerateMenuJson(Site site)
        {
            MenuJsonModel menu = new MenuJsonModel();
            menu.HorizontalMenuExist = horizontalMenuCheck[Request.Form["Menu"]];
            menu.VerticalMenuExist = verticalMenuCheck[Request.Form["Menu"]];
            AddHomeToMenu(menu, site.Url);
            site.MenuJson = JsonConvert.SerializeObject(menu);
        }

        private void AddHomeToMenu(MenuJsonModel menu, string siteUrl)
        {
            if (menu.HorizontalMenuExist)
            {
                menu.HorizontalMenu.Add(new MenuItemJsonModel
                {
                    Title = Resource.Home,
                    Link = User.Identity.GetUserName() + '/' + siteUrl + '/' + mainPageUrl
                });
            }
            else
                if (menu.VerticalMenuExist)
            {
                menu.VerticalMenu.Add(new MenuItemJsonModel
                {
                    Title = Resource.Home,
                    Link = User.Identity.GetUserName() + '/' + siteUrl + '/' + mainPageUrl
                });
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string userName, string siteUrl, string pageUrl, [Bind(Include = "Name,Url")]Page page)
        {
            Site site = SitesRepository.GetSite(userName + siteUrl);
            if (site == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenerateContentJson(page);
            CheckPageUrl(site, page);
            site.Pages.Add(page);
            return new RedirectResult('/' + userName + '/' +
                siteUrl + '/' + page.Url + "/edit");
        }

        private void CheckPageUrl(Site site, Page newPage)
        {
            if (site.Pages.FirstOrDefault(page => page.Url == newPage.Url) != null)
            {
                newPage.Url += "1";
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditSite(string userName, string siteUrl, string contentJson, string menuJson)
        {
            if (CheckCurrentUser(userName))
            {
                string siteId = userName + siteUrl;
                Site site = SitesRepository.GetSite(siteId);
                if (site == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SaveJson(site, mainPageUrl, contentJson, menuJson);
                PutSiteToDb(site, siteId);
                SitesRepository.Remove(siteId);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        private void PutSiteToDb(Site site, string siteId)
        {
            if (SitesRepository.Exists(siteId))
            {
                UpdateSite(site);
            }
            else
            {
                CreateSiteInDb(site);
            }
        }

        private void SaveJson(Site site, string pageUrl, string contentJson, string menuJson)
        {
            if (!String.IsNullOrEmpty(menuJson))
            {
                site.MenuJson = menuJson;
            }
            if (!String.IsNullOrEmpty(contentJson))
            {
                site.Pages.FirstOrDefault(page => page.Url == pageUrl).ContentJson = contentJson;
            }
        }

        private void CreateSiteInDb(Site site)
        {
            db.Sites.Add(site);
            db.SaveChanges();
        }

        private void UpdateSite(Site site)
        {
            db.Entry(site).State = EntityState.Modified;
            db.SaveChanges();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(string userName, string siteUrl, string pageUrl, string contentJson, string menuJson)
        {
            Site site = SitesRepository.GetSite(userName + siteUrl);
            if (site == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaveJson(site, pageUrl, contentJson, menuJson);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void FillEditViewModel(EditViewModel editModel, Site site, Page page)
        {
            editModel.MenuJson = site.MenuJson;
            editModel.Theme = site.Theme;
            editModel.ContentJson = page.ContentJson;
            editModel.SiteUrl = site.Url;
        }

        private EditViewModel CreateEditViewModel(Site site, string pageUrl)
        {
            Page page = site.Pages.FirstOrDefault(requiredPage => requiredPage.Url == pageUrl);
            if (page == null)
            {
                return null;
            }
            EditViewModel editModel = new EditViewModel();
            FillEditViewModel(editModel, site, page);
            return editModel;
        }

        [Authorize]
        public ActionResult Edit(string userName, string siteUrl, string pageUrl)
        {
            if (CheckCurrentUser(userName))
            {
                Site site = SitesRepository.GetSite(userName + siteUrl);
                if (site == null)
                {
                    return new RedirectResult('/' + userName + '/' + siteUrl + "/edit");
                }
                EditViewModel editModel = CreateEditViewModel(site, pageUrl);
                if (editModel == null)
                {
                    return HttpNotFound();
                }
                FillEditViewBag(site, editModel);
                return View(editModel);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        private void FillEditViewBag(Site site, EditViewModel editModel)
        {
            JToken token = JObject.Parse(site.MenuJson);
            ViewBag.horizontalMenu = token.SelectToken("horizontal_menu_exist");
            ViewBag.verticalMenu = token.SelectToken("vertical_menu_exist");
            ViewBag.Template = JObject.Parse(editModel.ContentJson).SelectToken("content_template");
        }

        [Authorize]
        public ActionResult EditSite(string userName, string siteUrl)
        {
            if (CheckCurrentUser(userName))
            {
                ApplicationUser user = FindUserInDb(userName);
                Site site = user.Sites.FirstOrDefault(requiredSite => requiredSite.Url == siteUrl);
                if (site == null)
                {
                    return HttpNotFound();
                }
                SitesRepository.Add(site, true, userName);
                return View(site);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
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

        [Route("uploadPicture")]
        public string UploadPicture(string pictureDataUrl)
        {
            var binData = new Base64Decoder().Decode(pictureDataUrl);
            using (var stream = new MemoryStream(binData))
            {
                var result = CloudinaryInitializer.Cloudinary.Upload(new CloudinaryDotNet.Actions.ImageUploadParams()
                {
                    File = new CloudinaryDotNet.Actions.FileDescription("pic", stream),
                    Folder = "content",
                });
                return result.SecureUri.ToString();
            }
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
