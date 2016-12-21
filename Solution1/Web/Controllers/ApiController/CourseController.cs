using Business.Services.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.ApiController
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
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
