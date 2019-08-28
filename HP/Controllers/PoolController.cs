using HP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        public ActionResult Index(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var model = new StandingsViewModel();

                if (ModelState.IsValid)
                {
                    var pool = context.Pools.Find(id);
                    var currentSeason = GetCurrentSeason();
                    model.SelectedPoolID = id;
                    model.Seasons = new SelectList(context.Seasons, "Id", "Name").ToList();
                    model.SelectedSeasonID = currentSeason.Id;
                    model.StandingRows = new List<StandingRow>();
                }

                return View(model);
            }
        }

        public ActionResult Standings(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var model = new StandingsViewModel();

                if (ModelState.IsValid)
                {
                    var pool = context.Pools.Find(id);
                    var currentSeason = GetCurrentSeason();
                    model.Seasons = new SelectList(context.Seasons, "Id", "Name").ToList();
                    model.SelectedSeasonID = currentSeason.Id;
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
                                join tt in context.TeamIntervalActiveTotal.Where(t => t.IntervalId == currentIntervalId) on new { ss.SeasonId, ss.TeamId } equals new { tt.SeasonId, tt.TeamId } into tiat
                                from tt in tiat.DefaultIfEmpty()
                                where ss.PoolId == poolId && ss.SeasonId == seasonId
                                select new StandingRow() { Rank = ss.Rank, Name = ss.Team.Name, Gain = (tt == null ? 0 : tt.IntervalTotal), Total = ss.Total, TeamId = ss.TeamId };
                return standings.OrderBy(s => s.Rank).ToList();
            }
        }
        public ActionResult StandingRows(int poolId, int seasonId)
        {
            return Json(GetStandingRows(poolId, seasonId), JsonRequestBehavior.AllowGet);
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

        public Season GetCurrentSeason()
        {
            using (var context = new ApplicationDbContext())
            {
                var today = DateTime.Now.Date;
                var intervals = context.Intervals.Where(i => (i.StartDate <= today) && (today <= i.EndDate));
                var interval = context.Intervals.OrderByDescending(i => i.EndDate).First();

                if (intervals.Count() > 0)
                    interval = intervals.First();

                return interval.Season;
            }
        }
        public ContentResult Assets(int poolId)
        {
            using (var context = new ApplicationDbContext())
            {
                var assets = context.Assets(null, poolId);
                return Content(assets, "application/json");
            }
        }
        public ContentResult DraftDashboard(int poolId, int seasonId)
        {
            using (var context = new ApplicationDbContext())
            {
                var picks = context.DraftDashboard(poolId, seasonId);
                var dashboard = JObject.Parse(picks);
                dashboard.Add("isAdmin", User.IsInRole("admin"));
                
                var user = UserManager.FindById(User.Identity.GetUserId());
                var teams = from t in user.Teams
                            where t.Team.PoolId == poolId
                            select t.TeamId;
                dashboard.Add("teamIds", new JArray(teams));
                var serializer = new JsonSerializer {ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var json = JObject.FromObject(dashboard, serializer).ToString();
                return Content(json, "application/json");
            }
        }

        public ContentResult AvailablePlayer(string searchString, int poolId)
        {
            string searchResults = NHL.NHL.getAvailablePlayers(searchString);
            using (var context = new ApplicationDbContext())
            {
                var result = context.AvailablePlayers(searchResults, poolId);
                return Content(result, "application/json");
            }
        }
        [HttpPost]
        public ActionResult DraftPlayer(int pickId, string player)
        {
            using (var context = new ApplicationDbContext())
            {
                if (User.IsInRole("admin"))
                {
                    var result = context.DraftPlayer(pickId, player);

                    return Json(result == 0);
                }
                return new HttpStatusCodeResult(401, "Unauthorised user.");
            }
        }

        public ActionResult UpdateOrder(string orders)
        {
            using (var context = new ApplicationDbContext())
            {
                if (User.IsInRole("admin"))
                {
                    var result = context.UpdateOrder(orders);

                    return Json(result == 0);
                }
                return new HttpStatusCodeResult(401, "Unauthorised user.");
            }
        }
        public ActionResult ResetOrder(int poolId, int seasonId)
        {
            using (var context = new ApplicationDbContext())
            {
                if (User.IsInRole("admin"))
                {
                    var result = context.ResetOrder(poolId,seasonId);

                    return Json(result == 0);
                }
                return new HttpStatusCodeResult(401, "Unauthorised user.");
            }
        }
        public ActionResult AddRound(int poolId, int seasonId)
        {
            using (var context = new ApplicationDbContext())
            {
                if (User.IsInRole("admin"))
                {
                    var result = context.AddRound(poolId, seasonId);

                    return Json(result == 0);
                }
                return new HttpStatusCodeResult(401, "Unauthorised user.");
            }
        }
        public ActionResult DeleteRound(int poolId, int seasonId)
        {
            using (var context = new ApplicationDbContext())
            {
                if (User.IsInRole("admin"))
                {
                    var result = context.DeleteRound(poolId, seasonId);

                    return Json(result == 0);
                }
                return new HttpStatusCodeResult(401, "Unauthorised user.");
            }
        }
    }
}