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
using DataAccess.Models;

namespace Web.Controllers
{
    public class HomeworkController : Controller
    {
        private IHomeworkService _homeworkService;
        private IMarkService _markService;

        public HomeworkController(IHomeworkService hw, IMarkService mark)
        {
            _homeworkService = hw;
            _markService = mark;
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
                moduleId = "bade8051-f56d-4187-9726-8694c9ca6aef";
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
        public IActionResult Download(string playerId = null, string moduleId = null)
        {
            if(playerId != null && moduleId != null)
            {
                string path = _homeworkService.Archive(playerId, moduleId);
                var redirectUrl = string.Format(@"/UpDown/Download?path={0}", path);
                return Redirect(redirectUrl);

            }
            return Evaluate(moduleId);
        }

        [HttpGet]
        [Authorize(Roles = "Professor")]
        public IActionResult Evaluate(string moduleId)
        {
            //Testing reasons, gets to be deleted
            if (moduleId == null)
            {
                moduleId = "bade8051-f56d-4187-9726-8694c9ca6aef";
            }
            IEnumerable<Player> thatUploaded = _homeworkService.GetPlayersThatUploaded(moduleId);
            ViewBag.Players = thatUploaded;
            return View("Evaluate", moduleId);
        }

        [HttpPost]
        public IActionResult Mark(string playerId, string moduleId, string mark)
        {
            string creatorId = this.GetLoggedInUserId();
            if(creatorId == null)
                return NotFound();

            _markService.MarkHomework(moduleId, playerId, creatorId, mark);
            return Evaluate(moduleId);
        }
    }
}