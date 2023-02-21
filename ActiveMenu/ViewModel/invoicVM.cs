using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActiveMenu.Model;

namespace ActiveMenu.ViewModel
{
    public class invoicVM
    {
        public List<orderDetail> lst { get; set; }
        public int  vat { get; set; }
        public string orderID { get; set; }
    }
}