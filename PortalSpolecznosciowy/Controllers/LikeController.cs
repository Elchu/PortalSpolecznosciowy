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
    public class LikeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(string userId, int? postId = null, int? commentId = null)
        {
            if (User.Identity.IsAuthenticated && User.Identity.GetUserId().Equals(userId))
            {
                Like newLike = new Like()
                {
                    CommentId = commentId,
                    PostId = postId,
                    UserId = userId,
                    Data = DateTime.Now
                };

                _db.Like.Add(newLike);

                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    //obsluzyc blad
                }
            }

            return RedirectToAction("Index", "Wall");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(string userId, int? postId = null, int? commentId = null)
        {
            string userLoggedId = User.Identity.GetUserId();

            if (User.Identity.IsAuthenticated && userLoggedId.Equals(userId))
            {
                //sprawdzam czy usuwamy komentarz czy post z polubien
                Like newLike = postId != null ? _db.Like.FirstOrDefault(l => l.PostId == postId && l.UserId == userLoggedId) : _db.Like.FirstOrDefault(l => l.CommentId == commentId && l.UserId == userLoggedId);

                if (newLike == null)
                    return RedirectToAction("Index", "Wall");

                _db.Like.Remove(newLike);

                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    //obsluzyc blad
                }
            }
            return RedirectToAction("Index", "Wall");
        }
    }
}