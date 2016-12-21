using Business.Services.Interfaces;
using DataAccess.Models;

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;
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
            if (uid == null) 
            {
                 return View();
            }
            IEnumerable<Mark> marks = _markService.FilterMarksByUser(uid);
            ViewBag.Marks = marks;  
            return View("Marks");
            
        }


        
    }
}