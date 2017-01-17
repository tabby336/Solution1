using System;
using System.IO;
using System.Net.Sockets;
using Business.MossService;
using Business.MossService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models.MossViewModel;

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
            [FromServices]IMossOptionService mossOptionService,
            [FromServices]IMossFileService fileService,
            [FromServices]IMossConnectionService connectionService,
            string mid)
        {
          
            try
            {
                var _root = Directory.GetCurrentDirectory();
                _root = Path.Combine(_root, "Data", "homeworks", mid);
                //make connection to Moss
                
                var ipe = connectionService.GetEndPoint("moss.stanford.edu", 7690);
                var socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(ipe);

                const string optionsFormatString = "G";

                var stream = new NetworkStream(socket);

                mossOptionService.SendOptions(mossModel.UserId, mossModel.IsDirectoryMode, mossModel.IsBetaRequest, mossModel.MaxMatches,
                        mossModel.NumberOfResultsToShow, stream);

                mossModel.Files = fileService.GetFiles(_root);

                fileService.SendFiles(mossModel.Files, mossModel.Language, mossModel.IsDirectoryMode, stream);
               
                mossOptionService.SendOption("query 0", mossModel.Comments, stream);

                var response = connectionService.GetResponse(stream);

                mossOptionService.SendOption(Options.end.ToString(optionsFormatString), string.Empty, stream);

                socket.Dispose();

                var urlResponse = connectionService.GetUrl(response);
                ViewData["Response"] = urlResponse;
               
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