using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSpolecznosciowy.Models;

namespace PortalSpolecznosciowy.Controllers
{
    public class CommentController : Controller
    {
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Comment comment)
        {
            string contentPost = Request.Form.Keys[3];
            if (!ModelState.IsValid)
                ModelState.AddModelError(contentPost, "Wpisz tekst");

            return RedirectToAction("Index", "Wall");
        }
    }
}