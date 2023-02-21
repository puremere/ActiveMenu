using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActiveMenu.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult index()
        {

            return View();
        }
        public ActionResult Error404()
        {

            return View();
        }
        public ActionResult Error500()
        {

            return View();
        }
        public ActionResult Error403()
        {

            return View();
        }
        public ActionResult Unknown()
        {

            return View();
        }
    }
}