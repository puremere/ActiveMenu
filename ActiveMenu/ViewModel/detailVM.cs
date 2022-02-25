using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActiveMenu.Model;

namespace ActiveMenu.ViewModel
{
    public class detailVM
    {
        public string  name { get; set; }
        public List<Item> itemList { get; set; }
    }
}