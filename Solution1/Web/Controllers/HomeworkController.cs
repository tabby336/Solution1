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

        public IActionResult Upload()
        {
            return View("Upload");
        }

        [HttpPost]
        public IActionResult Upload(IList<IFormFile> files, string uid, string mid, string obs)
        {
            if (uid == null)
            {
                uid = this.GetLoggedInUserId();
                if(uid == null)
                    return NotFound();
            }
            IUpload uploadHelper = new Upload(new FileDataSource());
            try
            {
                ViewData["Message"] += _homeworkService.Upload(uploadHelper, files, uid, mid, obs);
            } 
            catch
            {
                ViewData["Message"] = "Something went wrong.";
            }   
            return View("Upload");
        }

        public IActionResult Download(string uid = null, string mid = null)
        {
            if (uid == null)
            {
                uid = this.GetLoggedInUserId();
                if(uid == null)
                    return NotFound();
            }
            if(uid != null && mid != null)
            {
                string path = _homeworkService.Archive(uid, mid);
                var redirectUrl = string.Format(@"/UpDown/Download?path={0}", path);
                return Redirect(redirectUrl);

            }
            return View("Download");
        }
    }
}