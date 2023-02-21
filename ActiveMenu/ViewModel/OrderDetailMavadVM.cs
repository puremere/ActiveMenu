using ActiveMenu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class OrderDetailMavadVM
    {
       public List<vmItemMavad> lst { get; set;  }
        public string instruction { get; set; }
    }
}