using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MovieClub.App_Start;

namespace MovieClub
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // this is for the AutoMapper
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());


            // this is for the Web API
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
