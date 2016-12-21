using System;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Web.Controllers.ApiController
{
    public class AnouncementController : Controller
    {
        private readonly IAnouncementService _anouncementService;

        public AnouncementController(IAnouncementService anouncementService)
        {
            _anouncementService = anouncementService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var anouncements = _anouncementService.GetAllAnouncements();
            return Ok(anouncements);
        }

        [HttpGet]
        [Route("api/Anouncement/GetAllByCourse")]
        public IActionResult GetAllByCourse(Guid courseId)
        {
            var anouncements = _anouncementService.GetAllByCourse(courseId);
            return Ok(anouncements);
        }

        [HttpGet]
        [Route("api/Anouncement/GetAllForPeriod")]
        public IActionResult GetAllForPeriod(DateTime fromDate, DateTime toDate)
        {
            var anouncements = _anouncementService.GetAllByPeriod(fromDate, toDate);
            return Ok(anouncements);
        }

        [HttpGet]
        [Route("api/Anouncement/GetAllFilteredByPeriodAndCourse")]
        public IActionResult GetAllFilteredByPeriodAndCourse(DateTime fromDate, DateTime toDate, Guid courseId)
        {
            var anouncements = _anouncementService.GetAllFilteredByCourseAndPeriod(fromDate, toDate, courseId);
            return Ok(anouncements);
        }
    }
}
