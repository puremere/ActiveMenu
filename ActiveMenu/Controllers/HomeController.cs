using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveMenu.Model;
using ActiveMenu.ViewModel;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using ActiveMenu.Classes;

namespace ActiveMenu.Controllers
{

    public class HomeController : Controller
    {
        Context dbcontext = new Context();
        public ActionResult Index(string gu, string tb)
        {
            if (Response.Cookies["us"] == null)
            {
                if (Response.Cookies["us"].Value == "")
                {
                    Response.Cookies["us"].Value = CustomMethods.RandomString(25); 
                }
            }
            
            List<category> lst = dbcontext.categories.ToList();
           
           
            
            if (gu == null)
            {
                gu = "1c0b9a5d-f5ec-4224-a1ac-a67a0144027e";
            }
            Guid id = new Guid(gu);
            Response.Cookies["gu"].Value = gu;
            Response.Cookies["tb"].Value = tb;
            
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);
        

            Response.Cookies["logo"].Value = rst.logo;

            //rst.mobile = "09194594505";
            //rst.phone = "02188445566";
            //rst.whatsapp = "989124594505";
            //rst.instagram = "https://www.instagram.com/cafeniki";
            //rst.totem = "لحظه های شما  برای ما ارزشمند است";
            //rst.description = "ما در کافه رستوران نیکی با ایجاد فضای دنج و دوست داشتنی و منوی کامل با تنوع بی نظیر تلاش کردیم تا سهم کوچکی در لحظات شاد شما داشته باشیم";

            //dbcontext.SaveChanges();
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
                name = cat.title,
                itemList = itemList
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
                newitem.productid = str;
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
                    if (item.productid == str)
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
                   // data.Remove(data[lastindex]);
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

            long price = 0;
            long discount = 0;
            foreach (var item in data)
            {
                if (item.quantity > 0)
                {
                    Guid itemID = new Guid(item.productid);

                    Item myItem = dbcontext.items.SingleOrDefault(x => x.itemID == itemID);
                    price += myItem.price * item.quantity;
                    lst.Add(myItem);
                }
                
            };
            EndOrderVM model = new EndOrderVM()
            {
                lst = lst,
                discount = discount,
                orderPrice = price,
                finalPrice = price - discount
            };
            return View(model);

        }
        public ActionResult setOrder()
        {
            string restaurantID = Request.Cookies["gu"].Value as string;
            Guid resid = new Guid(restaurantID);
            restaurant restaurantItem = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == resid);
            string us = Request.Cookies["us"].Value as string;
            string tableID = Request.Cookies["tb"].Value as string;
            Guid orderID = Guid.NewGuid();
            string peigiry = CustomMethods.RandomString(12);
            string lastOrder = "";
            if (Request.Cookies["order"] != null)
            {
                lastOrder = Request.Cookies["order"].Value as string;
            }
            lastOrder += peigiry + ",";
            Response.Cookies["order"].Value = lastOrder;
            Response.Cookies["order"].Expires.AddDays(1);
            dbcontext.orders.Add(new order
            {
                restaurantID = new Guid(restaurantID),
                orderID = orderID,
                orderNumber = restaurantItem.orderNumberBegin + 1,
                us = us,
                isRefine = 0,
                minutToAdd = 10,
                peigiry = peigiry,
                orderTime = DateTime.Now,
            });
            restaurantItem.orderNumberBegin += 1;
            
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);

            foreach (var item in data)
            {
                dbcontext.orderDetails.Add(new orderDetail()
                {
                    count = item.quantity,
                    itemID = new Guid(item.productid),
                    orderID = orderID,
                    orderDetailID = Guid.NewGuid(),
                });
            }

            dbcontext.SaveChanges();
            string liveOrder = orderID.ToString();
            Response.Cookies["liveOrder"].Value = liveOrder;

            Response.Cookies["Modelcart"].Value = "";
            return RedirectToAction("info");
        }

        public ActionResult liveOrder(string id)
        {
            return View();
        }

        public ActionResult info()
        {
            
            string gu = Request.Cookies["gu"].Value as string;
            Guid rID = new Guid(gu);
            restaurant rs = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == rID);
            infoVM model = new infoVM()
            {
                restaurant = rs
            };
            return View(model);
        }

        public ActionResult getUserOrder()
        {
            string orderPeigiry = "";
            List<order> lst = new List<order>();
            if (Request.Cookies["order"] != null)
            {
                 orderPeigiry = Request.Cookies["order"].Value as string;
                foreach(var peigir in orderPeigiry.Trim(',').Split(',').ToList())
                {
                    Model.order myorder = dbcontext.orders.SingleOrDefault(x => x.peigiry == peigir);
                    //myorder.minutToAdd = 2;
                    //dbcontext.SaveChanges();
                    if (myorder != null)
                    {
                        TimeSpan timspan = DateTime.Now.Subtract(myorder.orderTime);
                        myorder.minutPassed = timspan.TotalMinutes;
                        lst.Add(myorder);
                    }
                       
                }
              
            }
            
            
            return PartialView("~/Views/Shared/_orderStatus.cshtml", lst);
        }
    }
}