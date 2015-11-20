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
                    // model.Seasons = context.Pools.Where(p => p.Id == PoolId).FirstOrDefault<Pool>().Seasons.ToList<Season>();
                    var pool = context.Pools.Find(id);
                    //var season = pool.Seasons;

                    model.Seasons = new SelectList(pool.Seasons, "Id", "Name").ToList();
                    model.SelectedSeasonID = Convert.ToInt32(model.Seasons.First().Value);
                    //model.Standings = pool.Teams.Select(p => new Standing(p.Standings.Where(s => s.SeasonId == model.SelectedSeasonID).First())).ToList();
                    //var standings = pool.Standings.Where(s => s.SeasonId == model.SelectedSeasonID);
                    //model.StandingRows = standings.Select(s => new StandingRow(s)).ToList();
                        //new List<StandingRows>(pool.Standings.Where(s => s.SeasonId == model.SelectedSeasonID).ToList());

                    //model.Standings = context.TeamSeasonStanding.Include("Team").Where(p => p.Team.Pool_Id == id && p.SeasonId == model.SelectedSeasonID).Select(s => new Standing(s)).ToList();
                    model.StandingRows = context.TeamSeasonStanding.Include("Team").Where(p => p.Team.Pool_Id == id && p.SeasonId == model.SelectedSeasonID).Select(
                        s => new StandingRow() {
                            Name = s.Team.Name,
                            Total = s.Total }).ToList();
                    //model.StandingRows = context
                    //model.Standings = context.Standings.Include("Team").Where(p => p.PoolId == id).ToList();
                    //var user = UserManager.FindById(User.Identity.GetUserId());
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