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
                    var currentIntervalId = GetCurrentInterval();
                    model.Seasons = new SelectList(pool.Seasons, "Id", "Name").ToList();
                    model.SelectedSeasonID = Convert.ToInt32(model.Seasons.Last().Value);
                    model.StandingRows = new List<StandingRow>();
                }

                return View(model);
            }
        }

        public List<StandingRow> GetStandingRows(int poolId, int seasonId)
        {
            using (var context = new ApplicationDbContext())
            {
                var currentIntervalId = GetCurrentInterval();
                var standings = from ss in context.TeamSeasonStanding 
                                join i in context.Intervals on ss.SeasonId equals i.SeasonId
                                from tiat in context.TeamIntervalActiveTotal
                                .Where(t=> t.TeamId == ss.TeamId && t.IntervalId == i.Id)
                                .DefaultIfEmpty()
                                where ss.PoolId == poolId && ss.SeasonId == seasonId && i.Id == currentIntervalId
                                select new StandingRow() { Rank = ss.Rank, Name = ss.Team.Name, Gain = (tiat == null ? 0 : tiat.IntervalTotal), Total = ss.Total };
                return standings.ToList();
            }
        }
        public ActionResult StandingRows(int poolId, int seasonId)
        {
            return Json(GetStandingRows(poolId,seasonId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Team()
        {
            return View();
        }

        public int GetCurrentInterval()
        {
            using (var context = new ApplicationDbContext())
            {
                var today = DateTime.Now.Date;
                var intervals = context.Intervals.Where(i => (i.StartDate <= today) && (today <= i.EndDate));
                var interval = context.Intervals.OrderByDescending(i => i.EndDate).First();
                
                if (intervals.Count() > 0)
                    interval = intervals.First();

                return interval.Id;
            }
        }
    }
}