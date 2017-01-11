using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Business.MossService;
using Microsoft.AspNetCore.Mvc;
using Web.Models.MossViewModel;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    public enum Options
    {
        Moss,
        Directory,
        X,
        Maxmatches,
        Show,
        End
    }
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
        public IActionResult SendRequest(SendRequestViewModel mossModel, [FromServices]IMossService mossService)
        {
            try
            {
                var hostEntry = Dns.GetHostAddressesAsync("moss.stanford.edu");
                var address = hostEntry.Result.First();
                ViewData["address"] = address;
                var ipe = new IPEndPoint(address, 7690);
                string result;
                using (var socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(ipe);
                    const string OptionsFormatString = "G";
                    ViewData["debug1"] = mossModel.UserId;

                    mossService.SetSocket(socket);

                     mossService.SendOption(
                        Options.Moss.ToString(OptionsFormatString),
                        mossModel.UserId.ToString(CultureInfo.InvariantCulture));
                  
                    mossService.SendOption(
                        Options.Directory.ToString(OptionsFormatString),
                        mossModel.IsDirectoryMode ? "1" : "0");

                    mossService.SendOption(
                        Options.X.ToString(OptionsFormatString),
                        mossModel.IsBetaRequest ? "1" : "0");

                    mossService.SendOption(
                        Options.Maxmatches.ToString(OptionsFormatString),
                        mossModel.MaxMatches.ToString(CultureInfo.InvariantCulture));

                     mossService.SendOption(
                        Options.Show.ToString(OptionsFormatString),
                        mossModel.NumberOfResultsToShow.ToString(CultureInfo.InvariantCulture));
                    
                    if (mossModel.BaseFiles.Count != 0)
                    {
                        foreach (var file in mossModel.BaseFiles)
                        {
                            mossService.SendFile(file, mossModel.Language, mossModel.IsDirectoryMode, 0);
                        }
                    } // else, no base files to send DoNothing();

                    if (mossModel.Files.Count != 0)
                    {
                        var fileCount = 1;
                        foreach (var file in mossModel.Files)
                        {
                            mossService.SendFile(file, mossModel.Language, mossModel.IsDirectoryMode, fileCount++);
                        }
                    } // else, no files to send DoNothing();
                    mossService.SendOption("query 0", mossModel.Comments);
                    
                    var bytes = new byte[512];
                    socket.Receive(bytes);

                    result = Encoding.UTF8.GetString(bytes);
                    ViewData["Response2"] = result;
                    mossService.SendOption(Options.End.ToString(OptionsFormatString), string.Empty);
                }
                Uri url;
                if (Uri.TryCreate(result, UriKind.Absolute, out url))
                {
                    ViewData["Response1"] = url.ToString().IndexOf("\n", StringComparison.Ordinal) > 0 ? url.ToString().Split('\n')[0] : url.ToString();
                    return View("Index");          
                }
                else
                {
                    ViewData["Response1"] = "Not a valid url";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ViewData["debug2"] = "Exception Response";
                ViewData["Response1"] = ex.Message;
                return View("Index");
            }
        }
    }
}
