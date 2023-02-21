using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class basketVM
    {
       List<ProductDetailCookie> basketData { get; set; }
    }
    public class ProductDetailCookie
    {
        public string productid { get; set; }
        public int quantity { get; set; }
        public string madatToRemove { get; set; }
        public string description { get; set; }
    }
}