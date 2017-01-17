using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Authorize]
    public class UpDownController : Controller
    {
        public IActionResult Download(string path)
        {
            try
            {
                var name = Path.GetFileName(path);
                var fileType = MimeMapping.MimeTypes.GetMimeMapping(path);

                var result = new FileContentResult(System.IO.File.ReadAllBytes(path), fileType)
                {
                    FileDownloadName = name
                };

                HttpContext.Response.ContentType = fileType;
                return result;
            }
            catch (Exception)
            {

                return NotFound();
            }
            
        }
    }
}
