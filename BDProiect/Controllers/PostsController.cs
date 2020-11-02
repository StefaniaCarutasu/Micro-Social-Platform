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
            var post = db.Posts;
            ViewBag.Posts = post;
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
            return View();
        }

        [HttpPost]
        public ActionResult New (Post post)
        {
            try
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }


    }
}