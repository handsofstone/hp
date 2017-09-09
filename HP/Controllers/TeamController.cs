﻿using HP.Helpers;
using HP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HP.Controllers
{
    public class TeamController : Controller
    {
        const string suggestURL = "https://suggest.svc.nhl.com/svc/suggest/v1/minplayers/{0}/99999";
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

        // GET: Team
        public ActionResult Index(int id)
        {
            var model = new TeamViewModel();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var team = context.Teams.Find(id);
                model.TeamId = id;
                model.RosterPlayersToAdd = new SelectList(team.RosterPlayers.Select(p => p.Player).ToList(), "Id", "LexicalName").ToList();
                //model.RosterPlayers = team.RosterPlayers.Select(p => new PlayerInterval(p)).OrderBy(p => p, new PlayerIntervalComparer()).ToList();
                model.AvailablePlayers = AvailablePlayers(team.PoolId);
                model.Intervals = new SelectList(context.IntervalsByPoolSeason(team.PoolId, 2), "Id", "Name").ToList();
                //model.PlayerIntervals = GetPlayerIntervals(id, context.IntervalsByPoolSeason(team.PoolId, 1).First().Id).ToList();
                model.SelectedIntervalId = GetCurrentInterval();
                model.CanSubmit = GetCanSubmit(model.TeamId, model.SelectedIntervalId);
                model.CanTrade = GetCanTrade(model.TeamId);
                model.SelectedStartTime = GetIntervalStartTime(model.SelectedIntervalId).ToString();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddPlayers(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var teamId = int.Parse(formCollection["teamId"]);
                    var playerIds = formCollection["playerIds"].Split(',').ToList().Select(int.Parse).ToList();
                    var team = context.Teams.Find(teamId);
                    var currPlayerIds = team.RosterPlayers.Select(p => p.PlayerId).ToList();
                    var players = context.RosterPlayers.Where(p => (p.TeamId == teamId) && (playerIds.Contains(p.PlayerId)));
                    var deletedPlayers = team.RosterPlayers.Except(players).ToList();
                    var addedPlayerIds = playerIds.Except(currPlayerIds);
                    context.RosterPlayers.RemoveRange(deletedPlayers);
                    foreach (int playerId in addedPlayerIds)
                    {
                        context.RosterPlayers.Add(new RosterPlayer() { TeamId = teamId, PlayerId = playerId });
                    }
                    context.SaveChanges();

                    return RedirectToAction("Index", new { id = teamId });
                }
            }
            return View();
        }

        public ActionResult SaveRoster(SaveRosterViewModel model)
        {
            SavePositions(model);
            return Json(true);
        }
        public void SavePositions(SaveRosterViewModel model)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (var player in model.Players)
                {
                    context.RosterPlayers.Find(model.TeamId, player.PlayerId).Position = player.Position;
                }
                context.SaveChanges();
            }
        }

        public void SaveLineup(LineupModel model)
        {
            using (var context = new ApplicationDbContext())
            {
                foreach (var player in model.Players)
                {
                    if (player.LineupPlayerId == null)
                        context.LineupPlayers.Add(new LineupPlayer()
                        {
                            TeamId = model.TeamId,
                            IntervalId = model.IntervalId,
                            PlayerId = player.PlayerId,
                            Active = player.Active,
                            Position = player.Position
                        });
                    else
                    {
                        var lineupPlayer = context.LineupPlayers.Find(player.LineupPlayerId);
                        lineupPlayer.Active = player.Active;
                    }
                }
                context.SaveChanges();
            }
        }

        public ActionResult SubmitLineup(LineupModel model)
        {
            SaveLineup(model);
            return Lineup(model.TeamId, model.IntervalId);
        }

        public int DeleteLineup(int teamId, int intervalId)
        {
            using (var context = new ApplicationDbContext())
            {
                var records = context.LineupPlayers.Where(p => p.IntervalId == intervalId && p.TeamId == teamId).ToList();

                context.LineupPlayers.RemoveRange(records);
                return context.SaveChanges();
            }
        }

        public ContentResult ResetLineup(int teamId, int intervalId)
        {
            DeleteLineup(teamId, intervalId);
            return Lineup(teamId, intervalId);
        }

        public IList<SelectListItem> AvailablePlayers(int poolId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var players = context.AvailablePlayers(poolId).OrderBy(player => player.LexicalName);
                return new SelectList(players, "Id", "LexicalName").ToList();
            }
        }

        public IEnumerable<PlayerInterval> GetPlayerIntervals(int teamId, int intervalId)
        {
            using (var context = new ApplicationDbContext())
            {
                var result = context.LineupPlayers.Where(p => (p.TeamId == teamId) && (p.IntervalId == intervalId)).ToList().Select(p => new PlayerInterval(p));

                if (!result.Any())
                {
                    var endDate = (from i in context.Intervals
                                   where i.Id == intervalId
                                   select i.EndDate).Single();
                    var lastIntervalId = (from lp in context.LineupPlayers
                                          join i in context.Intervals on lp.IntervalId equals i.Id
                                          where lp.TeamId == teamId && i.EndDate < endDate
                                          orderby i.EndDate descending
                                          select i.Id).Take(1).Single();
                    var lineup = from lp in context.LineupPlayers
                                 join rp in context.RosterPlayers on new { lp.TeamId, lp.PlayerId } equals new { rp.TeamId, rp.PlayerId }
                                 where lp.TeamId == teamId && lp.IntervalId == lastIntervalId
                                 select lp;
                    result = lineup.ToList().Select(p => new PlayerInterval(p, intervalId));

                    var result2 = lineup.ToList().Select(p => new LineupRow(p, intervalId));
                }
                return result.OrderBy(p => p, new PlayerIntervalComparer()).ToList();
            }
        }

        public IEnumerable<LineupRow> GetLineupRows(int teamId, int intervalId)
        {
            using (var context = new ApplicationDbContext())
            {
                var result = context.LineupPlayers.Where(p => (p.TeamId == teamId) && (p.IntervalId == intervalId)).ToList().Select(p => new LineupRow(p));

                if (!result.Any())
                {
                    var endDate = (from i in context.Intervals
                                   where i.Id == intervalId
                                   select i.EndDate).Single();
                    var lastIntervalId = (from lp in context.LineupPlayers
                                          join i in context.Intervals on lp.IntervalId equals i.Id
                                          where lp.TeamId == teamId && i.EndDate < endDate
                                          orderby i.EndDate descending
                                          select i.Id).Take(1).Single();
                    var lineup = from lp in context.LineupPlayers
                                 join rp in context.RosterPlayers on new { lp.TeamId, lp.PlayerId } equals new { rp.TeamId, rp.PlayerId }
                                 where lp.TeamId == teamId && lp.IntervalId == lastIntervalId
                                 select lp;

                    result = lineup.ToList().Select(p => new LineupRow(p, intervalId));
                }

                return result.OrderBy(p => p, new LineupRowComparer()).ToList();
            }
        }
        public JsonResult GetIntervalRoster(int teamId, int intervalId)
        {
            var playerIntervals = GetPlayerIntervals(teamId, intervalId);

            return Json(playerIntervals, JsonRequestBehavior.AllowGet);
        }

        public ContentResult Lineup(int teamId, int intervalId)
        {
            using (var context = new ApplicationDbContext())
            {
                var userId = User.Identity.GetUserId();
                var lineup = context.LineupDashboard(userId, teamId, intervalId);
                return Content(lineup, "application/json");//JsonRequestBehavior.AllowGet);
            }
        }

        public int GetCurrentInterval()
        {
            using (var context = new ApplicationDbContext())
            {
                var today = DateTime.Now;

                var current_intervals = context.Intervals.Where(i => (i.StartDate <= today) && (today <= i.EndDate));
                var future_intervals = context.Intervals.Where(i => (today <= i.StartDate)).OrderBy(i => i.StartDate);
                var interval = context.Intervals.OrderByDescending(i => i.EndDate).First(); //latest interval

                if (current_intervals.Count() > 0)
                    interval = current_intervals.First();
                else if (future_intervals.Count() > 0)
                    interval = future_intervals.First();

                return interval.Id;
            }
        }

        public bool GetCanTrade(int teamId)
        {
            var isOwner = false;
            if (User.IsInRole("admin"))
                return true;
            if (User.Identity.GetUserId() != null)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                isOwner = (user.Teams.Where(u => u.TeamId == teamId).Count() > 0);
            }
            return isOwner;
        }

        public bool GetCanSave(int teamId, int intervalId)
        {
            var today = DateTime.Now;
            var isOwner = false;

            if (User.Identity.GetUserId() != null)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                isOwner = (user.Teams.Where(u => u.TeamId == teamId).Count() > 0);
            }

            using (var context = new ApplicationDbContext())
            {
                var query = from g in context.Games
                            from i in context.Intervals
                            where i.Id == intervalId && DbFunctions.TruncateTime(g.StartTime) >= i.StartDate && DbFunctions.TruncateTime(g.StartTime) <= i.EndDate
                            orderby g.StartTime
                            select g;
                return isOwner && today < query.First().StartTime;
            }
        }

        public bool GetCanSubmit(int teamId, int intervalId)
        {
            var isOwner = false;
            if (User.IsInRole("admin"))
                return true;
            if (User.Identity.GetUserId() != null)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                isOwner = (user.Teams.Where(u => u.TeamId == teamId).Count() > 0);
            }

            var today = DateTime.Now;
            using (var context = new ApplicationDbContext())
            {
                var query = from g in context.Games
                            from i in context.Intervals
                            where i.Id == intervalId && DbFunctions.TruncateTime(g.StartTime) >= i.StartDate && DbFunctions.TruncateTime(g.StartTime) <= i.EndDate
                            orderby g.StartTime
                            select g;
                return isOwner && today < GetIntervalStartTime(intervalId);
            }
        }
        public JsonResult EnableSubmit(int teamId, int intervalId)
        {
            return Json(GetCanSubmit(teamId, intervalId), JsonRequestBehavior.AllowGet);
        }

        public DateTime? GetIntervalStartTime(int intervalId)
        {
            using (var context = new ApplicationDbContext())
            {
                var query = from g in context.Games
                            from i in context.Intervals
                            where i.Id == intervalId && DbFunctions.TruncateTime(g.StartTime) >= i.StartDate && DbFunctions.TruncateTime(g.StartTime) <= i.EndDate
                            orderby g.StartTime
                            select g;
                return query.First().StartTime;
            }
        }

        public JsonResult IntervalStartTime(int intervalId)
        {
            return Json(GetIntervalStartTime(intervalId).ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Analysis(int teamId, int intervalId)
        {
            var playerIntervals = GetPlayerIntervals(teamId, intervalId);

            return PartialView("_Analysis", playerIntervals);
        }

        public ContentResult RosterDashboard(int teamId)
        {
            using (var context = new ApplicationDbContext())
            {
                //var userId = User.Identity.GetUserId();
                var dashboard = context.RosterDashboard(teamId);
                return Content(dashboard, "application/json");//JsonRequestBehavior.AllowGet);
            }
        }

        public ContentResult TradeDashboard(int teamId)
        {
            using (var context = new ApplicationDbContext())
            {
                //var userId = User.Identity.GetUserId();
                var dashboard = context.TradeDashboard(teamId);
                return Content(dashboard, "application/json");//JsonRequestBehavior.AllowGet);
            }
        }
        public ContentResult Assets(int teamId)
        {
            using (var context = new ApplicationDbContext())
            {
                var assets = context.Assets(teamId);
                return Content(assets, "application/json");
            }
        }
        [HttpPost]
        public ActionResult SendOffer(int teamId, String json)
        {
            using (var context = new ApplicationDbContext())
            {
                if (GetCanTrade(teamId))
                {
                    var result = context.CreateOffer(json);

                    return Json(result == 0);
                }
                return new HttpStatusCodeResult(401, "Unauthorised user.");
            }
        }
        [HttpPost]
        public ActionResult UpdateOffer(int teamId, int tradeId, Boolean accept)
        {
            using (var context = new ApplicationDbContext())
            {
                if (GetCanTrade(teamId))
                { 
                        var result = context.UpdateOffer(teamId, tradeId, accept);

                    return Json(result == 0);
                }
                return new HttpStatusCodeResult(401, "Unauthorised user.");
            }
        }

        public ContentResult AvailablePlayer(string searchString, int teamId)
        {
            string html = String.Format(suggestURL, searchString);
            string searchResults;
            HTMLHelper.Origin = "www.nhl.com";
            StreamReader reader = new StreamReader(HTMLHelper.GetResponseStream(html));
            searchResults = reader.ReadToEnd();
            using (var context = new ApplicationDbContext())
            {
                var team = context.Teams.Find(teamId);
                var result = context.AvailablePlayers(searchResults, team.PoolId);
                return Content(result, "application/json");
            }
        }
    }
}