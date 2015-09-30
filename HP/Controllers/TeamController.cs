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
            return View();
        }
    }
}