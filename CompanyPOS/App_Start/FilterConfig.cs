﻿using CompanyPOS.Classes;
using System.Web;
using System.Web.Mvc;

namespace CompanyPOS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
			//filters.Add(new UseSSLAttribute());
	       }
    }
}
