using Business.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class MarkController : Controller 
    {
        private readonly IMarkService _markService;
    
        public MarkController(IMarkService service) 
        {
            _markService = service;
        }

        public IActionResult Index(string uid = null)
        {
            ViewData["Message"] = "Trecem si noi anul asta?";
            var marks = _markService.FilterMarksByUser(uid);
            if (marks == null || marks.Count == 0) 
            {
                return NotFound();
            }
            ViewBag.Marks = marks;
            return View();
            
        }


        
    }
}