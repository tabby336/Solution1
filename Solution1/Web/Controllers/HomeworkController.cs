using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

using Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Authorize]
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