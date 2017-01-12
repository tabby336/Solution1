using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.IO;

using Business.Services.Interfaces;
using Business.Services;

namespace Web.Controllers
{
    public class HomeworkController : Controller
    {
        private IHomeworkService _homeworkService;

        public HomeworkController(IHomeworkService service)
        {
            _homeworkService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IList<IFormFile> files, string uid, string mid, string obs)
        {
            ViewData["Message"] += _homeworkService.Upload(files, uid, mid, obs);   
            return View("Index");
        }
    }
}