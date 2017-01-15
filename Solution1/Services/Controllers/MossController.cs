using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Business.MossService;
using Business.MossService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models.MossViewModel;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    public class MossController : Controller
    {
    
        // GET: /Moss
        [HttpGet]
        public IActionResult Index()
        {
            return View();
          //  return "Not Implemented View";
        }

        [HttpPost]
        public IActionResult SendRequest(
            [FromForm]SendRequestViewModel mossModel, 
            [FromServices]IMossService mossService, 
            [FromServices]IMossFileService fileService,
            [FromServices]IMossConnectionService connectionService)
        {
            try
            {
                var ipe = connectionService.GetEndPoint("moss.stanford.edu", 7690);
                var socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipe);

                const string optionsFormatString = "G";

                var stream = new NetworkStream(socket);

                mossService.SendOptions(mossModel.UserId, mossModel.IsDirectoryMode, mossModel.IsBetaRequest, mossModel.MaxMatches,
                        mossModel.NumberOfResultsToShow, stream);

                mossModel.Files = fileService.GetFiles("C:\\Users\\alber\\Desktop\\mocksub");

                //mossModel.BaseFiles = (List<string>) fileService.GetFiles("caca");

                /*
                if (mossModel.BaseFiles.Count != 0)
                {
                    foreach (var file in mossModel.BaseFiles)
                    {
                        fileService.SendFile(file, mossModel.Language, mossModel.IsDirectoryMode, 0, stream);
                    }
                } // else, no base files to send DoNothing();
                */

                if (mossModel.Files.Count != 0)
                {
                    var fileCount = 1;
                    foreach (var file in mossModel.Files)
                    {
                        fileService.SendFile(file, mossModel.Language, mossModel.IsDirectoryMode, fileCount++, stream);
                    }
                }

                // mossService.SendOption("query 0", mossModel.Comments);

                var memStream = connectionService.ReadBytes(stream);

                ViewData["resLength"] = memStream.Length.ToString();

                var result = Encoding.UTF8.GetString(memStream);

                ViewData["ExperimentResponse"] = result;
                mossService.SendOption(Options.End.ToString(optionsFormatString), string.Empty, stream);

                socket.Dispose();

                Uri url;
                if (Uri.TryCreate(result, UriKind.Absolute, out url))
                {
                    ViewData["GoodResponse"] = url.ToString().IndexOf("\n", StringComparison.Ordinal) > 0 ? url.ToString().Split('\n')[0] : url.ToString();
                    return View("Index");
                }
                ViewData["InvalidUrl"] = "Not a valid url";
                ViewData["offset"] = mossService.GetOff().ToString();
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = "Something bad occured";
                ViewData["ExceptionM"] = ex.Message;
                return View("Index");
            }
        }
    }
    
}
