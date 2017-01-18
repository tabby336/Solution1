using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Business.Services.Interfaces;

namespace Business.Services
{
    public class MossFileService : IMossFileService
    {
        public void SendFile(string file, string language, bool isDirectoryMode, int number, NetworkStream stream)
        {
            const string fileUploadFormat = "file {0} {1} {2} {3}\n";
            try
            {
                var fileInfo = new FileInfo(file);

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

                stream.Write(package, 0, package.Length);

                var fileBytes = Encoding.UTF8.GetBytes(File.ReadAllText(file));

                stream.Write(fileBytes, 0, fileBytes.Length);

            }
            catch (Exception exception)
            {
                //haha 
                throw exception;
            }
        }

        public void SendFiles(List<string> files, string language, bool isDirectoryMode, NetworkStream stream, List<string> baseFiles= null)
        {

            if (baseFiles != null)
            {
                foreach (var file in baseFiles)
                {
                    SendFile(file, language, isDirectoryMode, 0, stream);
                }
            }
            if (files.Count == 0) return;
            {
                var fileCount = 1;
                foreach (var file in files)
                {
                    SendFile(file, language, isDirectoryMode, fileCount++, stream);
                }
            }
        }

        public List<string> GetFiles(string directory)
        {
            return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).ToList();
        }
    }
}