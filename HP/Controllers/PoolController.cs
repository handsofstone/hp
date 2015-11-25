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

        public ActionResult Standings(int id)
        {
            using (var context = new ApplicationDbContext())
            {

                var model = new StandingsViewModel();

                if (ModelState.IsValid)
                {
                    var pool = context.Pools.Find(id);
                    model.Seasons = new SelectList(pool.Seasons, "Id", "Name").ToList();
                    model.SelectedSeasonID = Convert.ToInt32(model.Seasons.First().Value);
                    model.StandingRows = context.TeamSeasonStanding.Where(p => p.PoolId == id && p.SeasonId == model.SelectedSeasonID).Select(
                        s => new StandingRow() {
                            Rank = s.Rank,
                            Name = s.Team.Name,
                            Total = s.Total }).ToList();
                }

                return View(model);
            }
        }

        public ActionResult Team()
        {
            return View();
        }
    }
}