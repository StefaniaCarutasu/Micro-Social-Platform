using MicroSocialPlatform.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroSocialPlatform.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Users


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;
            return View();
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            ViewBag.UserName = user.UserName;
            ViewBag.User = user;
            string currentRole = user.Roles.FirstOrDefault().RoleId;
            var userRoleName = (from role in db.Roles
                                where role.Id == currentRole
                                select role.Name).First();
            ViewBag.roleName = userRoleName;
            return View(user);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            return View(user);
        }
        [NonAction]

        [Authorize(Roles = "Admin")]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();
            var roles = from role in db.Roles select role;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;
                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Name);
                    }
                    var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
                    UserManager.AddToRole(id, selectedRole.Name);
                    db.SaveChanges();
                    TempData["message"] = "Utilizatorul a fost editat cu succes";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newData);
                }
                
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                newData.Id = id;
                return View(newData);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);
            var articles = db.Posts.Where(a => a.UserId == id);
            foreach (var article in articles)
            {
                db.Posts.Remove(article);
            }
            var comments = db.Comments.Where(comm => comm.UserId == id);
            foreach (var comment in comments)
            {
                db.Comments.Remove(comment);
            }
            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult AddFriend(FormCollection formData)
        {
            string currentUser = User.Identity.GetUserId();
            string friendToAdd = formData.Get("UserId"); // TODO: trebuie validare (verificare daca userul exista)

            Friend friendship = new Friend();
            friendship.User1_Id = currentUser;
            friendship.User2_Id = friendToAdd;
            friendship.Accepted = false; // Accepted = false, iar in lista de cereri -> accept
            friendship.RequestTime = DateTime.Now;

            // TODO: sa existe try si catch astfel incat sa nu se trimita o cerere de doua ori
            // verificare daca userul a primit deja cerere de la userul caruia doreste sa ii trimita
            // de verificat ca user1 sa nu fie deja prieten cu user2
            try
            {
                var fr = (from friend in db.Friends
                          where friend.User1_Id == currentUser && friend.User2_Id == friendToAdd
                          select friend).ToList();
                if(fr.Count()==0)
                {
                    db.Friends.Add(friendship);
                    db.SaveChanges();
                    
                }
                return Redirect("/Profiles/Show/" + @friendToAdd);
            }
            catch(Exception e)
            {
                return Redirect("/Profiles/Show/" + @friendToAdd);
            }
            

            
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult AcceptFriendship(FormCollection formData)
        {
            int id =Int32.Parse(formData.Get("FriendId"));
            Friend friendship = db.Friends.Find(id);
            friendship.Accepted = true;
            db.SaveChanges();

            return Redirect("/Profiles/Index");
            
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        public ActionResult DeclineFriendship(int id)
        {
            Friend friendship = db.Friends.Find(id);
            db.Friends.Remove(friendship);
            db.SaveChanges();

            return Redirect("/Profiles/Index");

        }



    }
}