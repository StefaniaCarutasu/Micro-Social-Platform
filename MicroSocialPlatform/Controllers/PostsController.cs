using BDProiect.Models;
using MicroSocialPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDProiect.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Posts
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index()
        {
            var posts = db.Posts.Include("Group");
            ViewBag.Posts = posts;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            ViewBag.Post = post;
            ViewBag.Group = post.Group;

            return View();
        }

        [Authorize(Roles = "Editor,Admin")]
        public ActionResult New()
        {
            Post post = new Post();
            post.UserId = User.Identity.GetUserId();
            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult New (Post post)
        {
            post.Date = DateTime.Now;

            post.UserId = User.Identity.GetUserId();

            post.GroupId = 1;
            try
            {
                if(ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "Postarea a fost adaugata";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(post);
                }
               
            }
            catch (Exception e)
            {
                return View(post);
            }
        }

        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(post);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                Post post = db.Posts.Find(id);
                if(TryUpdateModel(post))
                {
                    post.Content = requestPost.Content;
                    post.Date = requestPost.Date;
                    post.GroupId = requestPost.GroupId;
                    db.SaveChanges();
                    TempData["message"] = "Postarea a fost editat cu succes";
                    return RedirectToAction("Index");

                }
                return View(requestPost);
            }
            catch(Exception e)
            {
                return View(requestPost);
            }
        }


        [HttpDelete]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}