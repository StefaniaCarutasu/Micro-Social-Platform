using MicroSocialPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroSocialPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Users
        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;
            return View();
        }
    }
}