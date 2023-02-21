using ActiveMenu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class itemDetailVM
    {
        public Item item { get; set; }
        public List<mavadItemVM> mavadList {get; set;}
        public List<Item> itemlist { get; set; }
    }
}