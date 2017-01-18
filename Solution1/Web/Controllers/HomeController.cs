﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["hostName"] = Request.Host;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
