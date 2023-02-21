using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class orderDetailVM
    {
        public string itemTitle { get; set; }
        public string itemDesc { get; set; }
        public string itemImage { get; set; }
        public int itemCount { get; set; }
        public double itemprice  { get; set; }
    }
    public class orderDetailPageVM
    {
        public List<orderDetailVM> list { get; set; }
        public string  name { get; set; }
    }


}