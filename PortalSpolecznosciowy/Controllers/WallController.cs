﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalSpolecznosciowy.Models;
using PortalSpolecznosciowy.Models.HelpersClass;
using PortalSpolecznosciowy.Models.ViewModel;
using PagedList;

namespace PortalSpolecznosciowy.Controllers
{
    public class WallController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            string loggedUserId = User.Identity.GetUserId();
            ApplicationUser userLogged = _db.Users.Single(u => u.Id == loggedUserId);

            //pobieram wszystkich zaakceptowanych znajomych uzytkownika zalogowanego
            UserFriends userfriends = new UserFriends();

            //przekonwertowanie na liste zeby dodac uzytkownika
            List<ApplicationUser> friendUserAll = userfriends.ListOfFriendsUser(userLogged.Id).ToList();
            
            //dodanie zalogowanego uzytkownika aby mozna bylo wyswietlic jego posty wraz ze znajomymi
            friendUserAll.Add(userLogged);  

            //wybranie wszystich postow znajomych, aby zrobic paginacje
            IEnumerable<Post> listaPost = friendUserAll.SelectMany(p => p.Post).OrderByDescending(d=>d.Date).Where(p=>p.IsDeleted == false);
            ViewBag.User = userLogged;

            //polubienia uzytkownika
            TempData["UserLike"] = _db.Like.Where(l => l.UserId == loggedUserId).ToList();

            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(listaPost.ToPagedList(pageNumber, pageSize));
        }
    }
}