using ActiveMenu.Model;
using ActiveMenu.ViewModel.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class appindexVM
    {
        public List<CategoryVM> data { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string garson { get; set; }
    }
    public class indexVM
    {
        public List<category> calist { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string garson { get; set; }
    }
}