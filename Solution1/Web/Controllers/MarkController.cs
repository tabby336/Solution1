using Business.Services.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class MarkController : Controller 
    {
        private readonly IMarkService _markService;
    
        public MarkController(IMarkService service) 
        {
            _markService = service;
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult Index(string uid = null)
        {
            if (uid == null)
            {
                uid = this.GetLoggedInUserId();
                if(uid == null)
                    return NotFound();
            }
            var marks = _markService.GetHumanReadableMarks(uid);
            ViewBag.Marks = marks;  
            return View("Marks");
            
        }       
    }
}