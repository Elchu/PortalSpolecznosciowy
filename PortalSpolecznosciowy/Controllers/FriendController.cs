using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalSpolecznosciowy.Models;
using PortalSpolecznosciowy.Models.HelpersClass;
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

            //pobieram wszystkich znajomych uzytkownika
            UserFriends userFriends = new UserFriends();
            IEnumerable<ApplicationUser> friendUserAll = userFriends.ListOfFriendsUser(id);

            ViewBag.User = _db.Users.FirstOrDefault(u => u.Id == userLoggedId);
            //polubienia uzytkownika
            TempData["UserLike"] = _db.Like.Where(l => l.UserId == userLoggedId).ToList();

            UserFriendViewModel uf = new UserFriendViewModel()
            {
                User = user,
                IsFriend = isFriend != null ? true : false,
                IsAccept = isFriend != null ? isFriend.Accepted : false,
                AllFriendsUser = friendUserAll
            };

            return View("Show", uf);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(string friendId)
        { 
            string userLoggedId = User.Identity.GetUserId();

            string fullUserName = _db.Users.FirstOrDefault(n => n.Id == userLoggedId).FullName;
            string fullFriendName = _db.Users.FirstOrDefault(n => n.Id == friendId).FullName;

            //sprawdzam czy znajomosc nie istnieje w bazie, jesli nie to mozemy dodac znajomosc
            var isFriend = _db.Friend.FirstOrDefault(
                s =>
                    (s.UserFriendId.Equals(friendId) && s.UserId.Equals(userLoggedId)) ||
                    (s.UserFriendId.Equals(userLoggedId) && s.UserId.Equals(friendId)));

            if (isFriend == null)
            {
                //dodajemy znajomosc gdzie userId to zalogowany uzytkownik(zapraszajacy), a userFriend to osoba zaproszona
                Friend newFriend = new Friend()
                {
                    UserFriendId = friendId,
                    UserId = User.Identity.GetUserId(),
                    Time = DateTime.Now
                };
                _db.Friend.Add(newFriend);

                //wysylamy powiadomienie o dodaniu zaproszenia przez uzytkownika
                Notification newNotification = new Notification()
                {
                    FromWhoId = userLoggedId,
                    FullNameFromWho = fullUserName,
                    ToWhomId = friendId,
                    FullNameToWhom = fullFriendName,
                    Message = "Zaproszenie do znajomych.",
                    CreateDate = DateTime.Now
                };
                _db.Notification.Add(newNotification);

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
            string userLoggedId = User.Identity.GetUserId();

            string fullUserName = _db.Users.FirstOrDefault(n => n.Id == userLoggedId).FullName;
            string fullFriendName = _db.Users.FirstOrDefault(n => n.Id == friendId).FullName;

            //friendId to osoba ktora wyslala zaproszenie, userFriendId to osoba przyjmujaca zaproszenie
            var friend = _db.Friend.FirstOrDefault(f => f.UserFriendId == userLoggedId && f.UserId == friendId);

            if (friend != null)
            {
                friend.Accepted = true;
                _db.Entry(friend).State = EntityState.Modified;

                //wysylamy powiadomienie o akceptacji zaproszenia
                Notification newNotification = new Notification()
                {
                    FromWhoId = userLoggedId,
                    FullNameFromWho = fullUserName,
                    ToWhomId = friendId,
                    FullNameToWhom = fullFriendName,
                    Message = "Zaproszenie zostało zaakceptowane.",
                    CreateDate = DateTime.Now
                };
                _db.Notification.Add(newNotification);

                try
                {
                    _db.SaveChanges();
                    TempData["Message"] = "Znajmość z użytkownikiem została zawarta!!!";
                }
                catch (Exception)
                {
                    TempData["Message"] = "Wystąpił błąd podczas zawierania znajmości!!!";
                }
            }
            return RedirectToAction("Show", "ProfilUser", new { id = userLoggedId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFriend(string friendId)
        {
            string userLoggedId = User.Identity.GetUserId();
            var friend = _db.Friend.FirstOrDefault(
                f => f.UserFriendId == userLoggedId && f.UserId == friendId && f.Accepted || 
                f.UserFriendId == friendId && f.UserId == userLoggedId && f.Accepted);

            if (friend != null)
            {
                _db.Friend.Remove(friend);

                try
                {
                    _db.SaveChanges();
                    TempData["Message"] = "Znajmość z użytkownikiem została usunięta!!!";
                }
                catch (Exception)
                {
                    TempData["Message"] = "Wystąpił błąd podczas usuwania znajmości!!!";
                }
            }
            return RedirectToAction("Show", "ProfilUser", new {id = userLoggedId});
        }

        public ActionResult ListOfFriends(string id)
        {
            //id osoby szukajacej znajomych
            string loggedUserId = id;

            //pobieram wszystkich znajomych uzytkownika
            UserFriends uf = new UserFriends();
            IEnumerable<ApplicationUser> friendUserAll = uf.ListOfFriendsUser(loggedUserId);

            return View(friendUserAll);
        }
    }
}