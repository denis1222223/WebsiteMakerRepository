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
using System.Data.Entity.Validation;
using System.Diagnostics;

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
            if (!String.IsNullOrEmpty(userName))
            {
                return db.Users.Where(user => user.UserName == userName)
                    .Include(user => user.Sites)
                    .AsEnumerable()
                    .FirstOrDefault(user => user.UserName == userName);               
            }
            return null;
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
            Site requestedSite = SitesRepository.GetSite(userName + siteUrl);
            if (requestedSite == null)
            {
                return new RedirectResult('/' + userName + '/' + siteUrl + "/details");
            }
            DetailsViewModel detailsModel = CreateDetailsViewModel(requestedSite, pageUrl, false);
            if (detailsModel == null)
            {
                return HttpNotFound();
            }
            FillDetailsFromJson(requestedSite, detailsModel);
            ViewBag.IsEdit = false;
            return View(detailsModel);
        }

        public ActionResult SiteDetails(string userName, string siteUrl)
        {
            Site requestedSite = SitesRepository.GetSite(userName + siteUrl);
            if (requestedSite == null)
            {
                if (!AddSiteToRepository(userName, siteUrl, true))
                    return HttpNotFound();
            }
            return new RedirectResult('/' + userName + '/' +siteUrl + '/' + mainPageUrl);
        }

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
                CheckSiteUrl(site);
                FillSiteModel(site);

                return new RedirectResult(User.Identity.Name +
                    '/' + site.Url + '/' + mainPageUrl + '/' + "edit");
            }
            return View(new CreateSiteViewModel());
        }

        private void CheckSiteUrl(Site newSite)
        {
            if (db.Sites.Include(site => site.Author)
                    .Where(site => site.Url == newSite.Url && site.Author.UserName == User.Identity.Name)
                    .FirstOrDefault() != null)
            { 
                newSite.Url += "1";
            }
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
                    Link = '/' + User.Identity.GetUserName() + '/' + siteUrl + '/' + mainPageUrl
                });
            }
            else
                if (menu.VerticalMenuExist)
            {
                menu.VerticalMenu.Add(new MenuItemJsonModel
                {
                    Title = Resource.Home,
                    Link = '/' + User.Identity.GetUserName() + '/' + siteUrl + '/' + mainPageUrl
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
            AddPageToMenu(site, page.Name, '/' + userName + '/' + siteUrl + '/' + page.Url);       
            site.Pages.Add(page);
            return new RedirectResult('/' + userName + '/' +
                siteUrl + '/' + page.Url + "/edit");
        }

        private void AddPageToMenu(Site site, string title, string link)
        {
            JObject menuJson = JObject.Parse(site.MenuJson);
            JArray menu = (JArray)menuJson[Request.Form["menuType"]];
            JObject jsonObject = new JObject();
            jsonObject.Add("title", title);
            jsonObject.Add("link", link);
            menu.Add(jsonObject);
            site.MenuJson = menuJson.ToString();
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
        public ActionResult Edit(string userName, string siteUrl, string pageUrl, string contentJson, string menuJson, bool allowComments = true, bool allowRating = true)
        {
            Site editedSite = SitesRepository.GetSite(userName + siteUrl);
            if (editedSite == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaveToggles(editedSite, allowComments, allowRating);
            SaveJson(editedSite, pageUrl, contentJson, menuJson);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void SaveToggles(Site site, bool allowComments, bool allowRating)
        {
            site.AllowComments = allowComments;
            site.AllowRating = allowRating;
        }

        private void FillDetailsViewModel(DetailsViewModel detailsModel, Site site, Page page, bool isEdit)
        {
            detailsModel.MenuJson = site.MenuJson;
            detailsModel.Theme = site.Theme;
            detailsModel.ContentJson = page.ContentJson;
            detailsModel.SiteName = site.Name;
            detailsModel.IsEdit = isEdit;
            detailsModel.AllowComments = site.AllowComments;
            if (page.Url == mainPageUrl)
                detailsModel.Comments = site.Comments;
            detailsModel.AllowRating = site.AllowRating;
            detailsModel.Rating = site.Rating;
            detailsModel.AlreadyRated = site.RatedUsers.Contains(User.Identity.GetUserId());
        }

        private DetailsViewModel CreateDetailsViewModel(Site site, string pageUrl, bool isEdit)
        {
            Page page = site.Pages.FirstOrDefault(requiredPage => requiredPage.Url == pageUrl);
            if (page == null)
            {
                return null;
            }
            DetailsViewModel detailsModel = new DetailsViewModel();
            FillDetailsViewModel(detailsModel, site, page, isEdit);
            return detailsModel;
        }

        [Authorize]
        public ActionResult Edit(string userName, string siteUrl, string pageUrl)
        {
            if (CheckCurrentUser(userName))
            {
                Site editedSite = SitesRepository.GetSite(userName + siteUrl);
                if (editedSite == null)
                {
                    return new RedirectResult('/' + userName + '/' + siteUrl + "/edit");
                }
                DetailsViewModel editModel = CreateDetailsViewModel(editedSite, pageUrl, true);
                if (editModel == null)
                {
                    return HttpNotFound();
                }
                FillDetailsFromJson(editedSite, editModel);
                return View(editModel);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        private void FillDetailsFromJson(Site site, DetailsViewModel editModel)
        {
            JToken token = JObject.Parse(site.MenuJson);
            editModel.horizontalMenu = (bool)token.SelectToken("horizontal_menu_exist");
            editModel.verticalMenu = (bool)token.SelectToken("vertical_menu_exist");
            editModel.Template = (string)JObject.Parse(editModel.ContentJson).SelectToken("content_template");
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditSite(string userName, string siteUrl, string pageUrl, string contentJson)
        {
            if (CheckCurrentUser(userName))
            {
                string editedSiteId = userName + siteUrl;
                Site editedSite = SitesRepository.GetSite(editedSiteId);
                if (editedSite == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SaveJson(editedSite, pageUrl, contentJson, null);
                PutSiteToDb(editedSite, editedSiteId);
                SitesRepository.Remove(editedSiteId);
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
                Page savedPage = site.Pages.FirstOrDefault(page => page.Url == pageUrl);
                if (savedPage != null)
                {
                    savedPage.ContentJson = contentJson;
                }
            }
        }

        private void CreateSiteInDb(Site site)
        {
            try
            {
                foreach (Comment comment in site.Comments)
                {
                    try
                    {
                        db.Entry(comment.Author).State = EntityState.Modified;
                    }catch { }
                    db.Entry(comment).State = EntityState.Added;
                }
                db.Sites.Add(site);
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
        }

        public void UpdateSite(Site editedSite)
        {
            foreach (Comment comment in editedSite.Comments)
            {
                if (comment.Id != 0)
                {
                    db.Entry(comment).State = EntityState.Modified;
                    db.Entry(comment.Author).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(comment).State = EntityState.Added;
                }
            }

            foreach (Page page in editedSite.Pages)
            {
                if (page.Id != 0)
                {
                    db.Entry(page).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(page).State = EntityState.Added;
                }
            }
            
            db.Entry(editedSite).State = EntityState.Modified;
            db.SaveChanges();
        }

        [Authorize]
        public ActionResult EditSite(string userName, string siteUrl)
        {
            if (CheckCurrentUser(userName))
            {
                if (SitesRepository.GetSite(userName + siteUrl) == null)
                {
                    if (!AddSiteToRepository(userName, siteUrl, true))
                        return HttpNotFound();                   
                }
                return new RedirectResult('/' + userName + '/' + siteUrl + '/' + mainPageUrl + "/edit");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        private bool AddSiteToRepository(string userName, string siteUrl, bool siteExists)
        {
            Site editedSite = db.Sites.Include(site => site.Author)
                    .Where(site => site.Url == siteUrl && site.Author.UserName == userName)
                    .FirstOrDefault();
            if (editedSite == null)
            {
                return false;
            }
            SitesRepository.Add(editedSite, siteExists, userName);
            return true;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string userName, string siteUrl, string pageUrl, string menuJson)
        {
            Site deletedPageSite = SitesRepository.GetSite(userName + siteUrl);
            if (deletedPageSite != null)
            {
                deletedPageSite.MenuJson = menuJson;               
                DeletePageFromSite(deletedPageSite, pageUrl);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }

        private void DeletePageFromSite(Site deletedPageSite, string pageUrl)
        {
            Page deletedPage = deletedPageSite.Pages.FirstOrDefault(page => page.Url == pageUrl);
            if (deletedPage != null)
            {
                deletedPageSite.Pages.Remove(deletedPage);
                if (db.Pages.Any(page => page.Id == deletedPage.Id))
                {
                    db.Pages.Attach(deletedPage);
                    db.Pages.Remove(deletedPage);
                    db.SaveChanges();
                }
            }
        }

        [Authorize]
        public ActionResult DeleteSite(string userName, string siteUrl)
        {
            if (CheckCurrentUser(userName))
            {
                Site deletedSite = db.Sites.Include(site => site.Author)
                    .Where(site => site.Url == siteUrl && site.Author.UserName == userName)
                    .FirstOrDefault();
                SitesRepository.Remove(deletedSite.Author.UserName + deletedSite.Url);
                if (deletedSite == null)
                {
                    return HttpNotFound();
                }
                return View(CreateDeleteSiteViewModel(deletedSite));
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        private DeleteSiteViewModel CreateDeleteSiteViewModel(Site deletedSite)
        {
            DeleteSiteViewModel viewModel = new DeleteSiteViewModel();
            viewModel.Name = deletedSite.Name;
            viewModel.Url = deletedSite.Url;
            viewModel.Rating = deletedSite.Rating;
            return viewModel;
        }

        [Authorize]
        [HttpPost, ActionName("DeleteSite")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string userName, string siteUrl)
        {
            Site deletedSite = db.Sites.Include(site => site.Author)
                   .Where(site => site.Url == siteUrl && site.Author.UserName == userName)
                   .FirstOrDefault();
            db.Sites.Remove(deletedSite);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        [Route("add_comment")]
        public string AddComment(string userName, string siteUrl, string commentText)
        {
            Site site = SitesRepository.GetSite(userName + siteUrl);
            if (site == null) { return null; }
            Comment comment = FillComment(site, commentText);
            site.Comments.Insert(0, comment);
            
            //db.Comments.Add(comment);
            //db.Entry(comment).State = EntityState.Added;
            //try
            //{
            //    db.SaveChanges();
            //}           
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Trace.TraceInformation("Property: {0} Error: {1}",
            //                                    validationError.PropertyName,
            //                                    validationError.ErrorMessage);
            //        }
            //    }
            //}
            return GetCommentJsonString(comment);
        }

        private string GetCommentJsonString(Comment comment)
        {
          //  ApplicationUser author = FindUserInDb(User.Identity.Name);
            var commentJson = new CommentJsonModel()
            {
                Text = comment.Text,
                AuthorName = comment.Author.UserName,
                AuthorPicture = comment.Author.Picture,
                DateTime = comment.Date.ToString()
            };
            return JsonConvert.SerializeObject(commentJson);
        }

        private Comment FillComment(Site site, string commentText)
        {
            ApplicationUser currentUser = FindUserInDb(User.Identity.Name);
            Comment comment = new Comment(currentUser, commentText, site);
            return comment;
        }

        [HttpPost]
        [Authorize]
        [Route("add_rating")]
        public int AddRating(string userName, string siteUrl)
        {
            Site site = SitesRepository.GetSite(userName + siteUrl);
            if (site == null) { return 0; }
            if (site.RatedUsers.Contains(User.Identity.GetUserId()))
            {
                site.Rating--;
                site.RatedUsers.Remove(User.Identity.GetUserId());
            }
            else
            {
                site.Rating++;
                site.RatedUsers.Add(User.Identity.GetUserId());
            }
            return site.Rating;
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
