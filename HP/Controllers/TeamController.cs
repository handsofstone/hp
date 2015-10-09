using HP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HP.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Roster(int id)
        {
            var model = new RosterViewModel();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var team = context.Teams.Find(id);
                model.TeamId = id;
                model.RosterPlayers = new SelectList(team.RosterPlayers.Select(p => p.Player).ToList(), "Id", "LexicalName").ToList();
                model.AvailablePlayers = AvailablePlayers(team.Pool_Id);
                model.Intervals = new SelectList(context.IntervalsByPoolSeason(team.Pool_Id, 1), "Id", "Name").ToList();
                model.PlayerIntervals = team.RosterPlayers.Select(p => new PlayerInterval(p)).ToList();
                //model.PlayerIntervals = team.RosterPlayers.Select(p => new PlayerInterval(p)).ToList();

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveRoster(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var teamId = int.Parse(formCollection["teamId"]);
                    var playerIds = formCollection["playerIds"].Split(',').ToList().Select(int.Parse).ToList();

                    var team = context.Teams.Find(teamId);
                    var players = context.Players.Where(p => playerIds.Contains(p.Id)).ToList();
                    //var deletedPlayers = team.RosterPlayers.Except(p=>p.Player)
                    //var deletedPlayers = team.RosterPlayers.Except(players).ToList();
                    //var addedPlayers = players.Except(team.RosterPlayers).ToList();
                    //deletedPlayers.ForEach(p => team.RosterPlayers.Remove(p));
                    //foreach (NHLPlayer p in addedPlayers)
                    //{
                    //    team.RosterPlayers.Add(p);
                    //}
                    context.SaveChanges();

                    return RedirectToAction("Roster", new { id = teamId });
                }
            }
            return View();

        }

        public ActionResult SubmitRoster(FormCollection formCollection)
        {
            var teamId = int.Parse(formCollection["TeamId"]);
            var keys = formCollection["pid"].Split(',').Select(int.Parse);
            var values = formCollection["pi.position"].Split(',');

            Dictionary<int, string> positions = keys.Zip(values, (k, v) => new { Key = k, Value = v })
                     .ToDictionary(x => x.Key, x => x.Value); 
            SavePositions(teamId, positions);
            SaveLineup();
            return RedirectToAction("Roster", new { id = teamId });
        }
        public void SavePositions(int teamId, Dictionary<int,string> positions)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (KeyValuePair<int, string> entry in positions)
                {
                    context.RosterPlayers.Find(teamId, entry.Key).Position = entry.Value;
                }
                context.SaveChanges();
            }
        }

        public void SaveLineup()
        {

        }
        public IList<SelectListItem> AvailablePlayers(int poolId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var players = context.AvailablePlayers(poolId).OrderBy(player => player.FullName);
                return new SelectList(players, "Id", "FullName").ToList();
            }
        }
    }
}