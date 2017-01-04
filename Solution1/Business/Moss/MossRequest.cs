using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Business.Moss;

namespace Business.Moss
{

    public class Request
    {
        /// The user identifier.
        private long UserId { get; set; }

        /// The language for this request.
        private string Language { get; set; }

        /// The comments for this request.
        private string Comments { get; set; }

        /// The maximum matches. The -m option sets the maximum number of times a given passage may appear 
        /// before it is ignored.
        private int MaxMatches { get; set; }

        /// Gets an object representing the collection of the Source File(s) contained in this Request.
        private List<string> Files { get; set; }

        // Gets an object representing the collection of the Base File(s) contained in this Request.
        private List<string> BaseFiles { get; set; }

        private bool IsDirectoryMode { get; set; }

        private bool IsBetaRequest { get; set; }

        // The -n option determines the number of matching files to show in the results. 
        private int NumberOfResultsToShow { get; set; }

        private string Server { get; set; }

        private int Port { get; set; }

        // The moss socket.
     //   private Socket MossSocket { get; set; }

        // The file upload format string. 
        private const string FileUploadFormat = "file {0} {1} {2} {3}\n";

        // Displays the enumeration entry as a string value, if possible, and otherwise displays the integer value of the enum
        private const string OptionsFormatString = "G";

        private const int ReplySize = 512;

        public Request(long userId, IEnumerable<string> files, IEnumerable<string> baseFiles)
        {
            UserId = userId;
            Files = new List<string>(files);
            BaseFiles = new List<string>(baseFiles);

            // Some default values
            MaxMatches = 10;
            Comments = string.Empty;
            IsDirectoryMode = false;
            NumberOfResultsToShow = 250;
            Language = "cc";
            Port = 7690;
            Server = "moss.stanford.edu";
        }

        public bool SendRequest(out string response)
        {
            try
            {
                // var hostEntry = Dns.GetHostEntry(this.Server);
                var hostEntry = Dns.GetHostAddressesAsync(Server);
                var address = hostEntry.Result.First();
                var ipe = new IPEndPoint(address, Port);
                string result;
                using (var socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(ipe);
                    this.SendOption(
                        Options.Moss.ToString(OptionsFormatString),
                        this.UserId.ToString(CultureInfo.InvariantCulture),
                        socket);
                    this.SendOption(
                        Options.Directory.ToString(OptionsFormatString),
                        this.IsDirectoryMode ? "1" : "0",
                        socket);
                    this.SendOption(
                        Options.X.ToString(OptionsFormatString),
                        this.IsBetaRequest ? "1" : "0",
                        socket);
                    this.SendOption(
                        Options.Maxmatches.ToString(OptionsFormatString),
                        this.MaxMatches.ToString(CultureInfo.InvariantCulture),
                        socket);
                    this.SendOption(
                        Options.Show.ToString(OptionsFormatString),
                        this.NumberOfResultsToShow.ToString(CultureInfo.InvariantCulture),
                        socket);

                    if (BaseFiles.Count != 0)
                    {
                        foreach (var file in BaseFiles)
                        {
                            SendFile(file, socket, 0);
                        }
                    } // else, no base files to send DoNothing();

                    if (Files.Count != 0)
                    {
                        var fileCount = 1;
                        foreach (var file in this.Files)
                        {
                            SendFile(file, socket, fileCount++);
                        }
                    } // else, no files to send DoNothing();

                    this.SendOption("query 0", this.Comments, socket);

                    var bytes = new byte[ReplySize];
                    socket.Receive(bytes);

                    result = Encoding.UTF8.GetString(bytes);
                    SendOption(Options.End.ToString(OptionsFormatString), string.Empty, socket);
                }

                Uri url;
                if (Uri.TryCreate(result, UriKind.Absolute, out url))
                {
                    response = url.ToString().IndexOf("\n", System.StringComparison.Ordinal) > 0 ? url.ToString().Split('\n')[0] : url.ToString();
                    return true;
                }
                else
                {
                    response = "Not a valid response URL";
                    return false;
                }
            }
            catch (Exception ex)// Poor form to catch errors like this, but for now, if an error is thrown, I am not treating it any differently. 
            {
                response = ex.Message;
                return false;
            }
        }

        private void SendOption(string option, string value, Socket socket)
        {
            socket.Send(Encoding.UTF8.GetBytes(string.Format("{0} {1}\n", option, value)));
        }

        private void SendFile(string file, Socket socket, int number)
        {
            var fileInfo = new FileInfo(file);
            socket.Send(
                IsDirectoryMode
                    ? Encoding.UTF8.GetBytes(
                        string.Format(
                            FileUploadFormat,
                            number,
                            this.Language,
                            fileInfo.Length,
                            fileInfo.FullName.Replace("\\", "/")))
                    : Encoding.UTF8.GetBytes(
                        string.Format(FileUploadFormat, number, this.Language, fileInfo.Length, fileInfo.Name)));

            var fileBytes = Encoding.UTF8.GetBytes(File.ReadAllText(file));
            //socket.SendFile(file); //problem, does not exists in .net core api... 
            socket.Send(fileBytes);
        }

    }
}