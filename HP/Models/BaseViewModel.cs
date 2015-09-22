using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HP.Models
{
    public class BaseViewModel
    {
        public LoginViewModel Login { get; set; }
        public MenuViewModel Menu { get; set; }
    }
}