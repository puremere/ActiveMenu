using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveMenu.Model;
using ActiveMenu.ViewModel;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace ActiveMenu.Controllers
{

    public class HomeController : Controller
    {
        Context dbcontext = new Context();
        public ActionResult Index()
        {
            List<category> lst = dbcontext.categories.ToList();
            Guid id = new Guid("1c0b9a5d-f5ec-4224-a1ac-a67a0144027e");
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);
           

            indexVM model = new indexVM()
            {
                 calist = lst,
                  image = rst.imageAddress,
                   title = rst.title 
            };
            return View(model);
        }

        public ActionResult Detail(Guid id)
        {
            ViewBag.Message = "Your application description page.";
            category cat = dbcontext.categories.SingleOrDefault(x => x.categoryID == id);

            List<Item> itemList = dbcontext.items.Where(x => x.categoryID == id).ToList();

           
            detailVM model = new detailVM()
            {
                  name= cat.title,
                  itemList  = itemList
            };
            return View(model);
        }
        public ActionResult ItemDetail(string id)
        {



            Guid itemID = new Guid(id);
            Item model = dbcontext.items.SingleOrDefault(x => x.itemID == itemID);
            model.ingredient = "آب میوه طبیعی، طعم دهنده، نعنا، رنگ خوراکی";
            dbcontext.SaveChanges();

            return View(model);
        }

        public string addtocart(string id)
        {



            string str = id;

            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");

            List<ProductDetailCookie> data;
            if (cartModelString == "")
            {
                ProductDetailCookie newitem = new ViewModel.ProductDetailCookie();
                List<ProductDetailCookie> data2 = new List<ViewModel.ProductDetailCookie>();
                newitem.productid =str;
                newitem.quantity = 1;
                data2.Add(newitem);
                var json = new JavaScriptSerializer().Serialize(data2);
                Response.Cookies["Modelcart"].Value = json;
                return "1";
            }
            else
            {
                data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
                ProductDetailCookie newitem = new ViewModel.ProductDetailCookie();

                int j = 0;
                foreach (var item in data)
                {
                    if (item.productid ==str)
                    {
                        item.quantity += 1;
                        j = 1;

                    }

                }

                if (j == 0)
                {
                    newitem.productid = str;
                    newitem.quantity = 1;
                    data.Add(newitem);
                    var json = new JavaScriptSerializer().Serialize(data);
                    Response.Cookies["Modelcart"].Value = json;
                    return "1";
                }
                else
                {
                    var json = new JavaScriptSerializer().Serialize(data);
                    Response.Cookies["Modelcart"].Value = json;
                    return "10";
                }

            }




        }
        public void minusfromcart(string id)
        {

            string CookieModel = Request.Cookies["Modelcart"].Value;
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(CookieModel);
            string str = id;
            if (data != null && data.Count > 0)
            {

                ProductDetailCookie newitem = new ViewModel.ProductDetailCookie();

                bool boolian = false;
                int lastindex = 0;
                foreach (var item in data)
                {
                    lastindex = data.IndexOf(item);
                    if (item.productid == str)
                    {
                        if (item.quantity > 0)
                        {
                            item.quantity -= 1;
                        }


                    }

                    if (item.quantity == 0)
                    {
                        boolian = true;
                    }
                }

                if (boolian)
                {
                    data.Remove(data[lastindex]);
                }

                var json = new JavaScriptSerializer().Serialize(data);
                Response.Cookies["Modelcart"].Value = json;
                // SetCookie(json, "cartModel");


            }

        }
        public ActionResult getquntitiy()
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            //List<ProductDetailCookie> model = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
            //int final = model != null ?  model.Sum(x => x.quantity) : 0;
            return Content(cartModelString);
        }
        public ActionResult basket()
        {
            
            List<checkOutVM> finalmodel = new List<checkOutVM>();

            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            ViewBag.cookie = cartModelString;
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
            List<Item> lst = new List<Item>();
            foreach (var item in data)
            {
                Guid itemID = new Guid(item.productid);

                Item myItem = dbcontext.items.SingleOrDefault(x => x.itemID == itemID);
                lst.Add(myItem);
            };

            return View(lst);
           
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}