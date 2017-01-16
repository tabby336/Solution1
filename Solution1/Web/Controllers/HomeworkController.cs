using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.IO;

using Business.Services.Interfaces;
using Business.Services;
using Business.CommonInfrastructure;
using Business.CommonInfrastructure.Interfaces;

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
            IUpload uploadHelper = new Upload(new FileDataSource());
            try
            {
                ViewData["Message"] += _homeworkService.Upload(uploadHelper, files, uid, mid, obs);
            } 
            catch
            {
                ViewData["Message"] = "Something went wrong.";
            }   
            return View("Index");
        }
    }
}