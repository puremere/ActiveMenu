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
using System.Configuration;
using ClosedXML.Excel;
using System.Data;
using System.IO;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html;
using Rectangle = iTextSharp.text.Rectangle;

namespace ActiveMenu.Controllers
{

    public class HomeController : Controller
    {
        Context dbcontext = new Context();
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public void addDiez()
        {
            addItem obj = new addItem();
            obj.addDiez();
        }
        public void addCat()
        {
            addItem obj = new addItem();
            //obj.addCat();
        }

        public ActionResult main()
        {

            //user myuser = new user()
            //{
            //    userID = Guid.NewGuid(),
            //    password = "password",
            //    username = "menomotive",
            //    token = CustomMethods.RandomString(16)
            //};
            //dbcontext.users.Add(myuser);

            //role role = new role()
            //{
            //    roleID = Guid.NewGuid(),
            //    title = "admin",
            //};
            //dbcontext.roles.Add(role);

            //roleAccess myroleaccess = new roleAccess()
            //{
            //    roleID = role.roleID,
            //    action = "/Management/dashbaord",
            //};
            //dbcontext.roleAccesses.Add(myroleaccess);
            //roleAccess myroleaccess2 = new roleAccess()
            //{
            //    roleID = role.roleID,
            //    action = "/Management/ingredients",
            //};
            //dbcontext.roleAccesses.Add(myroleaccess);
            //roleAccess myroleaccess3 = new roleAccess()
            //{
            //    roleID = role.roleID,
            //    action = "/Management/tables",
            //};
            //dbcontext.roleAccesses.Add(myroleaccess);

            //userRole myuserrole = new userRole() {
            //    userROleID = Guid.NewGuid(),
            //    roleID = role.roleID,
            //    userID = myuser.userID
            //};
            //dbcontext.userRoles.Add(myuserrole);
            //dbcontext.SaveChanges();



            return View();
        }
        public ActionResult qrReadr()
        {
            return View();
        }
        public ActionResult Index(string gu, string tb, string mes)
        {
            // excel();
            // the health bar :gu=88a42d05-25bb-4cdb-88b7-b2c5f0172c11&tb=4b7cb1bd-c964-4536-9bb9-5a64e8d25ea8
            if (mes == "1")
            {
                ViewBag.endorder = "Your Order Is Registered Successfully";
            }
            if (Request.Cookies["gu"] != null && gu == null)
            {

                if (Request.Cookies["gu"].Value != "")
                {
                    gu = Request.Cookies["gu"].Value;
                    tb = Request.Cookies["tb"] != null ? Request.Cookies["tb"].Value : "";
                }

            }


            //if (tb == null)
            //{

            //    tb = "2f658781-4d99-49ad-9a50-c294cafc9d70";
            //}

            //if (gu == null)
            //{
            //    gu = "1c0b9a5d-f5ec-4224-a1ac-a67a0144027e";
            //}


            //if (Response.Cookies["us"] == null)
            //{
            //    if (Response.Cookies["us"].Value == "")
            //    {
            //        Response.Cookies["us"].Value = CustomMethods.RandomString(25);
            //    }
            //}



            Guid resturanGUID = new Guid(gu);
            List<category> lst = dbcontext.categories.Where(x => x.restaurantID == resturanGUID).ToList();
            foreach (var item in lst)
            {
                List<Item> firstItem = dbcontext.items.Where(x => x.categoryID == item.categoryID).ToList();
                string imgaddress = "MM3.png";
                if (firstItem.Count > 0)
                {
                    imgaddress = firstItem.Last().imageList.Split(',').First();
                }

                item.imageAddress = imgaddress;
            }

            Guid id = new Guid(gu);
            Response.Cookies["gu"].Value = gu;
            Response.Cookies["tb"].Value = tb;

            List<restaurant> lts = dbcontext.restaurants.ToList();

            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);
            int openning = Int32.Parse(rst.oppening.Hour.ToString());
            int closing = Int32.Parse(rst.colsing.Hour.ToString());
            int hour = Int32.Parse(DateTime.Now.AddHours(11).Hour.ToString());
            
            if (hour < openning || hour > closing)
            {
                //ViewBag.closed = "1";
               // ViewBag.time = openning + "/" + closing + "serverTime : "+ DateTime.Now.ToString();

                Response.Cookies["sabad"].Value = "0";
            }


            Response.Cookies["pardakht"].Value = rst.pardakht.ToString();
            Response.Cookies["sabad"].Value = rst.sabad.ToString();
            ViewBag.pardakht = rst.pardakht.ToString();
            ViewBag.sabad = rst.sabad.ToString();
            string istrue = "";
            if (Request.Cookies["or"] != null)
            {
                istrue = "1";
            }
            ViewBag.isTrue = istrue;
            //rst.username = "niki";
            //rst.password = "password";
            //dbcontext.SaveChanges();
            Response.Cookies["logo"].Value = rst.logo;
            Response.Cookies["username"].Value = rst.username;
            ViewBag.username = Request.Cookies["username"].Value as string;
            //rst.mobile = "09194594505";
            //rst.phone = "02188445566";
            //rst.whatsapp = "989124594505";
            //rst.instagram = "https://www.instagram.com/cafeniki";
            //rst.totem = "لحظه های شما  برای ما ارزشمند است";
            //rst.description = "ما در کافه رستوران نیکی با ایجاد فضای دنج و دوست داشتنی و منوی کامل با تنوع بی نظیر تلاش کردیم تا سهم کوچکی در لحظات شاد شما داشته باشیم";

            //dbcontext.SaveChanges();
            category mainitem = lst.SingleOrDefault(x => x.title == "Main");
            if (mainitem != null)
            {
                lst.Remove(mainitem);
            }
            indexVM model = new indexVM()
            {
                calist = lst,
                image = rst.imageAddress,
                title = rst.title,
                garson = rst.garson
            };

            return View(model);
        }

        public ActionResult Pager(string val)
        {
            string gu = Request.Cookies["gu"].Value;
            string tb = Request.Cookies["tb"].Value;
            Guid plcID = new Guid(tb);
            place plc = dbcontext.places.SingleOrDefault(x => x.placeID == plcID);
            plc.calling = 1;
            plc.message = val;
            dbcontext.SaveChanges();

            return Content("200");
        }


        //public ActionResult searchList()
        //{

        //}
        public ActionResult Detail(Guid id)
        {
            ViewBag.Message = "Your application description page.";
            category cat = dbcontext.categories.SingleOrDefault(x => x.categoryID == id);

            List<Item> itemList = dbcontext.items.Where(x => x.categoryID == id && x.isDisabled == 0).ToList();


            detailVM model = new detailVM()
            {
                name = cat.title,
                id = cat.categoryID.ToString().Replace("{", "").Replace("}", ""),
                itemList = itemList
            };
            ViewBag.username = Request.Cookies["username"].Value as string;
            string istrue = "";
            if (Request.Cookies["or"] != null)
            {
                istrue = "1";
            }
            ViewBag.isTrue = istrue;
            ViewBag.pardakht = Request.Cookies["pardakht"].Value;
            ViewBag.sabad = Request.Cookies["sabad"].Value;

            return View(model);
        }
        public ActionResult ItemDetail(string id)
        {

            ViewBag.username = Request.Cookies["username"].Value as string;
            string istrue = "";
            if (Request.Cookies["or"] != null)
            {
                istrue = "1";
            }

            Guid itemID = new Guid(id);
            Item model = dbcontext.items.SingleOrDefault(x => x.itemID == itemID);
            //model.ingredient = "آب میوه طبیعی، طعم دهنده، نعنا، رنگ خوراکی";
            ViewBag.isTrue = istrue;
            dbcontext.SaveChanges();
            ViewBag.pardakht = Request.Cookies["pardakht"].Value;
            ViewBag.sabad = Request.Cookies["sabad"].Value;

            List<mavadItemVM> mavadList = (from m in dbcontext.mavads   //.Join .Where(x => x.itemID == rst.itemguid).ToList();
                                           join mi in dbcontext.mavadItems
                                           on m.mavadID equals mi.mavadID
                                           where mi.itemID == itemID
                                           select new mavadItemVM
                                           {
                                               amount = mi.amount,
                                               title = m.title,
                                               ingID = mi.mavadID.ToString()

                                           }).ToList();

            List<Item> itemlist = dbcontext.items.Where(x => x.categoryID == model.categoryID && x.isDisabled == 0).OrderBy(r => Guid.NewGuid()).Take(5).ToList();
            itemDetailVM fmodel = new itemDetailVM();
            fmodel.item = model;
            fmodel.mavadList = mavadList;
            fmodel.itemlist = itemlist;

            return View(fmodel);
        }

        public ActionResult getMealMavd(string id)
        {
            Guid gid = new Guid(id);
            List<mavadItemVM> mavadList = (from m in dbcontext.mavads   //.Join .Where(x => x.itemID == rst.itemguid).ToList();
                                           join mi in dbcontext.mavadItems
                                           on m.mavadID equals mi.mavadID
                                           where mi.itemID == gid
                                           select new mavadItemVM
                                           {
                                               amount = mi.amount,
                                               title = m.title,
                                               ingID = mi.mavadID.ToString()

                                           }).ToList();

            return PartialView("/Views/Shared/_mavadList.cshtml", mavadList);
        }

        public string addtocart(string id, string addtocart, string desc)
        
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
                newitem.madatToRemove = addtocart;
                newitem.description = desc;

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
                    if (item.productid == str && item.madatToRemove == addtocart)
                    {
                        item.quantity += 1;
                        item.madatToRemove = addtocart;
                        item.description = desc;
                        j = 1;

                    }

                }

                if (j == 0)
                {
                    newitem.productid = str;
                    newitem.quantity = 1;
                    newitem.madatToRemove = addtocart;
                    newitem.description = desc;
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

                    if (item.productid == str)
                    {
                        lastindex = data.IndexOf(item);
                        if (item.quantity > 0)
                        {
                            item.quantity -= 1;
                        }

                        if (item.quantity == 0)
                        {
                            boolian = true;
                        }
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
                                                                                                                    //model.RemoveRange(model.Where(x => x.quantity == 0).ToList());

            //int final = model != null ?  model.Sum(x => x.quantity) : 0;
            return Content(cartModelString);
        }

        public ActionResult getBasket()
        {
            string error = "";
            if (error == "1")
            {
                ViewBag.error = error;
            }

            List<checkOutVM> finalmodel = new List<checkOutVM>();
            ViewBag.username = Request.Cookies["username"].Value as string;
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            ViewBag.cookie = cartModelString;
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
            List<Item> lst = new List<Item>();

            double price = 0;
            double discount = 0;
            if (data != null)
            {
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
            }

            EndOrderVM model = new EndOrderVM()
            {
                lst = lst,
                discount = discount,
                orderPrice = price,
                finalPrice = price - discount
            };
            return PartialView("/Views/Shared/_basketPartial.cshtml", model);
        }
        public ActionResult basket(string error)
        {

            if (error == "1")
            {
                ViewBag.error = error;
            }
            string rststring = Request.Cookies["gu"].Value;
            Guid resguid = new Guid(rststring);
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == resguid);
            List<checkOutVM> finalmodel = new List<checkOutVM>();
            ViewBag.username = Request.Cookies["username"].Value as string;
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            ViewBag.cookie = cartModelString;
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
            List<Item> lst = new List<Item>();

            double price = 0;
            double discount = 0;
            if (data == null)
            {
                return RedirectToAction("index");
            }
            foreach (var item in data)
            {
                if (item.quantity > 0)
                {
                    Guid itemID = new Guid(item.productid);
                    Item adddItem = new Item();
                    Item myItem = dbcontext.items.SingleOrDefault(x => x.itemID == itemID);
                    List<mavadItemVM> mavadList = (from m in dbcontext.mavads   //.Join .Where(x => x.itemID == rst.itemguid).ToList();
                                                   join mi in dbcontext.mavadItems
                                                   on m.mavadID equals mi.mavadID
                                                   where mi.itemID == itemID
                                                   select new mavadItemVM
                                                   {
                                                       amount = mi.amount,
                                                       title = m.title,
                                                       ingID = mi.mavadID.ToString()

                                                   }).ToList();
                    adddItem.ingredient = "";
                    foreach (var mavd in mavadList)
                    {
                        if (!item.madatToRemove.Contains(mavd.ingID.ToString() + ","))
                        {
                            adddItem.ingredient += mavd.title + ",";
                        }

                    }
                    adddItem.ingredient.Trim(',');
                    adddItem.desctiption = item.description;
                    adddItem.title = myItem.title;
                    adddItem.itemID = myItem.itemID;
                    adddItem.imageList = myItem.imageList;
                    adddItem.price = myItem.price;

                    price += myItem.price * item.quantity;
                    lst.Add(adddItem);
                }

            };
            EndOrderVM model = new EndOrderVM()
            {
                lst = lst,
                discount = discount,
                orderPrice = price,
                finalPrice = price - discount,
                vat = rst.vat
            };
            string istrue = "";
            if (Request.Cookies["or"] != null)
            {
                istrue = "1";
            }
            ViewBag.isTrue = istrue;
            ViewBag.pardakht = Request.Cookies["pardakht"].Value;
            ViewBag.sabad = Request.Cookies["sabad"].Value;
            return View(model);

        }
       
        public ActionResult setOrder()
        {

            string restaurantID = Request.Cookies["gu"].Value as string;
            Guid resid = new Guid(restaurantID);
            restaurant restaurantItem = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == resid);
            //string us = Request.Cookies["us"].Value as string;
            if (Request.Cookies["tb"] != null)
            {
                if (Request.Cookies["tb"].Value == "")
                {
                    return RedirectToAction("basket", new { error = 1 });
                }
            }
            else
            {
                return RedirectToAction("basket", new { error = 1 });
            }
            Guid tableID = new Guid(Request.Cookies["tb"].Value as string);
            place place = dbcontext.places.SingleOrDefault(x => x.placeID == tableID);
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
            order neworder = new order();
            neworder.restaurantID = new Guid(restaurantID);
            neworder.orderID = orderID;
            neworder.orderNumber = restaurantItem.orderNumberBegin + 1;
            neworder.placeID = tableID;
            neworder.place = place.title;
            neworder.us = "";

            neworder.isRefine = 0;
            neworder.minutToAdd = 10;
            neworder.peigiry = peigiry;
            neworder.orderTime = DateTime.Now;
            restaurantItem.orderNumberBegin += 1;

            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
            double finalPrice = 0;
            foreach (var item in data)
            {
                Model.Item selitem = dbcontext.items.SingleOrDefault(c => c.itemID == new Guid(item.productid));
                List<mavadItem> lstmavaitem = dbcontext.mavadItems.Where(x => x.itemID == selitem.itemID).ToList();

                Guid orderdetailID = Guid.NewGuid();
                dbcontext.orderDetails.Add(new orderDetail()
                {
                    count = item.quantity,
                    itemID = new Guid(item.productid),
                    orderID = orderID,
                    orderDetailID = orderdetailID,
                    price = selitem.price,
                     description = item.description
                });

                foreach (var mavadChosen in lstmavaitem)
                {
                    if (!item.madatToRemove.Contains(mavadChosen.mavadID.ToString() + ","))
                    {
                        dbcontext.orderDetailMavads.Add(new orderDetailMavad()
                        {
                            mavaID = mavadChosen.mavadID,
                            orderDetailID = orderdetailID,
                            odmID = Guid.NewGuid()
                        });
                    }

                }



                finalPrice += selitem.price * item.quantity;
            }

            neworder.price = finalPrice;
            dbcontext.orders.Add(neworder);
            dbcontext.SaveChanges();
            string liveOrder = orderID.ToString();
            Response.Cookies["liveOrder"].Value = liveOrder;

            Response.Cookies["Modelcart"].Value = "";


            if (Request.Cookies["or"] != null)
            {
                return RedirectToAction("info", "Admin");
            }
            else
            {
                return RedirectToAction("invoic", new { id = liveOrder });
            }

        }

        public ActionResult invoic(string id)
        {

            string restaurantID = Request.Cookies["gu"].Value as string;
            Guid resid = new Guid(restaurantID);
            restaurant restaurantItem = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == resid);


            Guid uid = new Guid(id);
            List<orderDetail> odlst = dbcontext.orderDetails.Where(x => x.orderID == uid).ToList();
            invoicVM model = new invoicVM()
            {
                lst = odlst,
                vat = restaurantItem.vat,
                orderID = id,
            };
            return View(model);

        }
        public ActionResult liveOrder(string id)
        {
            return View();
        }

        public ActionResult info(string message)
        {
            if (message != null)
            {
                ViewBag.message = message;
            }

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
                foreach (var peigir in orderPeigiry.Trim(',').Split(',').ToList())
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

        public ActionResult orderDetail(string orderNumber)
        {
            //string us = Request.Cookies["us"].Value as string;
            ViewBag.username = Request.Cookies["username"].Value as string;
            Guid orid = new Guid(orderNumber);
            order order = dbcontext.orders.SingleOrDefault(x => x.orderID == orid);
            List<Guid> ids = new List<Guid>();
            List<orderDetailVM> itemList = new List<orderDetailVM>();
            if (order.orderID != null)
            {
                itemList = (from a in dbcontext.orderDetails
                            join c in dbcontext.items on a.itemID equals c.itemID
                            where a.orderID == order.orderID
                            select new orderDetailVM { itemprice = c.price, itemTitle = c.title, itemImage = c.imageList, itemDesc = c.desctiption, itemCount = a.count }).ToList();


                orderDetailPageVM model = new orderDetailPageVM()
                {
                    list = itemList,
                    name = "سفارش شماره : " + order.orderNumber.ToString()
                };


                return View(model);
            }

            return Content("");
        }

        public ActionResult ReqestForPaymentZarin(string orderID)
        {
            Guid ordergu = new Guid(orderID);
            //string us = Request.Cookies["us"].Value as string;
            order order = dbcontext.orders.SingleOrDefault(x => x.orderID == ordergu);
            //if (order.us != us)
            //{
            //    return Content("error");
            //}



            string restaurantID = Request.Cookies["gu"].Value as string;
            Guid mygu = new Guid(restaurantID);
            restaurant rs = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == mygu);
            string zarin = rs.zarin;
            string srt = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            string txtDescription = "شماره پیگیری:" + order.orderNumber;
            string callB = "https://www.damane.ir/Home/VerifyZarin";
            System.Net.ServicePointManager.Expect100Continue = false;
            ServiceReference1.PaymentGatewayImplementationServicePortTypeClient zp = new ServiceReference1.PaymentGatewayImplementationServicePortTypeClient();
            string Authority;
            int Status = zp.PaymentRequest(zarin, (int)order.price, txtDescription, "info@banimo.com", "", callB, out Authority);
            if (Status == 100)
            {

                order.auth = Authority;
                dbcontext.SaveChanges();
                Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + Authority);

            }
            else
            {
                return Content("error: " + Status);
            }




            //string json = "";
            return Content("");


        }

        public ActionResult VerifyZarin()
        {


            try
            {

                if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
                {
                    if (Request.QueryString["Status"].ToString().Equals("OK"))
                    {
                        string authority = Request.QueryString["Authority"];
                        // get order by auth Request.QueryString["Authority"]
                        order order = dbcontext.orders.SingleOrDefault(x => x.auth == authority);
                        int Amount = (int)order.price;
                        long RefID;
                        System.Net.ServicePointManager.Expect100Continue = false;
                        ServiceReference1.PaymentGatewayImplementationServicePortTypeClient zp = new ServiceReference1.PaymentGatewayImplementationServicePortTypeClient();
                        int Status = zp.PaymentVerification(ConfigurationManager.AppSettings["zarin"], Request.QueryString["Authority"].ToString(), Amount, out RefID);
                        if (Status == 100)
                        {

                            order.status = 1;
                            dbcontext.SaveChanges();
                            return RedirectToAction("info", "Home", new { refID = RefID, status = 1 });


                        }
                        else
                        {
                            return RedirectToAction("info", "Home", new { refID = RefID + "-" + Status, status = 0 });
                        }

                    }
                    else
                    {
                        //Response.Write("some thing wrong with payment");
                    }
                }
                else
                {
                    //Response.Write("Invalid Input");
                }
            }
            catch (Exception ex)
            {
                //// Get stack trace for the exception with source file information
                //var st = new StackTrace(ex, true);
                //// Get the top stack frame
                //var frame = st.GetFrame(0);
                //// Get the line number from the stack frame
                //var line = frame.GetFileLineNumber();
                //int linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                //Response.Write(ex.Message + "-" + linenum);
            }


            return View();
        }

        public ActionResult setNewUser(string form_name, string form_email, string form_message)
        {
            Guid messageID = Guid.NewGuid();
            userMessage message = new userMessage()
            {
                messageID = messageID,
                email = form_email,
                name = form_name,
                text = form_message
            };
            dbcontext.userMessages.Add(message);
            dbcontext.SaveChanges();

            return Content("");
        }
        public ActionResult registerEmail(string form_email)
        {
            Guid messageID = Guid.NewGuid();
            emailBlog message = new emailBlog()
            {
                messageID = messageID,
                email = form_email,

            };
            dbcontext.emailBlogs.Add(message);
            dbcontext.SaveChanges();

            return Content("");
        }

        public void excel()
        {
            DataTable dt = new DataTable();
            string path = "G:/ALLPROJECT/ActiveMenu/ActiveMenu/file/Menumotive.xlsx";
            using (XLWorkbook workbook = new XLWorkbook(path))
            {
                IXLWorksheet worksheet = workbook.Worksheet(1);
                bool FirstRow = true;
                //Range for reading the cells based on the last cell used.  
                string readRange = "1:1";
                foreach (IXLRow row in worksheet.RowsUsed())
                {
                    //If Reading the First Row (used) then add them as column name  
                    if (FirstRow)
                    {
                        //Checking the Last cellused for column generation in datatable  
                        readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        FirstRow = false;
                    }
                    else
                    {
                        //Adding a Row in datatable  
                        dt.Rows.Add();
                        int cellIndex = 0;
                        //Updating the values of datatable  
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                            cellIndex++;
                        }
                    }
                }
                //If no data in Excel file  
                if (FirstRow)
                {
                    //ViewBag.Message = "Empty Excel File!";
                }
                //List<EcxelList> listIIEM = new List<EcxelList>();
                foreach (DataRow row in dt.Rows)
                {
                    string title = row[1].ToString();
                    decimal price = decimal.Parse(row[3].ToString());
                    string desc = row[2].ToString();
                    string catID = row[4].ToString();


                    Guid GUIDITEM = Guid.NewGuid();
                    string ITEMID = GUIDITEM.ToString();
                    Item item = new Item()
                    {
                        categoryID = new Guid(catID),
                        desctiption = desc,
                        imageList = title + ".png",
                        ingredient = "",
                        itemID = GUIDITEM,
                        price = Convert.ToInt64(price),
                        title = title,
                    };

                    dbcontext.items.Add(item);


                }
                dbcontext.SaveChanges();




            }
        }
        public ActionResult getReciet(string id)
        {
            string mystring = "";
            //CreatePDFFromHTMLFile(id);
            string fn = CreatePDFFromHTMLFile(id);


            byte[] fileBytes = System.IO.File.ReadAllBytes(fn);
            string fileName = "invoice.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


            //var pathToTheFile = Path.Combine(Server.MapPath("~/Content/Downloads/"), filename);
            //var fileStream = new FileStream(filename,
            //                                    FileMode.Open,
            //                                    FileAccess.Read
            //                                );
            //return new FileStreamResult(fileStream, "application/pdf");
        }

        private string CreatePDFFromHTMLFile(string id)
        {

            string name = RandomString(5) + ".pdf";
            string srt = Path.Combine(Server.MapPath("/file/"), name);
            System.IO.FileStream fs = new FileStream(srt, FileMode.Create);
            //Response.ContentType = "application/pdf";

            //Response.AddHeader("content-disposition", "attachment;filename=test_22.pdf");

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string imageFilePath = Server.MapPath("/images/app/Invoice.png");

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);

            // Page site and margin left, right, top, bottom is defined
            Document pdfDoc = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, fs);
            //Resize image depend upon your need
            //For give the size to image
            jpg.SetAbsolutePosition(0, 0);
            jpg.ScaleAbsoluteHeight(pdfDoc.PageSize.Height);
            jpg.ScaleAbsoluteWidth(pdfDoc.PageSize.Width);
            //jpg.ScaleToFit(3500, 800);

            //If you want to choose image as background then,

            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

            //If you want to give absolute/specified fix position to image.
            //jpg.SetAbsolutePosition(70, 69);

            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

            pdfDoc.Open();

            pdfDoc.NewPage();


            //string us = Request.Cookies["us"].Value as string;
            //ViewBag.username = Request.Cookies["username"].Value as string;
            Guid orid = new Guid(id);
            order order = dbcontext.orders.SingleOrDefault(x => x.orderID == orid);

            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == order.restaurantID);
            List<Guid> ids = new List<Guid>();
            List<orderDetailVM> itemList = new List<orderDetailVM>();
            orderDetailPageVM model = new orderDetailPageVM();


            pdfDoc.Add(jpg);
            PdfContentByte pcb = writer.DirectContent;

            string path = Path.Combine(Server.MapPath("/images/Logo/" + Request.Cookies["username"].Value + "/"), rst.logo);

            if (!System.IO.File.Exists(path))
            {
                path = Path.Combine(Server.MapPath("/assetes/portal/assets/images/"), "logo.jpg");
            }


            string savedimageString = path;
            iTextSharp.text.Image topImage = iTextSharp.text.Image.GetInstance(savedimageString);
            topImage.WidthPercentage = 20;
            topImage.Alignment = Element.ALIGN_RIGHT;


            topImage.ScaleToFit(100f, 100f);
            topImage.SetAbsolutePosition(450, 730);
            pdfDoc.Add(topImage);

            PdfContentByte cb = writer.DirectContent;

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 11);
            cb.BeginText();
            cb.SetTextMatrix(130, 690);
            cb.ShowText(order.orderTime.ToShortDateString().ToString());
            cb.EndText();


            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 11);
            cb.BeginText();
            cb.SetTextMatrix(380, 690);
            cb.ShowText(order.orderNumber.ToString());
            cb.EndText();

            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 11);
            cb.BeginText();
            cb.SetTextMatrix(60, 90);
            cb.ShowText(rst.phone);
            cb.EndText();


            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 13);
            cb.BeginText();
            cb.SetTextMatrix(60, 70);
            cb.ShowText(rst.Address);
            cb.EndText();


            PdfPTable table = new PdfPTable(12);
            table.TotalWidth = 477f;


            if (order.orderID != null)
            {
                itemList = (from a in dbcontext.orderDetails
                            join c in dbcontext.items on a.itemID equals c.itemID
                            where a.orderID == order.orderID
                            select new orderDetailVM { itemprice = c.price, itemTitle = c.title, itemImage = c.imageList, itemDesc = c.desctiption, itemCount = a.count }).ToList();


                model = new orderDetailPageVM()
                {
                    list = itemList,
                    name = "سفارش شماره : " + order.orderNumber.ToString()
                };



            }
            var blackListTextFont = FontFactory.GetFont("Arial", 10, WebColors.GetRGBColor("#4d4d4d"));
            BaseColor myColor = WebColors.GetRGBColor("#fff");
            foreach (var item in itemList)
            {

                int i = itemList.IndexOf(item);
                if ((i + 1) % 2 == 0)
                {
                    myColor = WebColors.GetRGBColor("#fff");
                }
                else
                {
                    myColor = WebColors.GetRGBColor("#eee");
                }


                PdfPCell fullname = new PdfPCell(new Phrase(item.itemTitle, blackListTextFont));
                fullname.NoWrap = false;
                fullname.SetLeading(14, 0);
                fullname.Border = Rectangle.BOTTOM_BORDER;
                fullname.BorderColorBottom = WebColors.GetRGBColor("#eee");
                fullname.BackgroundColor = myColor;
                fullname.Colspan = 6;
                fullname.PaddingTop = 5;
                fullname.PaddingBottom = 10;
                fullname.PaddingLeft = 15;
                fullname.VerticalAlignment = Element.ALIGN_MIDDLE;
                fullname.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(fullname);

                fullname = new PdfPCell(new Phrase(item.itemCount.ToString(), blackListTextFont));
                fullname.NoWrap = false;
                fullname.Border = Rectangle.BOTTOM_BORDER;
                fullname.BorderColorBottom = WebColors.GetRGBColor("#eee");
                fullname.BackgroundColor = myColor;
                fullname.SetLeading(14, 0);
                fullname.Colspan = 2;
                fullname.PaddingBottom = 10;
                fullname.PaddingTop = 5;
                fullname.VerticalAlignment = Element.ALIGN_MIDDLE;
                fullname.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(fullname);

                fullname = new PdfPCell(new Phrase(item.itemprice.ToString() + " AED", blackListTextFont));
                fullname.NoWrap = false;
                fullname.Border = Rectangle.BOTTOM_BORDER;
                fullname.BackgroundColor = myColor;
                fullname.SetLeading(14, 0);
                fullname.Colspan = 2;
                fullname.PaddingBottom = 10;
                fullname.PaddingTop = 5;
                fullname.VerticalAlignment = Element.ALIGN_MIDDLE;
                fullname.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(fullname);

                fullname = new PdfPCell(new Phrase((item.itemprice * item.itemCount).ToString() + " AED", blackListTextFont));
                fullname.NoWrap = false;
                fullname.Border = Rectangle.BOTTOM_BORDER;
                fullname.BackgroundColor = myColor;
                fullname.SetLeading(14, 0);
                fullname.Colspan = 2;
                fullname.PaddingBottom = 10;
                fullname.PaddingTop = 5;
                fullname.PaddingRight = 12;
                fullname.VerticalAlignment = Element.ALIGN_MIDDLE;
                fullname.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(fullname);
            }




            double subtotal = itemList.Sum(x => x.itemCount * x.itemprice);
            double vatPrice = (itemList.Sum(x => x.itemCount * x.itemprice) * rst.vat) / 100;
            double finaltotal = subtotal + vatPrice;


            myColor = WebColors.GetRGBColor("4d4d4d");

            PdfPCell cell = new PdfPCell(new Phrase("", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 8;
            cell.PaddingTop = 20;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Sub Total", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 2;
            cell.PaddingTop = 20;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase(subtotal.ToString() + " AED", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 2;
            cell.PaddingTop = 20;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);




            cell = new PdfPCell(new Phrase("", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 8;
            cell.PaddingTop = 2;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Vat", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 2;
            cell.PaddingTop = 2;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(vatPrice + " AED", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 2;
            cell.PaddingTop = 2;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 8;
            cell.PaddingTop = 2;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Total", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 2;
            cell.PaddingTop = 2;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(finaltotal + " AED", blackListTextFont));
            cell.NoWrap = false;
            cell.SetLeading(14, 0);
            cell.BorderColorBottom = WebColors.GetRGBColor("#eee");
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 2;
            cell.PaddingTop = 2;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 15;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);



            table.WriteSelectedRows(0, -1, 62, 607, pcb);


            //bottomable.WriteSelectedRows(0, -1, 300, 300 ,);


            pdfDoc.Close();
            writer.Close();
            // Always close open filehandles explicity  
            fs.Close();
            //Response.Write(pdfDoc);

            //Response.End();



            return srt;
        }

    }
}