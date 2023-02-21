using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel.api
{
    public class foodDetailVM
    {
        public List<suggestedItemsList> suggestedItemsList { get; set; }
        public itemDetail itemDetail { get; set; }
    }

    public class suggestedItemsList
    {
        public string itemID { get; set; }
        public string imageList { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public double price { get; set; }
       
        public string servingTime { get; set; }
        public string prepTime { get; set; }
        public string cookTime { get; set; }
    }
    public class itemDetail
    {
        public string itemID { get; set; }
        public string imageList { get; set; }
        public string title { get; set; }
        public decimal calory { get; set; }
        public string description { get; set; }
        public string ingredient { get; set; }
        public double price { get; set; }
        public string servingTime { get; set; }
        public string prepTime { get; set; }
        public string cookTime { get; set; }
        public string arModel { get; set; }
        public string categoryID { get; set; }
        public string orderDetails { get; set; }
    }
}