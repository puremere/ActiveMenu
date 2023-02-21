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
        public string  id { get; set; }
        public List<Item> itemList { get; set; }
        public List<category> catList { get; set; }
    }
}