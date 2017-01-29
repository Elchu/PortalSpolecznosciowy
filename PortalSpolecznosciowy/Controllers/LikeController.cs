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

        /// <summary>
        /// Dodanie polubienia
        /// </summary>
        /// <param name="userId">id uzytkownika ktory polubił</param>
        /// <param name="postId">id posta polubionego</param>
        /// <param name="commentId">id komentarza polubionego</param>
        /// <returns></returns>
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

                //dodajemy nowe polubienie do bazy
                _db.Like.Add(newLike);

                Notification notifi = null;

                //dodajemy powiadomienie jesli polubiony zostal post
                if (postId != null)
                    notifi = AddNotificationPost(userId, postId);

                //dodajemy powiadomienie jesli polubiony zostal komentarz
                if (commentId != null)
                    notifi = AddNotificationComment(userId, commentId);

                //dodajemy do bazy nowe powiadomienie
                _db.Notification.Add(notifi);

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

        /// <summary>
        /// usuniecie polubienia
        /// </summary>
        /// <param name="userId">id uzytkownika ktory nie lubi</param>
        /// <param name="postId">id posta nie lubianego</param>
        /// <param name="commentId">id komentarza nie lubionego</param>
        /// <returns></returns>
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

        /// <summary>
        /// Dodaje powiadomienie do polubionego komentarza
        /// </summary>
        /// <param name="userId">ID uzytkownika ktory polubiol komentarz</param>
        /// <param name="commentId">ID polubionego komentarza</param>
        /// <returns>Zwraca powiadomienie</returns>
        public Notification AddNotificationComment(string userId, int? commentId)
        {
            //powiadomienie o polubieniu komentarza.
            Notification notifi = new Notification()
            {
                CreateDate = DateTime.Now,
                Message = "Polubił Twój komentarz."
            };

            //pobieram komentarz ktory zostal polubiony, aby wyciagnac dane uzytkownika, ktory napisal komentarz.
            Comment comment = _db.Comment.FirstOrDefault(c => c.CommentId == commentId);
            if (comment != null)
            {
                notifi.CommentId = comment.CommentId;
                notifi.FullNameToWhom = comment.User.FullName;
                notifi.ToWhomId = comment.User.Id;
            }

            ApplicationUser userLogged = _db.Users.FirstOrDefault(u => u.Id.Equals(userId));

            if (userLogged != null)
            {
                notifi.FromWhoId = userLogged.Id;
                notifi.FullNameFromWho = userLogged.FullName;
            }

            return notifi;
        }
        
        /// <summary>
        /// Dodaje powiadomienie do polubionego posta
        /// </summary>
        /// <param name="userId">ID uzytkownika ktory polubiol komentarz</param>
        /// <param name="postId">ID polubionego komentarza</param>
        /// <returns>Zwraca powiadomienie</returns>
        public Notification AddNotificationPost(string userId, int? postId)
        {
            //powiadomienie o polubieniu posta.
            Notification notifi = new Notification()
            {
                CreateDate = DateTime.Now,
                Message = "Polubił Twój post."
            };

            //pobieram post ktory zostal polubiony, aby wyciagnac dane uzytkownika, ktory napisal post.
            Post post = _db.Post.FirstOrDefault(p => p.PostId == postId);
            if (post != null)
            {
                notifi.PostId = post.PostId;
                notifi.FullNameToWhom = post.User.FullName;
                notifi.ToWhomId = post.User.Id;
            }

            ApplicationUser userLogged = _db.Users.FirstOrDefault(u => u.Id.Equals(userId));

            if (userLogged != null)
            {
                notifi.FromWhoId = userLogged.Id;
                notifi.FullNameFromWho = userLogged.FullName;
            }

            return notifi;
        }
    }
}