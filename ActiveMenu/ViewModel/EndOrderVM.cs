using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class EndOrderVM
    {
        public double finalPrice { get; set; }
        public double orderPrice { get; set; }
        public double vat { get; set; }
        public double discount { get; set; }
        public List<Model.Item> lst { get; set; }
    }
}