using BDProiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDProiect.Controllers
{
    public class PostsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts;
            ViewBag.Posts = posts;
            return View();
        }

        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            ViewBag.Post = post;
            return View();
        }

        public ActionResult New()
        {
            Post post = new Post();
            return View(post);
        }

        [HttpPost]
        public ActionResult New (Post post)
        {
            post.Date = DateTime.Now;
            try
            {
                db.Posts.Add(post);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost adaugat";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(post);
            }
        }


    }
}