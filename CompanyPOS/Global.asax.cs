﻿
using DATA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CompanyPOS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CompanyPosDBContext>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<CompanyPosDBContext>());

            //Run this when you need add some change 
            //Database.SetInitializer(new CompanyPosDBContextSeeder());
            var config = GlobalConfiguration.Configuration;
   
			Database.SetInitializer<CompanyPosDBContext>(null);
		}
    }
}
