using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Business.MossService.Interfaces;

namespace Business.MossService
{
    public class MossFileService : IMossFileService
    {
        public void SendFile(string file, string language, bool isDirectoryMode, int number, NetworkStream stream)
        {
            const string fileUploadFormat = "file {0} {1} {2} {3}\n";
            try
            {
                var fileInfo = new FileInfo(file);

                Stream fs = File.OpenRead(file);

                var fileBuffer = new byte[fs.Length];

                fs.Read(fileBuffer, 0, (int)fs.Length);

                var package = isDirectoryMode
                    ? Encoding.UTF8.GetBytes(
                        string.Format(
                            fileUploadFormat,
                            number,
                            language,
                            fileInfo.Length,
                            fileInfo.FullName.Replace("\\", "/")))
                    : Encoding.UTF8.GetBytes(
                        string.Format(fileUploadFormat, number, language, fileInfo.Length, fileInfo.Name));

                var size = package.GetLength(0);

                stream.Write(package, 0, size);
              
                // var fileB = File.ReadAllBytes(file);

                stream.Write(fileBuffer, 0, fileBuffer.Length);

            }
            catch (Exception exception)
            {
                //haha 
                throw exception;
            }
        }

        public List<string> GetFiles(string directory)
        {
            return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).ToList();
        }
    }
}