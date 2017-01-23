using System;
using System.Collections.Generic;
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

            IQueryable<Notification> notificationList = _db.Notification.Where(n => n.ToWhomId.Equals(userLoggedId) && n.RecipientHasRead == false);

            return PartialView("_notificationCount", notificationList);
        }
    }
}