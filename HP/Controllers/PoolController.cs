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

        public ActionResult Standings(int PoolId)
        {
            var model = new StandingsViewModel();

            if (ModelState.IsValid)
            {
                using (var context = new ApplicationDbContext())
                {
                   // model.Seasons = context.Pools.Where(p => p.Id == PoolId).FirstOrDefault<Pool>().Seasons.ToList<Season>();

                    model.Seasons = new SelectList(context.Pools.Where(p => p.Id == PoolId).FirstOrDefault<Pool>().Seasons,"Id","Name").ToList();
                    var user = UserManager.FindById(User.Identity.GetUserId());
                }

            }
            return View(model);
        }

        public ActionResult Team()
        {
            return View();
        }
    }
}