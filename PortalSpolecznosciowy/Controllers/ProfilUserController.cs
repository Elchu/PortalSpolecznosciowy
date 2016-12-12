using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalSpolecznosciowy.Models;

namespace PortalSpolecznosciowy.Controllers
{
    public class ProfilUserController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Show(string id)
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
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
        public ActionResult Edit([Bind(Include = "Id,Sex,FullName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _db.Users.Find(applicationUser.Id);

                if (user != null)
                {
                    user.Sex = applicationUser.Sex;
                    user.FullName = applicationUser.FullName;
                    _db.SaveChanges();
                }
                    
                return RedirectToAction("Index");
            }
            return View(applicationUser);
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
