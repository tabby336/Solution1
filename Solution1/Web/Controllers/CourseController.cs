using System.Linq;
using Business.Services.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.CourseViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _courseService.GetAllCourses(true);
            var cvm = new CourseViewModel() {Courses = courses.ToList()};
            return View("Courses",  cvm);
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

        [HttpPost]
        public IActionResult Create(Course course)
        {
            _courseService.CreateCourse(course);
            return Ok();
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
