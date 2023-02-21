using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActiveMenu.Model;

namespace ActiveMenu.ViewModel
{
    public class CreateItemVM
    {
        public List<mavadItemVM> itemMavad = new List<mavadItemVM>();
        public List<mavad> lstMavad = new List<mavad>();
        public List<category> catlist = new List<category>();
        public string itemID { get; set; }

        public Item item = new Item();
        public string username { get; set; }
        public string catItem { get; set; } 
    }
}