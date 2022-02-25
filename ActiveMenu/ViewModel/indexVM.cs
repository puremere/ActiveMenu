using ActiveMenu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class indexVM
    {
        public List<category> calist { get; set; }
        public string title { get; set; }
        public string image { get; set; }
    }
}