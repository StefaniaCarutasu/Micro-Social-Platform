﻿using MicroSocialPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            ViewBag.Posts = (from post in db.Posts
                             where post.UserId == id
                             select post).ToList();
            ViewBag.ProfileDescription = user.ProfileDescription;
            ViewBag.UserId = id;
            ViewBag.User = user;
            var fr = (from friend in db.Friends
                      where friend.User2_Id == id && friend.Accepted == false
                      select friend);
            ViewBag.FriendshipRequests = fr;
            ViewBag.FrReqCount = fr.Count();
            return View();
        }
        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.Posts = (from post in db.Posts
                            where post.UserId == id
                            select post).ToList();
            ViewBag.Profile = from profile in db.Profiles
                              where profile.UserId == id
                              select profile;
            ViewBag.User = user;
            ViewBag.CurrentUser = db.Users.Find(User.Identity.GetUserId());
            ViewBag.Friends = db.Friends;
            ViewBag.FriendCount = db.Friends.Count();

            return View(user);


        }
    }
}