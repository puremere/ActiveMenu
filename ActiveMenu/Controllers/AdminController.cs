using ActiveMenu.Model;
using ActiveMenu.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IronBarCode;
using System;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using QRCoder;
using System.Configuration;

namespace ActiveMenu.Controllers
{
    public class AdminController : Controller
    {
        Context dbcontext = new Context();
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void SetCookie(string mymodel, string name)
        {

            Response.Cookies[name].Value = mymodel;

        }
        private string getCookie(string name)
        {

            string req2 = "";
            if (Request.Cookies[name] != null)
            {
                req2 = Request.Cookies[name].Value;
            }

            return req2;


        }
        // GET: Admin
        public ActionResult Index(string gu, string error, string tb)
        {
            




            if (Request.Cookies["gu"] != null && gu == null)
            {
                if (Request.Cookies["gu"].Value != "")
                {
                    gu = Request.Cookies["gu"].Value;
                    //tb = Request.Cookies["tb"].Value;
                }

            }
            else
            {
                ViewBag.error = error;
                if (gu == null)
                {
                    gu = "1c0b9a5d-f5ec-4224-a1ac-a67a0144027e";
                }
            }

            Response.Cookies["gu"].Value = gu;

            Guid id = new Guid(gu);
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);
            Response.Cookies["logo"].Value = rst.logo;
            Response.Cookies["username"].Value = rst.username;
            ViewBag.username = Request.Cookies["username"].Value as string;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {

            string gu = Request.Cookies["gu"].Value as string;
            Guid id = new Guid(gu);
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);


            if (!(rst.username == username && rst.password == rst.password))
            {
                return RedirectToAction("index", new { error = 1 });
            }

            Response.Cookies["or"].Value = rst.username;

            return RedirectToAction("info");
        }

        public void changeTB(string id)
        {
            Response.Cookies["tb"].Value = id;
        }
        public void readdata()
        {
            string pathString = "~/file";
            string finalpath = Path.Combine(Server.MapPath(pathString), "components.csv");
            StreamReader reader = null;
            if (System.IO.File.Exists(finalpath))
            {
                reader = new StreamReader(System.IO.File.OpenRead(finalpath));
                List<string> listA = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    try
                    {
                        string title = values[1].Replace("\"", "");
                        decimal cals = decimal.Parse(values[8].Replace("\"", ""));
                        decimal proteins = decimal.Parse(values[9].Replace("\"", ""));
                        decimal carbs = decimal.Parse(values[10].Replace("\"", ""));
                        decimal fats = decimal.Parse(values[11].Replace("\"", ""));
                        mavad mavad = new mavad()
                        {
                            Calories = cals,
                            Fats = fats,
                            mojoodi = 0,
                            Proteins = proteins,
                            Carbohydrates = carbs,
                            restaurantID = new Guid(),
                            mavadID = Guid.NewGuid(),
                            title = title,
                            type = "0"
                        };
                        dbcontext.mavads.Add(mavad);
                        dbcontext.SaveChanges();
                    }
                    catch (Exception)
                    {

                        continue;
                    }




                }



            }
            else
            {
                Console.WriteLine("File doesn't exist");
            }
            Console.ReadLine();
        }
        public void setdata(string name, string uc, string pc, string ua, string pa)
        {

            //userRole user = dbcontext.userRoles.SingleOrDefault(x => x.roleID == "Kichen");
            //user.roleID = "Kitchen";

            //user user = dbcontext.users.SingleOrDefault(x => x.username == "admin.com");
            //user.username = "admin";
            //dbcontext.SaveChanges();

            restaurant rst = new restaurant()
            {
                Address = "",
                brandName = "",
                colsing = DateTime.Now,
                description = "",
                email = "",
                garson = "",
                imageAddress = "",
                instagram = "",
                logo = "",
                meyar = "",
                oppening = DateTime.Now,
                mobile = "",
                orderNumberBegin = 1,
                pardakht = 0,
                password = "",
                phone = "",
                restaurantID = Guid.NewGuid(),
                sabad = 1,
                title = "",
                totem = "",
                username = name,
                whatsapp = "",
                whatsappPhone = "",
                zarin = "",

            };
            dbcontext.restaurants.Add(rst);
            user user = new user()
            {
                password = pc,
                token = "hkjhslkjdhlskjdhf",
                userID = Guid.NewGuid(),
                username = uc
            };


            userRole userrole = new userRole()
            {
                restuantID = rst.restaurantID,
                roleID = "Kitchen",
                userID = user.userID,
                useroleID = Guid.NewGuid(),
            };
            dbcontext.users.Add(user);
            dbcontext.userRoles.Add(userrole);
            dbcontext.SaveChanges();


            user = new user()
            {
                password = pa,
                token = "hkjhslkjdhlskjdhf",
                userID = Guid.NewGuid(),
                username = ua
            };


            userrole = new userRole()
            {
                restuantID = rst.restaurantID,
                roleID = "Management",
                userID = user.userID,
                useroleID = Guid.NewGuid(),
            };
            dbcontext.users.Add(user);
            dbcontext.userRoles.Add(userrole);
            dbcontext.SaveChanges();
        }
        public ActionResult info()
        {
            //string gu = Request.Cookies["gu"].Value as string;
            //Guid id = new Guid(gu);
            //restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);
            //List<category> orderList = dbcontext.categories.Where(x => x.restaurantID == rst.restaurantID).ToList();
            return View();
        }
        public ActionResult getOrder()
        {
            string orderPeigiry = "";

            string gu = Request.Cookies["gu"].Value as string;
            Guid resgu = new Guid(gu);
            List<order> orders = dbcontext.orders.Where(x => x.restaurantID == resgu && x.final != 1).ToList();
            foreach (var ord in orders)
            {

                if (ord != null)
                {
                    TimeSpan timspan = DateTime.Now.Subtract(ord.orderTime);
                    ord.minutPassed = timspan.TotalMinutes;

                }

            }


            return PartialView("~/Views/Shared/_adminOrderStatus.cshtml", orders);
        }
        public ActionResult endOrder(string orderSrt)
        {
            Guid orderID = new Guid(orderSrt);
            order order = dbcontext.orders.SingleOrDefault(x => x.orderID == orderID);
            order.final = 1;
            dbcontext.SaveChanges();
            return Content("200");
        }
        public ActionResult rejectOrder(string orderSrt)
        {
            Guid orderID = new Guid(orderSrt);
            order order = dbcontext.orders.SingleOrDefault(x => x.orderID == orderID);
            order.final = 2;
            dbcontext.SaveChanges();
            return Content("200");
        }
        


        public ActionResult createCat(string item)
        {

            string gu = Request.Cookies["gu"].Value as string;
            Guid rid = new Guid(gu);
            category cat = dbcontext.categories.SingleOrDefault(x => x.title == item && x.restaurantID == rid);
            if (cat == null)
            {
                cat = new category();
                cat.restaurantID = rid;
                cat.categoryID = Guid.NewGuid();
                cat.title = item;
                cat.imageAddress = "palceHolder.png";
                cat.isVisible = 1;
                cat.priority = 0;
                cat.culomn = "2";
                dbcontext.categories.Add(cat);
                dbcontext.SaveChanges();
            }

            return Content("");
        }
        public ActionResult createItem(string item, string catid)
        {

            Response.Cookies["lastIng"].Value = "";
            string gu = Request.Cookies["gu"].Value as string;
            Guid id = new Guid(gu);
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);

            ViewModel.CreateItemVM model = new ViewModel.CreateItemVM();
            model.itemID = item == null ? "" : item;
            if (String.IsNullOrEmpty(item))
            {
                string imagesList = getCookie("images");
                if (imagesList != null)
                {
                    if (imagesList != "")
                        ViewBag.images = imagesList.Trim(',');
                }
            }
            else
            {
                Guid myitemID = new Guid(item);
                Item myitem = dbcontext.items.SingleOrDefault(c => c.itemID == myitemID);
                SetCookie(myitem.imageList, "images");
                ViewBag.images = myitem.imageList;
                model.item = myitem;

                List<mavadItemVM> mavadList = (from m in dbcontext.mavads   //.Join .Where(x => x.itemID == rst.itemguid).ToList();
                                               join mi in dbcontext.mavadItems
                                               on m.mavadID equals mi.mavadID
                                               where mi.itemID == myitemID
                                               select new mavadItemVM
                                               {
                                                   amount = mi.amount,
                                                   title = m.title,
                                                   mid = mi.mavaditemID.ToString()

                                               }).ToList();
                model.itemMavad = mavadList;
            }
            ViewBag.username = rst.username;
            model.username = rst.username;

            model.catItem = catid;
            model.lstMavad = dbcontext.mavads.ToList();
            //Guid itemguid = new Guid(item);

            return View(model);
        }


        public ActionResult removeIngredientTo(string mid)
        {

            Guid guid = new Guid(mid);

            string lastIng = "";
            if (Request.Cookies["lastIng"] != null)
            {
                lastIng = Request.Cookies["lastIng"].Value;
            }

            lastIng = lastIng.Replace(mid + ",", "");
            Response.Cookies["lastIng"].Value = lastIng;
            dbcontext.mavadItems.Remove(dbcontext.mavadItems.SingleOrDefault(x => x.mavaditemID == guid));
            dbcontext.SaveChanges();
            
            return Content("");
        }

        public ActionResult addIngredientTo(string description, string ItemID, string mavadItem, decimal amount)
        {

            Guid mid = Guid.NewGuid();
            Guid mavadFItem = new Guid(mavadItem);
            

            Guid itemFID = Guid.NewGuid();
            if (ItemID != "")
            {
                itemFID = new Guid(ItemID);
            }
            mavadItem item = new mavadItem()
            {
                amount = amount,
                itemID = itemFID,
                mavadID = mavadFItem,
                description = description,
                mavaditemID = mid

            };
            dbcontext.mavadItems.Add(item);
            string lastIng = "";
            if (Request.Cookies["lastIng"] != null)
            {
                lastIng = Request.Cookies["lastIng"].Value;
            }
            Response.Cookies["lastIng"].Value = lastIng + mid + ",";

            dbcontext.SaveChanges();
            mavad mavad = dbcontext.mavads.SingleOrDefault(x => x.mavadID == mavadFItem);
            mavadItemVM model = new mavadItemVM()
            {
                title = mavad.title,
                amount = amount,
                calories = mavad.Calories,
                carbo = mavad.Carbohydrates,
                fat = mavad.Fats,
                protein = mavad.Proteins,
                ingID = mavad.mavadID.ToString(),
                mid = mid.ToString()

            };

            return PartialView("/Views/Shared/_mavadItemPartial.cshtml", model);
        }

        public ContentResult sendToServerByJS()
        {
            string pathString = "~/images";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string filename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                filename = RandomString(7) + hpf.FileName; ;
                string savedFileName = Path.Combine(Server.MapPath(pathString), filename);
                string savedFileNameThumb = Path.Combine(Server.MapPath(pathString), "0" + filename);
                hpf.SaveAs(savedFileName);

            }
            return Content(filename);
        }
        public ContentResult sendToServer(string srt)
        {

            string gu = Request.Cookies["gu"].Value as string;
            Guid id = new Guid(gu);
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);


            string pathString = "~/images/Items/" + rst.username;
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string filename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                filename = RandomString(7) + hpf.FileName;
                string savedFileName = Path.Combine(Server.MapPath(pathString), filename);
                string savedFileNameThumb = Path.Combine(Server.MapPath(pathString), "0" + filename);
                hpf.SaveAs(savedFileName);
                string imagesList = getCookie("images");
                imagesList += filename + ",";
                SetCookie(imagesList, "images");



            }
            return Content(filename);

        }
        public void removeImage(string id)
        {
            string pathString = "~/images";
            string savedFileName = Path.Combine(Server.MapPath(pathString), id);
            System.IO.File.Delete(savedFileName);
            string imagesList = getCookie("images");
            if (imagesList != null)
            {
                imagesList = imagesList.Replace(id + ",", "");
            }

            SetCookie(imagesList, "images");
        }
        public void deleteImage(string id)
        {
            string pathString = "~/images";
            string savedFileName = Path.Combine(Server.MapPath(pathString), id);
            //System.IO.File.Delete(savedFileName);

        }
        [HttpPost]
        public ActionResult addItem(addItemVM model)
        {
            string ITEMID = model.itemID;
            string imagesList = getCookie("images");
            string itemmavadList = getCookie("lastIng");

            if (!String.IsNullOrEmpty(model.itemID) && model.itemID != "00000000-0000-0000-0000-000000000000")
            {
                Guid itemID = new Guid(model.itemID);
                Item ITE = dbcontext.items.SingleOrDefault(x => x.itemID == itemID);
                ITE.imageList = imagesList;
                ITE.price = double.Parse(model.price);
                ITE.desctiption = model.description;
                ITE.ingredient = model.ingredient;
                ITE.title = model.title;
                ITE.Fats = decimal.Parse(model.fat);
                ITE.Carbohydrates = decimal.Parse(model.hyd);
                ITE.Proteins = decimal.Parse(model.prot);
                ITE.calory = decimal.Parse(model.cal);
                ITE.Instruction = model.instruction;

            }
            else
            {
                Guid GUIDITEM = Guid.NewGuid();
                ITEMID = GUIDITEM.ToString();
                Item item = new Item()
                {
                    categoryID = new Guid(model.catid),
                    desctiption = model.description,
                    imageList = imagesList,
                    ingredient = model.ingredient,
                    itemID = GUIDITEM,
                    price =double.Parse(model.price),
                    title = model.title,
                    Calories = decimal.Parse(model.cal),
                    Fats = decimal.Parse(model.fat),
                    Proteins = decimal.Parse(model.prot),
                    Carbohydrates = decimal.Parse(model.hyd),
                    time = Int32.Parse(model.Time),
                    Instruction = model.instruction


                };


                dbcontext.items.Add(item);

                List<string> mavadIDlist = itemmavadList.Trim(',').Split(',').ToList();
                foreach (var mitem in mavadIDlist)
                {
                    Guid mavadIDs = new Guid(mitem);
                    mavadItem selectedItem = dbcontext.mavadItems.SingleOrDefault(x => x.mavaditemID == mavadIDs);
                    selectedItem.itemID = GUIDITEM;
                }

            }
            SetCookie("", "images");
            dbcontext.SaveChanges();
            SetCookie("", "lastIng");
            return RedirectToAction("meals", "Management");

        }


        public ActionResult DeleteIngredient(string id)
        {
            Guid catid = new Guid(id);
            mavad item = dbcontext.mavads.SingleOrDefault(x => x.mavadID == catid);
            if (item != null)
            {

                dbcontext.mavads.Remove(item);
                dbcontext.SaveChanges();
            }
            return Content("0");
        }
        public ActionResult DeleteCategory(string id)
        {
            Guid catid = new Guid(id);
            Item item = dbcontext.items.SingleOrDefault(x => x.categoryID == catid);
            if (item == null)
            {
                category cat = dbcontext.categories.SingleOrDefault(x => x.categoryID == catid);
                dbcontext.categories.Remove(cat);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("categories", "Management");
        }
        public ActionResult DeleteItem(string item)
        {
            string gu = Request.Cookies["gu"].Value as string;
            Guid id = new Guid(gu);

            Guid myitemID = new Guid(item);
            Item model = dbcontext.items.SingleOrDefault(x => x.itemID == myitemID);
            Item myitem = dbcontext.items.SingleOrDefault(c => c.itemID == myitemID);

            dbcontext.items.Remove(myitem);
            dbcontext.SaveChanges();
            return RedirectToAction("index", "Home");
        }


        public ActionResult inventory()
        {


            qrVM model = new qrVM();
            return View(model);
        }
        public ActionResult updateInventoryList(string query, int page)
        {
            string gu = Request.Cookies["gu"].Value as string;
            Guid resgu = new Guid(gu);
            int skip = page >= 0 ? (page - 1) * 12 : 0;

            var lst = dbcontext.mavads.Where(x => x.restaurantID == resgu);
            if (!String.IsNullOrEmpty(query))
            {
                lst = lst.Where(x => x.title.Contains(query));
            }
            List<mavad> final = lst.ToList();
            return PartialView("/Views/Shared/_Mavad.cshtml", final);
        }
        public ActionResult addInventory(string name, decimal cal, decimal carb, decimal fat, decimal prot)
        {
            string gu = Request.Cookies["gu"].Value as string;
            Guid resgu = new Guid(gu);
            mavad iteminventory = new mavad();
            Guid mavadID = Guid.NewGuid();
            iteminventory.title = name;
            iteminventory.Carbohydrates = carb;
            iteminventory.Calories = cal;
            iteminventory.Fats = fat;
            iteminventory.Proteins = prot;

            iteminventory.restaurantID = resgu;
            iteminventory.mavadID = mavadID;
            iteminventory.mojoodi = 0;
            iteminventory.type = "";
            dbcontext.mavads.Add(iteminventory);
            dbcontext.SaveChanges();
            return Content("200");
        }

        public ActionResult addInventoryAmount(string id, decimal amount)
        {
            Guid mavadID = new Guid(id);
            mavad mavad = dbcontext.mavads.SingleOrDefault(x => x.mavadID == mavadID);
            decimal currentAmount = mavad.mojoodi;
            mavad.mojoodi = currentAmount + amount;
            dbcontext.SaveChanges();
            return Content("200");
        }
        public void RemoveInventory(string id)
        {
            Guid newid = new Guid(id);
            mavad plc = dbcontext.mavads.SingleOrDefault(x => x.mavadID == newid);
            dbcontext.mavads.Remove(plc);
            dbcontext.SaveChanges();
        }

        public ActionResult Qr()
        {


            qrVM model = new qrVM();
            return View(model);
        }
        public ActionResult updateQrList(string query, int page)
        {
            string gu = Request.Cookies["gu"].Value as string;
            Guid resgu = new Guid(gu);
            int skip = page >= 0 ? (page - 1) * 12 : 0;

            var lst = dbcontext.places.Where(x => x.restaurantID == resgu);
            if (!String.IsNullOrEmpty(query))
            {
                lst = lst.Where(x => x.title.Contains(query));
            }
            List<place> final = lst.OrderBy(x => x.date).Skip(skip).Take(12).ToList();
            return PartialView("/Views/Shared/_QR.cshtml", final);
        }
        public string createQR(string url)
        {
            string username = "oc";// Request.Cookies["or"].Value as string;
            string pathString = "~/images/Qr/" + username;
            string filename = RandomString(5) + ".png";
            string finalPath = Path.Combine(Server.MapPath(pathString), filename);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(9);
            qrCodeImage.Save(finalPath, ImageFormat.Png);


            //QRCodeWriter.CreateQrCode(url, 200, QRCodeWriter.QrErrorCorrectionLevel.Medium).SaveAsPng(finalPath);
            finalPath = merg(finalPath);
            return finalPath;
        }

        public string merg(string url)
        {

            string pathString = "~/images/app";
            string tempPath = Path.Combine(Server.MapPath(pathString), "sample.jpg");
            string finalPath = Path.Combine(Server.MapPath(pathString), "final.png");

            using (Image image = Image.FromFile(finalPath))
            using (Image watermarkImage = Image.FromFile(url))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
            {
                int x = (image.Width / 2 - watermarkImage.Width / 2);
                int y = (image.Height / 2 - watermarkImage.Height / 2 + 76);
                watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                url = url.Replace("Qr", "Qr2");
                image.Save(url);
            }
            return url;
        }
        public void RemoveQR(string id)
        {
            Guid newid = new Guid(id);
            place plc = dbcontext.places.SingleOrDefault(x => x.placeID == newid);
            dbcontext.places.Remove(plc);
            dbcontext.SaveChanges();
        }

        public ActionResult getRequest()
        {
            string gu = Request.Cookies["gu"].Value as string;
            Guid rstID = new Guid(gu);
            List<place> plc = dbcontext.places.Where(c => c.restaurantID == rstID && c.calling == 1).ToList();
            return PartialView("~/Views/Shared/_garconRequest.cshtml", plc);
        }
        public ActionResult addQr(string name)
        {
            string gu = Session["gu"].ToString();// Request.Cookies["gu"].Value as string;
            Guid resgu = new Guid(gu);

            List<restaurant> lst = dbcontext.restaurants.ToList();

            place place = new place();
            place.title = name;
            Guid placeID = Guid.NewGuid();
            string url = ConfigurationManager.AppSettings["domain"]+ "Home/index?gu=" + gu + "&tb=" + placeID.ToString();
            place.url = url;
            place.image = createQR(url);
            place.placeID = placeID;
            place.restaurantID = resgu;
            place.date = DateTime.Now;
            dbcontext.places.Add(place);
            dbcontext.SaveChanges();
            return Content("200");
        }

        public void downloadQr(string id)
        {

            Guid placid = new Guid(id);
            place plc = dbcontext.places.SingleOrDefault(x => x.placeID == placid);
            string image = plc.image;
            string filename = plc.title + ".png"; System.IO.Path.GetFileName(image);



            Response.ContentType = "image/jpeg";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", filename));
            Response.ContentEncoding = Encoding.UTF8;
            Response.WriteFile(image);
            Response.HeaderEncoding = Encoding.UTF8;
            Response.Flush();
            Response.End();
        }


        public void verifyComment(string id)
        {
            Guid placeid = new Guid(id);
            place plc = dbcontext.places.SingleOrDefault(x => x.placeID == placeid);
            plc.calling = 0;
            plc.message = "";
            dbcontext.SaveChanges();

        }

    }





}