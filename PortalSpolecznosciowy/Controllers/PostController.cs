using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSpolecznosciowy.Models;
using PortalSpolecznosciowy.Models.ViewModel;

namespace PortalSpolecznosciowy.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(string userId, string Content)
        {
            if (string.IsNullOrEmpty(Content))
                ModelState.AddModelError("Content", "Dodaj tekst do wiadomości");


            if (ModelState.IsValid)
            {
                Post newPost = new Post()
                {
                    Content = Content,
                    Date = DateTime.Now,
                    UserId = userId
                };

                _db.Post.Add(newPost);

                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Message"] = "Wystąpił błąd podczas dodawania wpisu. Spróbuj później";
                }

            }

            return RedirectToAction("Show", "ProfilUser", new { id = userId });
        }

        public ActionResult ShowSinglePost(int id)
        {
            return View();
        }
    }
}