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
                //make connection to Moss
                var ipe = connectionService.GetEndPoint("moss.stanford.edu", 7690);
                var socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(ipe);

                //System.IO.File.WriteAllText("C:\\Users\\alber\\Desktop\\socket.txt", "connected socked");

                const string optionsFormatString = "G";

                var stream = new NetworkStream(socket);

                mossService.SendOptions(mossModel.UserId, mossModel.IsDirectoryMode, mossModel.IsBetaRequest, mossModel.MaxMatches,
                        mossModel.NumberOfResultsToShow, stream);

              //  System.IO.File.WriteAllText("C:\\Users\\alber\\Desktop\\options.txt", "Options Send");

                mossModel.Files = fileService.GetFiles("C:\\Users\\alber\\Desktop\\mocksub");

               // mossModel.BaseFiles = fileService.GetFiles("C:\\Users\\alber\\Desktop\\mocksub\\base");

                fileService.SendFiles(mossModel.Files, mossModel.Language, mossModel.IsDirectoryMode, stream);
               
                 mossService.SendOption("query 0", mossModel.Comments, stream);


                var response = connectionService.GetResponse(stream);

              //  System.IO.File.WriteAllText("C:\\Users\\alber\\Desktop\\receive.txt", "Response read: ");

            //    System.IO.File.WriteAllText("C:\\Users\\alber\\Desktop\\receiveR.txt", response);

                mossService.SendOption(Options.end.ToString(optionsFormatString), string.Empty, stream);

                socket.Dispose();

                var urlResponse = mossService.GetUrl(response);
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