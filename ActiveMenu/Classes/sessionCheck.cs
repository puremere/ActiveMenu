using ActiveMenu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveMenu.Classes;
using System.Web.Routing;

namespace ActiveMenu.Classes
{
    public class sessionCheck : ActionFilterAttribute
    {

    }
    public class CoreSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            string[] lst = { "Login", "login" };
            if (!lst.Contains(actionName))
            {

                if (session["gu"] != null || filterContext.HttpContext.Request.Cookies["AT"] != null)
                {
                    string token = "";
                    if (session["gu"] == null)
                    {
                        var request = filterContext.HttpContext.Request.Cookies["AT"].Value;
                        session["gu"] = request.ToString();
                        token = request.ToString();

                    }
                    else
                    {
                        filterContext.HttpContext.Response.Cookies["AT"].Value = session["gu"].ToString();
                    }

                   
                   
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary {
                                    { "Controller", "Management" },
                                    { "Action", "Login" }
                                  });
                }
            }


            //if (session["logedInUser"] != null || filterContext.HttpContext.Request.Cookies["AT"] != null)
            //{
            //    string token = "";
            //    if (session["logedInUser"] == null)
            //    {
            //        var request = filterContext.HttpContext.Request.Cookies["AT"].Value;
            //        session["logedInUser"] = request.ToString();
            //        token = request.ToString(); 

            //    }
            //    else
            //    {
            //        token = session["logedInUser"].ToString();
            //    }
            //    string[] lst = { "dashbaord", "ingredients", "tables" };
            //    if (lst.Contains(actionName))
            //    {
            //        //string result = "";
            //        //using (Context dbcontext = new Context())
            //        //{
                       
            //        //    if (dbcontext.users.Any(x=>x.token == token))
            //        //    {
            //        //        user myuser = (dbcontext.users.SingleOrDefault(x => x.token == token));

            //        //        var role = from u in  user  
            //        //                   join ur in userRole on u.userID equals ur.userID
            //        //                   join ra in roleAccess on ur.


            //        //    }
            //        //}
            //        //if (model.status != "200")
            //        //{
            //        //    filterContext.Result = new RedirectToRouteResult(
            //        //    new RouteValueDictionary {
            //        //                    { "Controller", "admin" },
            //        //                    { "Action", "Index" }
            //        //                   });
            //        //}

            //    }

            //}
            //else
            //{

            //    //filterContext.Result = new RedirectToRouteResult(
            //    //new RouteValueDictionary {
            //    //                { "Controller", "Admin" },
            //    //                { "Action", "Index" }
            //    //            });

            //}

        }
    }
}