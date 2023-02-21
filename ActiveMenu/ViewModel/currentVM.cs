using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class currentVM
    {
        public string  itemordertitle { get; set; }
        public int itemorderCount { get; set; }
        public string orderplace { get; set; }
        public string itemImage { get; set; }
        public string orderDetailDesc { get; set; }
        public Guid orderdetailID { get; set; }

    }
}