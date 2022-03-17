using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class EndOrderVM
    {
        public long finalPrice { get; set; }
        public long orderPrice { get; set; }
        
        public long discount { get; set; }
        public List<Model.Item> lst { get; set; }
    }
}