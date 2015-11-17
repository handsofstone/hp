using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HP.Models
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            Pools = new List<SelectListItem>();
            Teams = new List<Team>();
        }

        public MenuViewModel(User user)
        {
            Pools = new SelectList(user.GetPools(),"Id","Name").ToList();
            Teams = new List<Team>();
            //foreach (var pool in user.GetPools())
            //    Pools.Add(new SelectListItem() { Text = pool.Name, Value = pool.Id.ToString() });
            if (Pools.Count != 0)
            {
                SelectedPoolId = Int32.Parse(Pools.First().Value);
                Teams = new ApplicationDbContext().TeamsByPoolID(SelectedPoolId);
            }
        }

        public bool ShowPool { get { return Pools != null && Pools.Count > 0; } }

        [Display(Name = "Pool")]
        public int SelectedPoolId { get; set; }
        public IList<SelectListItem> Pools { get; set; }

        [Display(Name = "Team")]
        public Team SelectedTeam { get; set; }
        public IList<Team> Teams { get; set; }

    }
}