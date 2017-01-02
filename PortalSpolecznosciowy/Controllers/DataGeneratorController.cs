using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PortalSpolecznosciowy.Models;

namespace PortalSpolecznosciowy.Controllers
{
    public class DataGeneratorController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GenerujDane(string email, string fullName, string image, string pass, string sex)
        {

            var store = new UserStore<ApplicationUser>(_db);
            var manager = new ApplicationUserManager(store);
            var user = new ApplicationUser()
            {
                Email = email,
                UserName = email,
                FullName = fullName,
                Image = image,
                Sex = sex == "Pani" ? "f" : "m"
            };
            manager.Create(user, "Haslo!1qaz");

            return Json(new { success = "Dane zostały zapisane" });
        }
    }
}