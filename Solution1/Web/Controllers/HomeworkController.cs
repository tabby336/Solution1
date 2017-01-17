using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult Upload(string moduleId)
        {
            //Testing reasons, gets to be deleted
            if (moduleId == null)
            {
                moduleId = "1f0b4daa-3a7e-4a94-a5de-9dcf707d9ab4";
            }
            return View("Upload", moduleId);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult Upload(IList<IFormFile> files, string uid, string moduleId, string obs)
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
                ViewData["Message"] += _homeworkService.Upload(uploadHelper, files, uid, moduleId, obs);
            } 
            catch
            {
                ViewData["Message"] = "Something went wrong.";
            }   
            return View("Upload", moduleId);
        }

        [HttpGet]
        [Authorize(Roles = "Professor")]
        public IActionResult Download(string uid = null, string moduleId = null)
        {
            if (uid == null)
            {
                uid = this.GetLoggedInUserId();
                if(uid == null)
                    return NotFound();
            }
            if(uid != null && moduleId != null)
            {
                string path = _homeworkService.Archive(uid, moduleId);
                var redirectUrl = string.Format(@"/UpDown/Download?path={0}", path);
                return Redirect(redirectUrl);

            }
            return View("Download", moduleId);
        }
    }
}