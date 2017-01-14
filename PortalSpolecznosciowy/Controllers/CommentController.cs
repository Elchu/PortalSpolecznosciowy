using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSpolecznosciowy.Models;

namespace PortalSpolecznosciowy.Controllers
{
    public class CommentController : Controller
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();
               
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Comment comment)
        {
            //pobranie content z komentarza, poniewaz content ma dynamicznie nazwe generowana
            string contentPost = Request.Form[3];

            if (string.IsNullOrEmpty(contentPost))
            {
                ModelState.AddModelError(Request.Form.Keys[3], "Wpisz komentarz.");
                TempData["Error_" + comment.PostId] = "Komentarz nie może być pusty.";
                return RedirectToAction("Index", "Wall");
            }
                
            comment.Content = contentPost;
            comment.Date = DateTime.Now;
            comment.IsDeleted = false;

            _db.Comment.Add(comment);

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["MessageComment"] = "Wystąpił błąd podczas zapisywania komentarza. Spróbuj później.<br /> " + ex.Message;
            }
            
            return RedirectToAction("Index", "Wall");
        }

        public ActionResult Edit(int id)
        {
            Comment comment = _db.Comment.FirstOrDefault(c => c.CommentId == id);
            if(comment == null)
                return RedirectToAction("Index", "Wall");

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Comment comment)
        {
            if (!_db.Comment.Any(c => c.CommentId == comment.CommentId))
                return HttpNotFound();

            comment.Date = DateTime.Now;
            _db.Entry(comment).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
                return RedirectToAction("Show", "ProfilUser", new { id = comment.UserId });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Wystąpił błąd podczas edytowania. Sprubój później");
            }
            return View(comment);
        }

        public ActionResult Delete(int id)
        {
            if (Request.IsAjaxRequest())
            {
                Comment comment = _db.Comment.FirstOrDefault(c => c.CommentId == id);
                if (comment != null)
                {
                    comment.IsDeleted = true;
                    _db.Entry(comment).State = EntityState.Modified;

                    //wszystkie polubienia nalezace do komentarza
                    List<Like> likes = _db.Like.Where(l => l.CommentId == comment.CommentId).ToList();

                    //usuwamy polubienia komentarza
                    if (likes.Any())
                    {
                        foreach (Like like in likes)
                        {
                            _db.Entry(like).State = EntityState.Deleted;
                        }
                    }

                    try
                    {
                        _db.SaveChanges();
                        return Json(new { message = "Wpis został pomyślnie usunięty" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        return Json(new { message = "Wystąpił błąd podczas usuwania wpisu. Spróbuj później" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { message = "Wystąpił błąd podczas usuwania wpisu. Spróbuj później" }, JsonRequestBehavior.AllowGet);
        }
    }
}