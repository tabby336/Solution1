using Business.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
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
            if (uid != null) 
            {
                ViewData["Marks"] = _markService.FilterMarksByUser(uid);
            }
            return View();
        }


        
    }
}