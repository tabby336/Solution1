using System;
using System.Dynamic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace Business.MossService
{
    public class MossService : IMossService
    {

    private const string FileUploadFormat = "file {0} {1} {2} {3}\n";

        private Socket _socket;

        public MossService(Socket socket)
        {
            _socket = socket;
        }

        public void SetSocket(Socket socket)
        {
            _socket = socket;
        }
        
        public void SendFile(string file, string language, bool IsDirectoryMode, int number)
        {
            try
            {
                var fileInfo = new FileInfo(file);
                _socket.Send(
                    IsDirectoryMode
                        ? Encoding.UTF8.GetBytes(
                            string.Format(
                                FileUploadFormat,
                                number,
                                language,
                                fileInfo.Length,
                                fileInfo.FullName.Replace("\\", "/")))
                        : Encoding.UTF8.GetBytes(
                            string.Format(FileUploadFormat, number, language, fileInfo.Length, fileInfo.Name)));

                var fileBytes = Encoding.UTF8.GetBytes(System.IO.File.ReadAllText(file));

                _socket.Send(fileBytes);
   
            }
            catch (Exception exception)
            {
                //haha 
                throw exception;
            }
        }

        public void SendOption(string option, string value)
        {
            try
            {
                _socket.Send(Encoding.UTF8.GetBytes(string.Format("{0} {1}\n", option, value)));
  
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}