using System;
using System.Collections.Generic;
using System.Linq;
using Business.Services.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models.CourseViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IPlayerService _playerService;

        public CourseController(ICourseService courseService, IPlayerService playerService)
        {
            _courseService = courseService;
            _playerService = playerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _courseService.GetAllCourses(true);
            var myId = this.GetLoggedInUserId();
            var myCourses = _courseService.GetCoursesForPlayer(myId);
            var cvm = new CourseViewModel() {Courses = courses.ToList(), MyCourses = myCourses.ToList()};
            return View("Courses",  cvm);
        }

        [HttpGet]
        public IActionResult GetMine()
        {
            var myId = this.GetLoggedInUserId();
            var courses = _courseService.GetCoursesForPlayer(myId);
            var enumerable = courses as IList<Course> ?? courses.ToList();
            var cvm = new CourseViewModel() { Courses = enumerable.ToList(), MyCourses = enumerable.ToList()};
            return View("Courses", cvm);
        }

        [HttpGet]
        public IActionResult GetCoursesByAuthor(string author)
        {
            var courses = _courseService.GetCoursesForAuthor(author);
            var enumerable = courses as IList<Course> ?? courses.ToList();
            var cvm = new CourseViewModel() { Courses = enumerable.ToList(), MyCourses = enumerable.ToList()};
            return View("Courses", cvm);
        }

        [HttpGet]
        [Authorize(Roles = "Professor")]
        public IActionResult CreateCourse()
        {
            return View("CreateCourse", new CreateCourseViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Professor")]
        public IActionResult CreateCourse(IFormFile file, CreateCourseViewModel model)
        {
            if (!ModelState.IsValid) return View("CreateCourse", model);
            if (file == null)
            {
                ModelState.AddModelError(string.Empty, "You must upload an image for the Course.");
                return View("CreateCourse", model);
            }

            var myId = this.GetLoggedInUserId();
            var course = _courseService.CreateCourse(myId, model.Title, model.Description, model.HashTag, model.DataLink, new List<IFormFile> { file });

            if (course != null) //success
            {
                return RedirectToAction("GetMine");
            }
            
            ModelState.AddModelError(string.Empty, "Cannot create Course. Please try again later.");
            return View("CreateCourse", model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Image(string id = null)
        {
            if (id == null)
                return NotFound();

            var filePath = _courseService.GetImagePathForCourseId(id);
            if (filePath == null)
                return NotFound();

            var redirectUrl = string.Format(@"/UpDown/Download?path={0}", filePath);
            return Redirect(redirectUrl);
        }
        
        [Authorize(Roles = "Student")]
        public IActionResult Partikip(string courseId = null)
        {
            if (courseId == null)
                return NotFound();

            var myId = this.GetLoggedInUserId();
            try
            {
                _courseService.Partikip(myId, courseId);
            }
            catch(Exception e)
            {
                // here return view error
                return BadRequest(e.Message);
            }

            return RedirectToAction("GetMine");
        }
    }
}
