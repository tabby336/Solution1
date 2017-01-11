using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
        private const string OptionsFormatString = "G";
        private const string FileUploadFormat = "file {0} {1} {2} {3}\n";

        // GET: /Moss
        [HttpGet]
        public IActionResult Index()
        {
            return View();
          //  return "Not Implemented View";
        }

        [HttpPost]
        public IActionResult SendRequest(SendRequestViewModel mossModel)
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
                    ViewData["debug1"] = mossModel.UserId;
                    SendOption(
                        Options.Moss.ToString(OptionsFormatString),
                        mossModel.UserId.ToString(CultureInfo.InvariantCulture),
                        socket);
                  
                    SendOption(
                        Options.Directory.ToString(OptionsFormatString),
                        mossModel.IsDirectoryMode ? "1" : "0",
                        socket);
                    SendOption(
                        Options.X.ToString(OptionsFormatString),
                        mossModel.IsBetaRequest ? "1" : "0",
                        socket);
                    SendOption(
                        Options.Maxmatches.ToString(OptionsFormatString),
                        mossModel.MaxMatches.ToString(CultureInfo.InvariantCulture),
                        socket);
                    SendOption(
                        Options.Show.ToString(OptionsFormatString),
                        mossModel.NumberOfResultsToShow.ToString(CultureInfo.InvariantCulture),
                        socket);
                    /*
                    if (mossModel.BaseFiles.Count != 0)
                    {
                        foreach (var file in mossModel.BaseFiles)
                        {
                            SendFile(file, socket, mossModel.Language, mossModel.IsDirectoryMode, 0);
                        }
                    } // else, no base files to send DoNothing();

                    if (mossModel.Files.Count != 0)
                    {
                        var fileCount = 1;
                        foreach (var file in mossModel.Files)
                        {
                            SendFile(file, socket, mossModel.Language, mossModel.IsDirectoryMode, fileCount++);
                        }
                    } // else, no files to send DoNothing();
                    */
                    SendOption("query 0", mossModel.Comments, socket);
                    
                    var bytes = new byte[512];
                    socket.Receive(bytes);

                    result = Encoding.UTF8.GetString(bytes);
                    ViewData["Response2"] = result;
                    SendOption(Options.End.ToString(OptionsFormatString), string.Empty, socket);
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

        private static void SendOption(string option, string value, Socket socket)
        {
            socket.Send(Encoding.UTF8.GetBytes(string.Format("{0} {1}\n", option, value)));
        }

        private static void SendFile(string file, Socket socket, string Language, bool IsDirectoryMode, int number)
        {
            var fileInfo = new FileInfo(file);
            socket.Send(
                IsDirectoryMode
                    ? Encoding.UTF8.GetBytes(
                        string.Format(
                            FileUploadFormat,
                            number,
                            Language,
                            fileInfo.Length,
                            fileInfo.FullName.Replace("\\", "/")))
                    : Encoding.UTF8.GetBytes(
                        string.Format(FileUploadFormat, number, Language, fileInfo.Length, fileInfo.Name)));

            var fileBytes = Encoding.UTF8.GetBytes(System.IO.File.ReadAllText(file));
     
            socket.Send(fileBytes);
        }
    }
}
