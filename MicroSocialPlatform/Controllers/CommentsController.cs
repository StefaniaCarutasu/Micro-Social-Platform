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
    public class CommentsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comments
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New(Comment comment)
        {
            comment.Date = DateTime.Now;
            comment.UserId = User.Identity.GetUserId();
            try
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + comment.PostId);
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return Redirect("/Posts/Show/" + comment.PostId);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.UserId == User.Identity.GetUserId() || comment.Post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + comment.PostId);
            }
            else
            {
                return Redirect("/Posts/Show/" + comment.PostId); 
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            ViewBag.Comment = comment;
            if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return Redirect("/Posts/Show/" + comment.PostId);

            }
        }


        [HttpPut]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id, Comment reqComment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comm = db.Comments.Find(id);
                    if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(comm))
                        {
                            comm.Content = reqComment.Content;
                            db.SaveChanges();
                        }
                        return Redirect("/Posts/Show/" + comm.PostId);
                    }
                    else
                    {
                        return Redirect("/Posts/Show/" + comm.PostId);
                    }
                }
                else
                {
                    return View(reqComment);
                }
            }
            catch(Exception e)
            {
                return View();
            }
        }

    }
}