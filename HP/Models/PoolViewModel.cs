using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HP.Models
{
    public class Standing
    {
        public String Name { get; set; }
        public String Gain { get; set; }
        public String Total { get; set; }
    }

    public class StandingsViewModel
    {
        public int SelectedPoolID { get; set; }
        [Display(Name="Season")]
        public int SelectedSeasonId { get; set; }
        public IList<SelectListItem> Seasons { get; set; }
        public List<Standing> Standings { get; set; }
    }
}