using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_PLANT_PPE
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new CorsHandler());
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "GET,POST");
            config.EnableCors(cors);
        }
    }
}
