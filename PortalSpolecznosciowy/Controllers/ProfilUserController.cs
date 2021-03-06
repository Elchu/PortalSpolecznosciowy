﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalSpolecznosciowy.Models;
using PortalSpolecznosciowy.Models.HelpersClass;
using PortalSpolecznosciowy.Models.ViewModel;

namespace PortalSpolecznosciowy.Controllers
{
    public class ProfilUserController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        
        public ActionResult Show(string id)
        {
            string userLoggedId = User.Identity.GetUserId();
            ApplicationUser user = _db.Users.Find(id);
            
            if (user == null || userLoggedId == null)
            {
                return HttpNotFound();
            }
            
            //sprawdzam i pobieram jacy uzytkownicy sa zaproszeni do znajomosci ale jeszcze nie zaakceptowanymi
            var friends = (from f in _db.Friend
                join u in _db.Users on f.UserFriendId equals u.Id
                where f.UserFriendId == id && f.Accepted == false
                select new UserFriendTemp
                {
                    IdUser = f.User.Id,
                    UserFullName = f.User.FullName,
                    UserFriendId = f.UserFriendId,
                    IsAccepted = f.Accepted
                }
                ).ToList();

            //pobieram wszystkich znajomych uzytkownika zalogowanego
            UserFriends userFriends = new UserFriends();
            IEnumerable<ApplicationUser> friendUserAll = userFriends.ListOfFriendsUser(user.Id);

            //uzytkownik zalogowanych do wyswietlenia w komentarzach jako zdjecie
            ViewBag.User = _db.Users.FirstOrDefault(u=>u.Id == userLoggedId);

            //polubienia uzytkownika
            TempData["UserLike"] = _db.Like.Where(l => l.UserId == userLoggedId).ToList();

            //zbieram wszystkie potrzebne informacje do wyswieltenie w widoku
            UserFriendToAcceptedViewModel userFriendsViewModel = new UserFriendToAcceptedViewModel()
            {
                User = user,
                FriendsToAccepted = friends,
                AllFriendsUser = friendUserAll
            };

            return View(userFriendsViewModel);
        }

        public ActionResult ListOfFriends(string id)
        {
            string loggedUserId = User.Identity.GetUserId();
            
            //pobieram wszystkich znajomych uzytkownika zalogowanego
            UserFriends uf = new UserFriends();
            //IEnumerable<ApplicationUser> friendUserAll = uf.ListOfFriendsUser(loggedUserId);
            IEnumerable<ApplicationUser> friendUserAll = uf.ListOfFriendsUser(id);

            return View(friendUserAll);
        }

        public ActionResult Edit(string id)
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            if (id == null || !id.Equals(user.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _db.Users.Find(User.Identity.GetUserId());
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Sex,FullName")] ApplicationUser profilUser, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _db.Users.Find(profilUser.Id);

                if (user != null)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string pathDirectory = Path.Combine(Server.MapPath("~/Content/UploadUsersAvatars"), profilUser.Id);
                        string pathFile = Path.Combine(Server.MapPath("~/Content/UploadUsersAvatars/"+ profilUser.Id), fileName);

                        if (Directory.Exists(pathDirectory))
                            Directory.Delete(pathDirectory, true);
                        
                        Directory.CreateDirectory(pathDirectory);
                        
                        try
                        {
                            file.SaveAs(pathFile);
                            user.Image = Path.GetFileName(file.FileName);
                        }
                        catch (Exception e)
                        {
                            //wypisac komunikat o bledzie
                        }
                    }
                    
                    user.Sex = profilUser.Sex;
                    user.FullName = profilUser.FullName;
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        //tutaj trzeba usunac plik z avatarem jesli zapisywanie do bazy sie nie powiodło
                    }

                }
                return RedirectToAction("Index", "Home");
            }
            return View(profilUser);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
