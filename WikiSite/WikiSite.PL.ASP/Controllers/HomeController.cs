﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WikiSite.PL.ASP.Classes;

namespace WikiSite.PL.ASP.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //FormsAuthentication.SignOut();
            this.CatchAlert();
            return View();
        }
    }
}