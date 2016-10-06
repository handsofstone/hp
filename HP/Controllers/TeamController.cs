﻿using HP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HP.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult Index(int id)
        {
            var model = new TeamViewModel();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var team = context.Teams.Find(id);
                model.TeamId = id;
                model.RosterPlayersToAdd = new SelectList(team.RosterPlayers.Select(p => p.Player).ToList(), "Id", "LexicalName").ToList();
                model.RosterPlayers = team.RosterPlayers.Select(p => new PlayerInterval(p)).OrderBy(p => p, new PlayerIntervalComparer()).ToList();
                model.AvailablePlayers = AvailablePlayers(team.PoolId);
                model.Intervals = new SelectList(context.IntervalsByPoolSeason(team.PoolId, 2), "Id", "Name").ToList();
                //model.PlayerIntervals = GetPlayerIntervals(id, context.IntervalsByPoolSeason(team.PoolId, 1).First().Id).ToList();
                model.SelectedIntervalId = GetCurrentInterval();
                model.CanSubmit = GetCanSubmit(model.SelectedIntervalId);
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
            return Json(true, JsonRequestBehavior.AllowGet);
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
                if (result.Count() == 0)
                {
                    var team = context.Teams.Find(teamId);
                    result = team.RosterPlayers.Select(p => new PlayerInterval(p, intervalId));
                }
                return result.OrderBy(p => p, new PlayerIntervalComparer()).ToList();
            }
        }

        public JsonResult GetIntervalRoster(int teamId, int intervalId)
        {
            var playerIntervals = GetPlayerIntervals(teamId, intervalId);

            return Json(playerIntervals, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Lineup(int teamId, int intervalId)
        {
            var playerIntervals = GetPlayerIntervals(teamId, intervalId);

            return PartialView("_Lineup", playerIntervals);
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

        public bool GetCanSave()
        {
            var today = DateTime.Now;

            return false;
        }

        public bool GetCanSubmit(int intervalId)
        {
            var today = DateTime.Now;
            using (var context = new ApplicationDbContext())
            {                
                var query = from g in context.Games
                            from i in context.Intervals
                            where i.Id == intervalId && DbFunctions.TruncateTime(g.StartTime) >= i.StartDate && DbFunctions.TruncateTime(g.StartTime) <= i.EndDate
                            orderby g.StartTime
                            select g;
                return today < query.First().StartTime;
            }
        }
        public JsonResult EnableSubmit(int intervalId)
        {            
            return Json(GetCanSubmit(intervalId), JsonRequestBehavior.AllowGet);            
        }

        public ActionResult Analysis(int teamId, int intervalId)
        {
            var playerIntervals = GetPlayerIntervals(teamId, intervalId);

            return PartialView("_Analysis", playerIntervals);
        }
    }
}