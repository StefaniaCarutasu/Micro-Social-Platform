using MicroSocialPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroSocialPlatform.Controllers
{
    public class ProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Profiles

        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            ViewBag.User = user;
            return View();
        }
        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.User = user;
            return View(user);
        }
    }
}