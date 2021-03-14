using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebToanHoc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                   name: "Home Index",
                   url: "trang-chu",
                   defaults: new { controller = "Home", action = "Index" },
                   namespaces: new[] { "WebToanHoc.Controllers" }
               );
            routes.MapRoute(
                  name: "Home Detail_Document",
                  url: "{metatitle}/{id}",
                  defaults: new { controller = "Home", action = "Detail_Document", id = UrlParameter.Optional },
                  namespaces: new[] { "WebToanHoc.Controllers" }
              );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                   name: "Home Category_Document",
                   url: "danh-muc/{metatitle}/{id}",
                   defaults: new { controller = "Home", action = "Category_Document", id = UrlParameter.Optional },
                   namespaces: new[] { "WebToanHoc.Controllers" }
               );
            
            routes.MapRoute(
                   name: "Default",
                   url: "{controller}/{action}/{id}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   namespaces: new[] { "WebToanHoc.Controllers" }
               );
          }
    }
}
