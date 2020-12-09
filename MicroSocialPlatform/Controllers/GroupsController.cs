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
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Groups
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var groups = db.Groups;
            ViewBag.Groups = groups;
            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            Group group = db.Groups.Find(id);
            ViewBag.isAdmin= false;
            ViewBag.isGroupOwner = false;
            ViewBag.UserId = User.Identity.GetUserId();
            if (User.IsInRole("Admin"))
            {
                ViewBag.isAdmin = true;
            }
            if(group.UserId == User.Identity.GetUserId())
            {
                ViewBag.isGroupOwner = true;
            }
         
            ViewBag.Group = group;
            return View(group);
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New()
        {
            Group group = new Group();
            group.UserId = User.Identity.GetUserId();
            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New (Group gr)
        {
            gr.UserId = User.Identity.GetUserId();
            try
            {
                if(ModelState.IsValid)
                {
                    db.Groups.Add(gr);
                    db.SaveChanges();
                    TempData["message"] = "Grupul a fost adaugat!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(gr);
                }
               
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "User,Editor, Admin")]
        public ActionResult Edit (int id)
        {
            Group gr = db.Groups.Find(id);
            if(gr.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(gr);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui grup care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Editor, Admin")]
        public ActionResult Edit (int id, Group requestGroup)
        {
            try
            {
                Group gr = db.Groups.Find(id);
                if(TryUpdateModel(gr))
                {
                    gr.GroupName = requestGroup.GroupName;
                    gr.GroupDescription = requestGroup.GroupDescription;
                    db.SaveChanges();
                    TempData["message"] = "Grupul a fost editat cu succes";
                    return RedirectToAction("Index");
                }
                return View(requestGroup);
            }
            catch(Exception e)
            {
                return View(requestGroup);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete (int id)
        {
            Group gr = db.Groups.Find(id);
            if (gr.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Groups.Remove(gr);
                db.SaveChanges();
                TempData["message"] = "Grupul a fost sters";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un grup care nu va apartine";
                return RedirectToAction("Index");
            }
        }

        //public ActionResult NewMember(ApplicationUser user, int groupId)
        //{
        //    Group group = db.Groups.Find(groupId);
        //    group.Users.Add(user);
        //    db.SaveChanges();

        //    return Redirect("/Groups/Show/" + @group.GroupId);
        //}
    }

   
}