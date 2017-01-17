using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            var cvm = new CourseViewModel() {Courses = courses.ToList()};
            return View("Courses",  cvm);
        }

        [HttpGet]
        //[Authorize(Roles = "Professor")]
        public IActionResult CreateCourse()
        {
            return View("CreateCourse");
        }

        [HttpPost]
        //[Authorize(Roles = "Professor")]
        public IActionResult CreateCourse(CreateCourseViewModel model)
        {
            if (!ModelState.IsValid) return View("CreateCourse", model);

            var id = this.GetLoggedInUserId();
            var me = _playerService.GetPlayerData(id);
            var courseFromData = new Course()
            {
                Title = model.Title,
                Description = model.Description,
                HashTag = model.HashTag,
                PhotoUrl = model.PhotoUrl,
                DataLink = model.DataLink,
                Author = me.FirstName + " " + me.LastName,
                TimeStamp = DateTime.Now
            };

            var course = _courseService.CreateCourse(courseFromData, me);

            if (course != null) //success
            {
                return View("CreateCourse", model);
            }


            ModelState.AddModelError(string.Empty, "Cannot create Course. Please try again later.");
            return View("CreateCourse", model);
        }

        [HttpGet]
        [Route("api/Course/GetCourseNames")]
        public IActionResult GetCourseNames()
        {
            var courseNames = _courseService.GetAllCourseNames();
            return Ok(courseNames);
        }

        [HttpGet]
        [Route("api/Course/GetAllCourses")]
        public IActionResult GetAllCourses()
        {
            var courses = _courseService.GetAllCourses();
            return Ok(courses);
        }

        [HttpPut]
        public IActionResult Update(Course course)
        {
            _courseService.UpdateCourse(course);
            return Ok();
        }

        [HttpDelete]
        public void Delete(Course course)
        {
            _courseService.DeleteCourse(course);
        }
    }
}
