using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel.api
{
    public class getMainVM
    {
        public string gu { get; set; }
        public string tb { get; set; }
      
    }

    public class setOrderVM : getMainVM
    {
        public string basketmodel { get; set; }
    }
}