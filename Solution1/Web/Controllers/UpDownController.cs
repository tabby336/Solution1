

using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UpDownController : Controller
    {
        public IActionResult Download(string path)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                var name = Path.GetFileName(fullPath);
                var fileType = MimeMapping.MimeTypes.GetMimeMapping(fullPath);

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
