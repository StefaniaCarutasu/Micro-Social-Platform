using BDProiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDProiect.Controllers
{
    public class GroupsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        // GET: Groups
        public ActionResult Index()
        {
            var groups = db.Groups;
            ViewBag.Groups = groups;
            return View();
        }

        public ActionResult Show(int id)
        {
            Group group = db.Groups.Find(id);
            ViewBag.Group = group;
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New (Group gr)
        {
            try
            {
                db.Groups.Add(gr);
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