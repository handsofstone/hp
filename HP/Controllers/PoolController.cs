using HP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HP.Controllers
{
    public class PoolController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Pool
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Standings()
        {
            var model = new StandingsViewModel();

            if (ModelState.IsValid)
            {
                
                var user = UserManager.FindById(User.Identity.GetUserId());

                model.User = user;
                model.Teams = user.Teams;


            }
            return View(model);
            return View();
        }

        public ActionResult Team()
        {
            return View();
        }
    }
}