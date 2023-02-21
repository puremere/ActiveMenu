using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.ViewModel
{
    public class settingVM
    {
        public string brandName { get; set; }
        public string mobileNumber { get; set; }
        public string phoneNumber { get; set; }
        public string whatsappNumber { get; set; }
        public int oppening { get; set; }
        public int closing { get; set; }

        public DateTime oppeningD { get; set; }
        public DateTime closingD { get; set; }


        public string email { get; set; }
        public int vat { get; set; }
        public string address { get; set; }
        public HttpPostedFileBase imagefile { get; set; }

    }
}