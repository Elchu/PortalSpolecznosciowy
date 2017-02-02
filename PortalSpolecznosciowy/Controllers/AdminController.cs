using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PortalSpolecznosciowy.Models;

namespace PortalSpolecznosciowy.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }


        #region Autoryzacja

        public ActionResult ListaRol()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }

        public ActionResult UtworzRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UtworzRole(FormCollection collection)
        {
            try
            {
                _db.Roles.Add(
                    new IdentityRole()//Microsoft.AspNet.Identity.EntityFramework.
                    {
                        Name = collection["RoleName"]
                    });
                _db.SaveChanges();
                ViewBag.ResultMessage = "Rola utworzona pomyślnie";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EdytujRole(string RoleName)
        {

            var role = _db.Roles.FirstOrDefault(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase));

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EdytujRole(IdentityRole role)
        {
            try
            {
                var isRole = _db.Roles.FirstOrDefault(r => r.Name.Equals(role.Name, StringComparison.CurrentCultureIgnoreCase));
                if (isRole == null)
                {
                    _db.Entry(role).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("ListaRol");
                }
                else
                {
                    ViewBag.Blad = "Rola o takiej nazwie już istnieje";
                    return View(role);
                }
            }
            catch
            {
                return View();

            }
        }

        public ActionResult DodajRoleDoUzytkownika()
        {
            var list = _db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var userList = _db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

            ViewBag.Roles = list;
            ViewBag.Users = userList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajRoleDoUzytkownika(string UserName, string RoleName)
        {
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.InvariantCultureIgnoreCase));
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            um.AddToRole(user.Id, RoleName);

            var list = _db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = list;
            ViewBag.Powterdzenie = "Rola została dodana";

            return RedirectToAction("ListaRol");
        }

        public ActionResult WyswietlRoleUzytkownika()
        {
            var userList = _db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WyswietlRoleUzytkownika(string UserName)
        {
            var list = _db.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                           new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }
                           ).ToList();
            ViewBag.Roles = list;

            var userList = _db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userList;

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = _db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>
                (new ApplicationDbContext()));

                ViewBag.RolesForThisUser = um.GetRoles(user.Id);
            }
            return View("WyswietlRoleUzytkownika");
        }

        public ActionResult UsunRoleUzytkownikowi()
        {
            var list = _db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var userList = _db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

            ViewBag.Roles = list;
            ViewBag.Users = userList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsunRoleUzytkownikowi(string UserName, string RoleName)
        {

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));

            if (um.IsInRole(user.Id, RoleName))
            {
                um.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultSuccess = "Rola została usunięta!";
            }
            else
            {
                ViewBag.ResultWarning = "Ten użytkownik nie posiada takiej roli.";
            }

            // prepopulat roles for the view dropdown

            var list = _db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = list;

            return RedirectToAction("ListaRol");

        }
        #endregion //Autoryzacja
    }
}