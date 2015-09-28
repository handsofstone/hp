using HP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HP.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationUserManager _userManager;
        public MenuController() { }

        public MenuController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

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

        // GET: Menu
        public PartialViewResult Index()
        {
            MenuViewModel model = null;

            if (ModelState.IsValid)
            {
                if (User.Identity.GetUserId() != null)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    model = new MenuViewModel(user);
                }
            }

            return PartialView("_Menu", model ?? new MenuViewModel());
        }

        [HttpPost]
        public async Task<PartialViewResult> Index(MenuViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            return PartialView(model);
        }

        public JsonResult GetTeams(string id)
        {            
            using (var context = new ApplicationDbContext())
            {
                var teams = context.TeamsByPoolID(Int32.Parse(id));

                return Json(new SelectList(teams, "Id", "Name"), JsonRequestBehavior.AllowGet);
            }
        }

    }
}