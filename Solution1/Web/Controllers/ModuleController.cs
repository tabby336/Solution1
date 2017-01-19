

using Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ModuleViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class ModuleController: Controller
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [Authorize(Roles = "Student, Professor")]
        public IActionResult Index(string moduleId = null)
        {
            if(moduleId == null)
                return NotFound();

            var module = _moduleService.GetModule(moduleId);
            var model = new ModuleViewModel()
            {
                Module = module
            };
            return View("Module", model);
        }

        [Authorize(Roles = "Student, Professor")]
        public IActionResult GetPdf(string id = null)
        {
            if (id == null)
                return NotFound();

            var filePath = _moduleService.GetPdfPathForModule(id);
            if (filePath == null)
                return NotFound();

            var redirectUrl = string.Format(@"/UpDown/Download?path={0}", filePath);
            return Redirect(redirectUrl);
        }
    }
}
