using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class mavadItemVM
    {
        public string title { get; set; }
        public string mid { get; set; }
        public decimal calories { get; set; }
        public decimal protein { get; set; }
        public decimal carbo { get; set; }
        public decimal fat { get; set; }
        public decimal amount { get; set; }
        public string ingID { get; set; }
    }
}