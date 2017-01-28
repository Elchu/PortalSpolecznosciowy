using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalSpolecznosciowy.Models;

namespace PortalSpolecznosciowy.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [ChildActionOnly]
        public ActionResult NotificationCount()
        {
            string userLoggedId = User.Identity.GetUserId();

            //zlicza ile jest powiadomień
            int notificationCount = _db.Notification.Count(n => n.ToWhomId.Equals(userLoggedId) && n.RecipientHasRead == false);

            return PartialView("_notificationCount", notificationCount);
        }

        public ActionResult Index()
        {
            string userLoggedId = User.Identity.GetUserId();

            IQueryable<Notification> notificationList = _db.Notification.Where(n => n.ToWhomId.Equals(userLoggedId) && n.RecipientHasRead == false);

            return View(notificationList);
        }

        public ActionResult Read(int notificationId)
        {
            Notification notification = _db.Notification.FirstOrDefault(n => n.NotificationId == notificationId);

            if (notification != null)
            {
                notification.RecipientHasRead = true;
                _db.Entry(notification).State = EntityState.Modified;

                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["MessageNotification"] = "Wystąpił błąd podczas podczas zapisywanie przeczytania wiadomośći.";
                }
            }

            return RedirectToAction("Index");
        }
    }
}