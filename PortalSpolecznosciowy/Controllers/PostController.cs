using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalSpolecznosciowy.Models;
using PortalSpolecznosciowy.Models.ViewModel;

namespace PortalSpolecznosciowy.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(string userId, string content)
        {
            if (string.IsNullOrEmpty(content))
                ModelState.AddModelError("Content", "Dodaj tekst do wiadomości");


            if (ModelState.IsValid)
            {
                Post newPost = new Post()
                {
                    Content = content,
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
        /// <summary>
        /// Show post
        /// </summary>
        /// <param name="id">ID display post</param>
        /// <returns></returns>
        public ActionResult ShowSinglePost(int id)
        {
            Post post = _db.Post.FirstOrDefault(p => p.PostId == id);
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == post.UserId);

            if (post == null || user == null)
                return HttpNotFound();

            SinglePostUser singlePostUser = new SinglePostUser()
            {
                Post = post,
                User = user
            };

            return View(singlePostUser);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Post editPost = _db.Post.FirstOrDefault(p => p.PostId == id);

            if (editPost == null || User.Identity.GetUserId() != editPost.UserId)
                return HttpNotFound();

            return View(editPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (!_db.Post.Any(p=>p.PostId == post.PostId))
                return HttpNotFound();

            post.Date = DateTime.Now;
            _db.Entry(post).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
                return RedirectToAction("Show", "ProfilUser", new {id = post.UserId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Wystąpił błąd podczas edytowania. Sprubój później");
            }
            return View(post);
        }
    }
}