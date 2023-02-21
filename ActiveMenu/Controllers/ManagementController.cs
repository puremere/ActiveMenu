using ActiveMenu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using ActiveMenu.Model;
using ActiveMenu.ViewModel;
using ActiveMenu.Classes;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;

namespace ActiveMenu.Controllers
{
    [CoreSessionCheck]
    public class ManagementController : Controller
    {
        private static Random random = new Random();
        Model.Context dbcontext = new Model.Context();
        // GET: Management
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
        public ActionResult Index()
        {
            return View();
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

      
        public ActionResult Login(string error)
        {


            if (error != null)
            {
                ViewBag.error = error;
            }
           


            //List<Item> orderlddist1 = dbcontext.items.ToList();
            //dbcontext.items.RemoveRange(orderlddist1);

            //List<category> orderlddist2 = dbcontext.categories.ToList();
            //dbcontext.categories.RemoveRange(orderlddist2);

            //List<place> orderlddist3 = dbcontext.places.ToList();
            //dbcontext.places.RemoveRange(orderlddist3);


            //List<order> orderlddist4 = dbcontext.orders.ToList();
            //dbcontext.orders.RemoveRange(orderlddist4);


            //List<mavadItem> orderlddist5 = dbcontext.mavadItems.ToList();
            //dbcontext.mavadItems.RemoveRange(orderlddist5);





            //dbcontext.SaveChanges();
            return View();
        }
        public ActionResult dashbaord()
        {
            string gu = Session["gu"].ToString();
            Guid regu = new Guid(gu);
            List<double> pricestring = new List<double>();
            string monthstring = "";
            List<chartVM> pricelist = dbcontext.orders.Where(x => x.restaurantID == regu).GroupBy(c => c.orderTime.Month)
                .Select(g => new chartVM
                {
                    price = g.Sum(s => s.price),
                    title = g.Select(x => x.orderTime).Max().Month.ToString()
                }).ToList();


            List<chartVM> newpricelist = new List<chartVM>();
            for (int i = 1; i < 13; i++)
            {
                string srttitle = i.ToString();
                newpricelist.Add(new chartVM()
                {
                    title = i.ToString(),
                    price = pricelist.Where(x => x.title == srttitle).Sum(x => x.price)
                });
            }
            foreach (var item in newpricelist)
            {

                pricestring.Add(item.price);
                monthstring += item.title.ToString() + ",";

            }
            dashbaordAdminVM model = new dashbaordAdminVM()
            {
                pricesrt = pricestring,
                monthstring = monthstring
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {



            List<user> userlist = dbcontext.users.ToList();

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
                else if (loginmodel.roleID == "Kichen")
                {
                    Session["logedInUser"] = "kitchen";
                    return RedirectToAction("dashboard", "kitchen");
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

        public ActionResult ingredients(string q)
        {
            string gu = Session["gu"].ToString();
            Guid rID = new Guid(gu);


            var mllst = dbcontext.mavads.AsEnumerable();

            //var students = dbcontext.mavads.GroupBy(s => new { s.title, s.Proteins, s.Carbohydrates , s.Fats, s.Calories}).SelectMany(grp => grp.OrderBy(x=>x.title).Skip(1)); ;
            //dbcontext.mavads.RemoveRange(students);
            //dbcontext.SaveChanges();
            //List<mavad> mavads = dbcontext.mavads.ToList();
            
            //foreach(var item in mavads)
            //{

            //    item.restaurantID = rID;
            //}
            //dbcontext.SaveChanges();
                
            if (q != null)
            {
                mllst = mllst.Where(x => x.title.Contains(q));
            }

            List<mavad> ml = mllst.OrderBy(x => x.title).ToList();
            return View(ml);
        }
        public ActionResult tables(string q)
        {
            string gu = Session["gu"].ToString();
            Guid rID = new Guid(gu);


            var mlq = dbcontext.places.Where(x => x.restaurantID == rID);
            if (q != null)
            {
                mlq = mlq.Where(x => x.title.Contains(q));

            }

            List<place> ml = mlq.ToList();

            return View(ml);
        }
        public ActionResult getReciet(string id)
        {
            string mystring = "";
            //CreatePDFFromHTMLFile(id);
            string fn = CreatePDFFromHTMLFile(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fn);
            string fileName = "invoice.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private string CreatePDFFromHTMLFile(string id)
        {

            string name = RandomString(5) + ".pdf";
            
            string srt = Path.Combine(Server.MapPath("/file/"), name);
            System.IO.FileStream fs = new FileStream(srt, FileMode.Create);
            //Response.ContentType = "application/pdf";

            //Response.AddHeader("content-disposition", "attachment;filename=test_22.pdf");

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);



            string imageFilePath = Server.MapPath("/images/app/Invoice.png")  ;

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

            string path = Path.Combine(Server.MapPath("/images/Logo/"+ Request.Cookies["username"].Value+"/"), rst.logo);

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
                if ((i+1)%2 == 0)
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

                fullname = new PdfPCell(new Phrase((item.itemprice * item.itemCount).ToString() + " AED",blackListTextFont));
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
        public ActionResult categories()
        {
            if (Session["logedInUser"] == null)
            {
                //return RedirectToAction("Login");
            }
            string gu = Session["gu"].ToString();
            //string gu = Session["logedInUser"].ToString();



            Guid resturanGUID = new Guid(gu);
            List<category> lst = dbcontext.categories.Where(x => x.restaurantID == resturanGUID).ToList();
            foreach (var item in lst)
            {
                Item firstItem = dbcontext.items.FirstOrDefault(x => x.categoryID == item.categoryID);
                string imgaddress = "";
                if (firstItem != null)
                {
                    imgaddress = firstItem.imageList.Split(',').First();
                }

                item.imageAddress = imgaddress;
            }

            Guid id = new Guid(gu);
            Response.Cookies["gu"].Value = gu;


            List<restaurant> lts = dbcontext.restaurants.ToList();

            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);

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
            indexVM model = new indexVM()
            {
                calist = lst,
                image = rst.imageAddress,
                title = rst.title,
                garson = rst.garson
            };

            return View(model);
        }
        public ActionResult meals(Guid? id, string q)
        {
            string gu = Session["gu"].ToString();
            Guid resturanguid = new Guid(gu);

            ViewBag.Message = "Your application description page.";
            List<Item> itemList = new List<Item>();
            if (id != null)
            {
                category cat = dbcontext.categories.SingleOrDefault(x => x.categoryID == id);

                var nlistvar = dbcontext.items.Where(x => x.categoryID == cat.categoryID);
                if (q != null)
                {
                    nlistvar = nlistvar.Where(c => c.title.Contains(q));
                }
                itemList = nlistvar.ToList();
            }
            else
            {

                var lstvar = dbcontext.categories.Where(x => x.restaurantID == resturanguid);

                foreach (var item in lstvar.ToList())
                {
                    var nlistvar = dbcontext.items.Where(x => x.categoryID == item.categoryID);
                    if (q != null)
                    {
                        nlistvar = nlistvar.Where(c => c.title.Contains(q));
                    }
                    List<Item> nlist = nlistvar.ToList();
                    foreach (var meal in nlist)
                    {
                        itemList.Add(meal);
                    }
                }

            }

            List<category> catlist = dbcontext.categories.Where(x => x.restaurantID == resturanguid).ToList();

            detailVM model = new detailVM()
            {
                name = "",
                id = "",
                itemList = itemList,
                catList = catlist
            };
            ViewBag.username = Request.Cookies["username"].Value as string;
            string istrue = "";
            if (Request.Cookies["or"] != null)
            {
                istrue = "1";
            }

            ViewBag.isTrue = istrue;
            if (Request.Cookies["pardakht"] != null)
            {
                ViewBag.pardakht = Request.Cookies["pardakht"].Value;
            }
            else
            {
                ViewBag.pardakht = "0";
            }
            if (Request.Cookies["sabad"] != null)
            {
                ViewBag.pardakht = Request.Cookies["sabad"].Value;
            }
            else
            {
                ViewBag.sabad = "0";
            }

           

            return View(model);
        }
        public ActionResult orderDetail(string id)
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
                    name = "سفارش شماره : " + order.orderNumber.ToString()
                };



            }


            return View(model);
        }
        public ActionResult createItem(string item, string catid)
        {
            if (Request.Cookies["lastIng"] != null )
            {
              string  inglist = Request.Cookies["lastIng"].Value;
                List<string> lst = inglist.Trim(',').Split(',').ToList();
                if (lst.Count() > 0)
                {
                    foreach(var mitt in lst)
                    {
                        if (mitt != "")
                        {
                            Guid mittgu = new Guid(mitt);
                            mavadItem mai = dbcontext.mavadItems.SingleOrDefault(x => x.mavaditemID == mittgu);
                            if (mai != null)
                            {
                                dbcontext.mavadItems.Remove(mai);

                            }

                        }
                       
                    }
                    dbcontext.SaveChanges();
                }
            }

            Response.Cookies["lastIng"].Value = "";
            string gu = Session["gu"].ToString();
            Guid id = new Guid(gu);
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == id);
            List<category> catlist = dbcontext.categories.Where(x => x.restaurantID == id).ToList();
            ViewModel.CreateItemVM model = new ViewModel.CreateItemVM();
            model.catlist = catlist;
            model.itemID = item == null ? "" : item;
            if (String.IsNullOrEmpty(item))
            {
                string imagesList = getCookie("images");
                if (imagesList != null)
                {
                    if (imagesList != "")
                        ViewBag.images = imagesList.Trim(',');
                }
                string inglist = "";
                
                
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
                                                   calories = m.Calories,
                                                   carbo = m.Carbohydrates,
                                                   fat = m.Fats,
                                                   protein = m.Proteins,
                                                   mid = mi.mavaditemID.ToString()

                                               }).ToList();
                model.itemMavad = mavadList.OrderBy(x => x.title).ToList();
            }
            ViewBag.username = rst.username;
            model.username = rst.username;

            model.catItem = catid;
            model.lstMavad = dbcontext.mavads.OrderBy(x => x.title).ToList();
            //Guid itemguid = new Guid(item);

            return View(model);
        }
        public ActionResult setting()
        {
            string gusrt = Session["gu"].ToString();
            Guid gu = new Guid(gusrt);
            restaurant rst = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == gu);
            return View(rst);
        }
        [HttpPost]
        public ActionResult setting(settingVM model)
        {


            model.oppening = model.oppeningD.Hour;
            model.closing = model.closingD.Hour;
            string gusrt = Session["gu"].ToString();
            Guid gu = new Guid(gusrt);

            restaurant restu = dbcontext.restaurants.SingleOrDefault(x => x.restaurantID == gu);
            restu.Address = model.address;
            restu.brandName = model.brandName;
            restu.colsing = model.closingD;
            restu.oppening = model.oppeningD;
            restu.phone = model.phoneNumber;
            restu.whatsappPhone = model.whatsappNumber;
            restu.mobile = model.mobileNumber;
            restu.email = model.email;
            restu.vat = model.vat;
            restu.phone = model.phoneNumber;
            dbcontext.SaveChanges();
            string lastfile = restu.logo;
            string username = Request.Cookies["username"].Value;
            if (model.imagefile != null)
            {
                string newlogo = model.imagefile.FileName;
                restu.logo = newlogo;
                dbcontext.SaveChanges();
                Response.Cookies["logo"].Value = newlogo;
                string lastfinalpath = Path.Combine(Server.MapPath("/images/Logo/" + username + "/"), newlogo);
                if (System.IO.File.Exists(lastfinalpath))
                {
                    System.IO.File.Delete(lastfinalpath);
                }
            }

            if (model.imagefile != null)
            {
                var fileName = Path.GetFileName(model.imagefile.FileName);

                var path = Path.Combine(Server.MapPath("/images/Logo/" + username + "/"), fileName);
                model.imagefile.SaveAs(path);
            }

            return RedirectToAction("dashbaord");
        }

        public ActionResult orders(string tb)
        {
            string orderPeigiry = "";

            string gu = Session["gu"].ToString();
            Guid tableID = new Guid(tb);
            place place = dbcontext.places.SingleOrDefault(x => x.placeID == tableID);
            Guid resgu = new Guid(gu);
            List<order> orders = dbcontext.orders.Where(x => x.restaurantID == resgu && x.final != 1 && x.placeID == tableID).ToList();
            foreach (var ord in orders)
            {

                if (ord != null)
                {
                    TimeSpan timspan = DateTime.Now.Subtract(ord.orderTime);
                    ord.minutPassed = timspan.TotalMinutes;

                }

            }

            orderVM model = new orderVM();
            model.tb = tb;
            model.orderlist = orders;
            return View(model);
        }

        [HttpPost]
        public ActionResult orders(string tb, int? status, DateTime? dateFrom, DateTime? dateTo)
        {
            string orderPeigiry = "";

            string gu = Session["gu"].ToString();
            Guid tableID = new Guid(tb);
            place place = dbcontext.places.SingleOrDefault(x => x.placeID == tableID);
            Guid resgu = new Guid(gu);
            var ordersvar = dbcontext.orders.Where(x => x.restaurantID == resgu && x.placeID == tableID);
            List<order> lss = ordersvar.ToList();
            if (status != -1)
            {
                ordersvar = ordersvar.Where(x => x.final == status);
            }
            if (dateFrom != null)
            {
                ordersvar = ordersvar.Where(x => x.orderTime >= dateFrom);
            }
            if (dateTo != null)
            {
                ordersvar = ordersvar.Where(x => x.orderTime <= dateTo);
            }

            List<order> orders = ordersvar.ToList();
            foreach (var ord in orders)
            {

                if (ord != null)
                {
                    TimeSpan timspan = DateTime.Now.Subtract(ord.orderTime);
                    ord.minutPassed = timspan.TotalMinutes;

                }

            }

            orderVM model = new orderVM();
            model.tb = tb;
            model.orderlist = orders;
            return View(model);
        }
        public ActionResult DisableItem(string item)
        {
            string gu = Request.Cookies["gu"].Value as string;
            Guid id = new Guid(gu);

            Guid myitemID = new Guid(item);
            Item model = dbcontext.items.SingleOrDefault(x => x.itemID == myitemID);
            model.isDisabled = 1;
            dbcontext.SaveChanges();
            return Content("");
        }
        public ActionResult EableItem(string item)
        {
            string gu = Request.Cookies["gu"].Value as string;
            Guid id = new Guid(gu);

            Guid myitemID = new Guid(item);
            Item model = dbcontext.items.SingleOrDefault(x => x.itemID == myitemID);
            model.isDisabled = 0;
            dbcontext.SaveChanges();
            return Content("");
        }
        


    }



}