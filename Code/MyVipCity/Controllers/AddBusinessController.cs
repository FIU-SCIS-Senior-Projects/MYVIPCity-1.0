﻿using System.Web.Mvc;

namespace MyVipCity.Controllers
{
	[Authorize(Roles = "Admin")]
    public class AddBusinessController : Controller
    {
        // GET: AddBusiness
        public ActionResult Index()
        {
            return View();
        }
    }
}