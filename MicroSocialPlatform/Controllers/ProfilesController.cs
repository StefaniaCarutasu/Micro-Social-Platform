using MicroSocialPlatform.Models;
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
                             select post).ToList().OrderByDescending(post => post.Date);
            ViewBag.ProfileDescription = user.ProfileDescription;
            ViewBag.UserId = id;
            ViewBag.User = user;
            ViewBag.CurrentUser = db.Users.Find(User.Identity.GetUserId());
            string currentId = User.Identity.GetUserId();
            var fr = (from friend in db.Friends
                      where friend.User2_Id == id && friend.Accepted == false
                      select friend);
            ViewBag.FriendshipRequests = fr;
            ViewBag.FrReqCount = fr.Count();
            var isFriend = (from friend in db.Friends
                            where ((friend.User2_Id == id) || (friend.User1_Id == id)) && friend.Accepted == true
                            select friend).ToList();
            ViewBag.UserFriends = isFriend;
            int count = 0;
            foreach (var fre in db.Friends)
            {
                if ((fre.User1_Id == ViewBag.User.Id) || (fre.User2_Id == ViewBag.User.Id))
                {

                    if (fre.Accepted == true)
                    {
                        count++;
                    }
                }
            }
            ViewBag.Count = count;
            var users = from usr in db.Users
                        orderby usr.UserName
                        select usr;
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<string> userIds = db.Users.Where(
                    us => us.UserName.Contains(search)).Select(u => u.Id).ToList();
                users = (IOrderedQueryable<ApplicationUser>)db.Users.Where(usr => userIds.Contains(usr.Id));
                ViewBag.CountUsers = users.Count();
            }
            else
            {
                ViewBag.CountUsers = 0;
            }


            ViewBag.UsersList = users;
            return View();
        }
        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.Posts = (from post in db.Posts
                            where post.UserId == id
                            select post).ToList().OrderByDescending(post => post.Date);
            ViewBag.Profile = from profile in db.Profiles
                              where profile.UserId == id
                              select profile;
            ViewBag.User = user;
            ViewBag.ProfileDescription = user.ProfileDescription;
            ViewBag.CurrentUser = db.Users.Find(User.Identity.GetUserId());
           string currentId = User.Identity.GetUserId();
            ViewBag.Friends = db.Friends;
            ViewBag.FriendCount = db.Friends.Count();
            var isFriend = (from friend in db.Friends
                           where ((friend.User2_Id == id)|| (friend.User1_Id == id)) && friend.Accepted == true
                           select friend).ToList();
            ViewBag.UserFriends = isFriend;
            ViewBag.isAdmin = false;
            if (User.IsInRole("Admin"))
            {
                ViewBag.isAdmin = true;
            }
            int count = 0;
            foreach (var fr in db.Friends)
            {
                if ((fr.User1_Id == ViewBag.User.Id) || (fr.User2_Id == ViewBag.User.Id))
                {

                    if (fr.Accepted == true)
                    {
                        count++;
                    }
                }
            }
            ViewBag.Count = count;
            var users = from usr in db.Users
                        orderby usr.UserName
                        select usr;
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<string> userIds = db.Users.Where(
                    us => us.UserName.Contains(search)).Select(u => u.Id).ToList();
                users = (IOrderedQueryable<ApplicationUser>)db.Users.Where(usr => userIds.Contains(usr.Id));
                ViewBag.CountUsers = users.Count();
            }
            else
            {
                ViewBag.CountUsers = 0;
            }


            ViewBag.UsersList = users;


            return View(user);
        }
        [Authorize(Roles = "User,Admin")]
        public ActionResult ShowFriends(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.Profile = from profile in db.Profiles
                              where profile.UserId == id
                              select profile;
            ViewBag.User = user;
            ViewBag.CurrentUser = db.Users.Find(User.Identity.GetUserId());
            string currentId = User.Identity.GetUserId();
            ViewBag.Friends = db.Friends;
            var fr = (from friend in db.Friends
                      where friend.User2_Id == id && friend.Accepted == false
                      select friend);
            ViewBag.FriendshipRequests = fr;
            ViewBag.FrReqCount = fr.Count();
            int count = 0;
            foreach (var fre in db.Friends)
            {
                if ((fre.User1_Id == ViewBag.User.Id) || (fre.User2_Id == ViewBag.User.Id))
                {

                    if (fre.Accepted == true)
                    {
                        count++;
                    }
                }
            }
            ViewBag.Count = count;
            ViewBag.FriendCount = db.Friends.Count();
            var isFriend = (from friend in db.Friends
                            where ((friend.User2_Id == id) || (friend.User1_Id == id)) && friend.Accepted == true
                            select friend).ToList();
            ViewBag.UserFriends = isFriend;
            ViewBag.ProfileDescription = user.ProfileDescription;
            ViewBag.isAdmin = false;
            if (User.IsInRole("Admin"))
            {
                ViewBag.isAdmin = true;
            }
            var users = from usr in db.Users
                        orderby usr.UserName
                        select usr;
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<string> userIds = db.Users.Where(
                    us => us.UserName.Contains(search)).Select(u => u.Id).ToList();
                users = (IOrderedQueryable<ApplicationUser>)db.Users.Where(usr => userIds.Contains(usr.Id));
                ViewBag.CountUsers = users.Count();
            }
            else
            {
                ViewBag.CountUsers = 0;
            }


            ViewBag.UsersList = users;

            return View(user);
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Notifications()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            ViewBag.Posts = (from post in db.Posts
                         where post.UserId == id
                         select post).ToList();
            ViewBag.ProfileDescription = user.ProfileDescription;
            ViewBag.UserId = id;
            ViewBag.User = user;
            ViewBag.CurrentUser = db.Users.Find(User.Identity.GetUserId());
            string currentId = User.Identity.GetUserId();
            var fr = (from friend in db.Friends
                  where friend.User2_Id == id && friend.Accepted == false
                  select friend);
            ViewBag.FriendshipRequests = fr;
            ViewBag.FrReqCount = fr.Count();
            int count = 0;
            foreach (var fre in db.Friends)
            {
                if ((fre.User1_Id == ViewBag.User.Id) || (fre.User2_Id == ViewBag.User.Id))
                {

                    if (fre.Accepted == true)
                    {
                        count++;
                    }
                }
            }
            ViewBag.Count = count;
            var users = from usr in db.Users
                        orderby usr.UserName
                        select usr;
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<string> userIds = db.Users.Where(
                    us => us.UserName.Contains(search)).Select(u => u.Id).ToList();
                users = (IOrderedQueryable<ApplicationUser>)db.Users.Where(usr => userIds.Contains(usr.Id));
                ViewBag.CountUsers = users.Count();
            }
            else
            {
                ViewBag.CountUsers = 0;
            }


            ViewBag.UsersList = users;
            return View(user);
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            return View(user);
        }

        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(ApplicationUser requestUser)
        {
            string id = User.Identity.GetUserId();
            try
            {
                ApplicationUser user = db.Users.Find(id);
                if (TryUpdateModel(user))
                {
                    user.UserName = requestUser.UserName;
                    user.BirthDate = requestUser.BirthDate;
                    user.City = requestUser.City;
                    user.PhoneNumber = requestUser.PhoneNumber;
                    user.ProfileDescription = requestUser.ProfileDescription;
                    user.Visibility = requestUser.Visibility;
                    db.SaveChanges();
                    TempData["message"] = "Profilul a fost editat cu succes";
                    return RedirectToAction("Index");

                }
                return View(requestUser);
            }
            catch (Exception e)
            {
                return View(requestUser);
            }
        }
    }
}