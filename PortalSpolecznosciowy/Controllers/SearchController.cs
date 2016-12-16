using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSpolecznosciowy.Models;

namespace PortalSpolecznosciowy.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserSearch(string searchUser)
        {
            List<ApplicationUser> userList = _db.Users.ToList();

            if (!string.IsNullOrEmpty(searchUser))
                userList = userList.Where(u => u.FullName.Contains(searchUser)).ToList();

            return View(userList);
        }
    }
}