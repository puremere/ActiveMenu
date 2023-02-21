using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel.api
{
    public class CategoryVM
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public int isVisible { get; set; }
        public string icon { get; set; }
        public string image { get; set; }
        public List<ItemVM> foodList { get; set;}
    }

    public class ItemVM
    {
        public Guid id { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public int price { get; set; }
    }

}