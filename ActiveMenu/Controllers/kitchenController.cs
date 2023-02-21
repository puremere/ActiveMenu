using ActiveMenu.Classes;
using ActiveMenu.Model;
using ActiveMenu.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActiveMenu.Controllers
{
    [CoreSessionCheck]
    public class KitchenController : Controller
    {
        // GET: Kitchen
        Model.Context dbcontext = new Model.Context();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string error)
        {
            ViewBag.error = error;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            List<user> usermy = dbcontext.users.ToList();
            if (dbcontext.users.Any(x => x.username == username && x.password == password))
            {
                user myuser = dbcontext.users.SingleOrDefault(x => x.username == username && x.password == password);
                Guid userID = myuser.userID;

                userRole loginmodel = dbcontext.userRoles.SingleOrDefault(x => x.userID == userID);
                restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == loginmodel.restuantID);

                Session["gu"] = loginmodel.restuantID.ToString();
                Response.Cookies["gu"].Value = loginmodel.restuantID.ToString();
                Response.Cookies["username"].Value = rst.username;
                if (rst.logo != "")
                {
                    Response.Cookies["logo"].Value = rst.logo;
                }

                string subPath = "/images/Qr/" + rst.username; // Your code goes here
                string subPath2 = "/images/Qr2/" + rst.username; // Your code goes here
                string subPath3 = "/images/Logo/" + rst.username; // Your code goes here
                string subPath4 = "/images/Items/" + rst.username; // Your code goes here
                string subPath5 = "/images/Categories/" + rst.username; // Your code goes here
                List<string> lst = new List<string>();
                lst.Add(subPath);
                lst.Add(subPath2);
                lst.Add(subPath3);
                lst.Add(subPath4);
                lst.Add(subPath5);

                foreach (var item in lst)
                {
                    bool exists = System.IO.Directory.Exists(Server.MapPath(item));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(item));
                }



                if (loginmodel.roleID == "Management")
                {
                    Session["logedInUser"] = "Management";
                    return RedirectToAction("dashbaord", "Management");

                }
                else if (loginmodel.roleID == "Kitchen")
                {
                    Session["logedInUser"] = "kitchen";
                    return RedirectToAction("dashbaord", "kitchen");
                }
                else
                {
                    return RedirectToAction("Login", new { error = 1 });
                }
            }
            else
            {
                return RedirectToAction("Login", new { error = 1 });
            }


        }
        public ActionResult dashbaord()
        {

            ViewBag.username = Request.Cookies["username"].Value;

           

            List<currentVM> orders = (from od in dbcontext.orderDetails
                                      join o in dbcontext.orders
                                      on od.orderID equals o.orderID
                                      join itm in dbcontext.items
                                      on od.itemID equals itm.itemID
                                      where o.final != 1 && od.final != 1
                                      orderby o.place
                                      select new currentVM
                                      {
                                          itemImage = itm.imageList,
                                          itemorderCount = od.count,
                                          itemordertitle = itm.title,
                                          orderDetailDesc = od.description,
                                          orderdetailID = od.orderDetailID,
                                          orderplace = o.place
                                      }).ToList();


            return View(orders);
        }
       
        public ActionResult meals(string id)
        {

            //string us = Request.Cookies["us"].Value as string;
            ViewBag.username = Request.Cookies["username"].Value as string;
            Guid orid = new Guid(id);
            order order = dbcontext.orders.SingleOrDefault(x => x.orderID == orid);
            List<Guid> ids = new List<Guid>();
            List<orderDetailVM> itemList = new List<orderDetailVM>();
            orderDetailPageVM model = new orderDetailPageVM();
            if (order.orderID != null)
            {
                itemList = (from a in dbcontext.orderDetails
                            join c in dbcontext.items on a.itemID equals c.itemID
                            where a.orderID == order.orderID
                            select new orderDetailVM { itemprice = c.price, itemTitle = c.title, itemImage = c.imageList, itemDesc = c.desctiption, itemCount = a.count }).ToList();


                 model = new orderDetailPageVM()
                {
                    list = itemList,
                    name = "Order Number : " + order.orderNumber.ToString()
                };


               
            }
            

            return View(model);
        }
        public ActionResult orders()
        {
            string orderPeigiry = "";

            string gu = Request.Cookies["gu"].Value.ToString();
            Guid resgu = new Guid(gu);
            List<order> orders = dbcontext.orders.Where(x => x.restaurantID == resgu ).ToList();
            foreach (var ord in orders)
            {

                if (ord != null)
                {
                    TimeSpan timspan = DateTime.Now.Subtract(ord.orderTime);
                    ord.minutPassed = timspan.TotalMinutes;

                }

            }
            return View(orders);
        }
        public ActionResult Current()
        {


            List<currentVM> orders = (from od in dbcontext.orderDetails
                                      join o in dbcontext.orders
                                      on od.orderID equals o.orderID
                                      join itm in dbcontext.items
                                      on od.itemID equals itm.itemID
                                      where o.final != 1 && od.final != 1
                                      orderby o.place
                                      select new currentVM
                                      {
                                          itemImage = itm.imageList,
                                          itemorderCount = od.count,
                                          itemordertitle = itm.title,
                                          orderDetailDesc = od.description,
                                          orderdetailID = od.orderDetailID,
                                          orderplace = o.place
                                      }).ToList();


            return View(orders);
        }
        public ActionResult getOrderDetailMavad(string id )
        {
            OrderDetailMavadVM vmmodel = new OrderDetailMavadVM();
            Guid sendID = new Guid(id);
            List<vmItemMavad> model = (from m in dbcontext.mavads
                                 join p in dbcontext.orderDetailMavads
                                 on m.mavadID equals p.mavaID
                                 join mi in dbcontext.mavadItems
                                 on m.mavadID equals mi.mavadID
                                 where p.orderDetailID == sendID
                                 select new vmItemMavad { 
                                        title = m.title,
                                         amount = mi.amount.ToString()
                                 } ).ToList();

            
            vmmodel.lst = model;
            orderDetail ord = dbcontext.orderDetails.SingleOrDefault(x => x.orderDetailID == sendID);

            vmmodel.instruction = dbcontext.items.SingleOrDefault(x => x.itemID == ord.itemID).Instruction;
            return PartialView("/Views/Shared/_orderDetailMavad.cshtml", vmmodel);
        }
        
        public ActionResult EndOrderDetail(string id)
        {
            Guid orduid = new Guid(id);
            orderDetail ord = dbcontext.orderDetails.SingleOrDefault(c => c.orderDetailID == orduid);
            ord.final = 1;
            dbcontext.SaveChanges();
            return Content("");
        }


    }
}