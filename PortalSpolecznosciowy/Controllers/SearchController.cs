using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSpolecznosciowy.Models;
using PagedList;

namespace PortalSpolecznosciowy.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult UserSearch(string searchUser, int? page)
        {
            List<ApplicationUser> userList = _db.Users.ToList();

            if (!string.IsNullOrEmpty(searchUser))
            {
                userList = userList.Where(u => u.FullName.ToLower().Contains(searchUser.ToLower())).ToList();
            }

            page = 1;
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(userList.ToPagedList(pageNumber, pageSize));

        }
    }
}