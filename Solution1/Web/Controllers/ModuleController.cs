

using Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Web.Models.ModuleViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class ModuleController : Controller
    {
        private readonly IModuleService _moduleService;
        private readonly ICourseService _courseService;

        public ModuleController(IModuleService moduleService, ICourseService courseService)
        {
            _moduleService = moduleService;
            _courseService = courseService;
        }

        [Authorize(Roles = "Student,Professor")]
        public IActionResult Index(string moduleId = null)
        {
            if (moduleId == null)
                return NotFound();

            var module = _moduleService.GetModule(moduleId);
            var model = new ModuleViewModel()
            {
                Module = module
            };
            return View("Module", model);
        }

        [HttpGet]
        [Authorize(Roles = "Professor")]
        public IActionResult CreateModule()
        {
            var myId = this.GetLoggedInUserId();
            var courses = _courseService.GetCoursesForPlayer(myId);
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            return View("CreateModule", new CreateModuleViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Professor")]
        public IActionResult CreateModule(IFormFile file, CreateModuleViewModel model)
        {
            // reassign courses
            var myId = this.GetLoggedInUserId();
            var courses = _courseService.GetCoursesForPlayer(myId);
            ViewBag.Courses = new SelectList(courses, "Id", "Title");

            if (!ModelState.IsValid) return View("CreateModule", model);
            if (file == null)
            {
                ModelState.AddModelError(string.Empty, "You must upload a file for the Module.");
                return View("CreateModule", model);
            }
            
            var module = _moduleService.CreateModule(myId, model.CourseId, model.Title, model.Description, new List<IFormFile> { file }, model.HasHomework, model.HasTest);

            if (module != null) //success
            {
                return RedirectToAction("Index", "Module", new { moduleId = module.Id.ToString()});
            }

            ModelState.AddModelError(string.Empty, "Cannot create Module. Please try again later.");
            return View("CreateModule", model);
        }

        [Authorize(Roles = "Student,Professor")]
        public IActionResult GetPdf(string id = null)
        {
            if (id == null)
                return NotFound();

            var filePath = _moduleService.GetPdfPathForModule(id);
            if (filePath == null)
                return NotFound();

            var redirectUrl = string.Format(@"/UpDown/Download?path={0}", filePath);
            return Redirect(redirectUrl);
        }

    }
}
