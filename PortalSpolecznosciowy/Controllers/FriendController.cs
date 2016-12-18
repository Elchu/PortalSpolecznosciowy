using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalSpolecznosciowy.Models;
using PortalSpolecznosciowy.Models.ViewModel;

namespace PortalSpolecznosciowy.Controllers
{
    public class FriendController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Parametr okresla id przyjaciela</param>
        /// <returns></returns>
        public ActionResult Show(string id)
        {
            ApplicationUser user = _db.Users.Find(id);
            string userLoggedId = User.Identity.GetUserId();
            if (user == null)
            {
                return HttpNotFound();
            }

            var isFriend = _db.Friend.FirstOrDefault(
                s =>
                    (s.UserFriendId.Equals(id) && s.UserId.Equals(userLoggedId)) ||
                    (s.UserFriendId.Equals(userLoggedId) && s.UserId.Equals(id)));

            UserFriendViewModel uf = new UserFriendViewModel()
            {
                User = user,
                IsFriend = isFriend != null ? true : false,
                IsAccept = isFriend != null ? isFriend.Accepted : false

            };

            return View("Show", uf);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(string friendId)
        { 
            string userLoggedId = User.Identity.GetUserId();

            var isFriend = _db.Friend.FirstOrDefault(
                s =>
                    (s.UserFriendId.Equals(friendId) && s.UserId.Equals(userLoggedId)) ||
                    (s.UserFriendId.Equals(userLoggedId) && s.UserId.Equals(friendId)));

            if (isFriend == null)
            {
                Friend newFriend = new Friend()
                {
                    UserFriendId = friendId,
                    UserId = User.Identity.GetUserId(),
                    Time = DateTime.Now
                };
                _db.Friend.Add(newFriend);

                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Message"] = "Wystąpił błąd podczas dodawania znajomego. Spróbuj później";
                }
            }
            else
            {
                TempData["Message"] = "Taka znajomość już istnieje!!!";
            }

            return RedirectToAction("Show", "ProfilUser", new { id = friendId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Accept(string friendId)
        {
            return View();
        }
    }
}