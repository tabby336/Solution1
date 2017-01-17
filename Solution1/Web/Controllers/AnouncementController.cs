using System;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models.AnouncementViewModel;

namespace Web.Controllers
{
    public class AnouncementController : Controller
    {
        private readonly IAnouncementService _anouncementService;

        public AnouncementController(IAnouncementService anouncementService)
        {
            _anouncementService = anouncementService;
        }

        public IActionResult Index()
        {
            var anouncements = _anouncementService.GetAllAnouncements();
            var anouncementViewModel = new AnouncementViewModel
            {
                Anouncements = anouncements
            };

            return View(anouncementViewModel);
        }
        [HttpGet]
        [Route("Anouncement/GetAllByCourse")]
        public IActionResult GetAllByCourse(Guid courseId)
        {
            var anouncements = _anouncementService.GetAllByCourse(courseId);
            return Ok(anouncements);
        }

        [HttpGet]
        [Route("Anouncement/GetAllForPeriod")]
        public IActionResult GetAllForPeriod(DateTime fromDate, DateTime toDate)
        {
            var anouncements = _anouncementService.GetAllByPeriod(fromDate, toDate);
            return Ok(anouncements);
        }

        [HttpGet]
        [Route("Anouncement/GetAllFilteredByPeriodAndCourse")]
        public IActionResult GetAllFilteredByPeriodAndCourse(DateTime fromDate, DateTime toDate, Guid courseId)
        {
            var anouncements = _anouncementService.GetAllFilteredByCourseAndPeriod(fromDate, toDate, courseId);
            return Ok(anouncements);
        }
    }
}
