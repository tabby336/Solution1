using System.Collections.Generic;
using System.IO;

namespace Business.Services.Interfaces
{
    public interface IMossFileService
    {
        void SendFiles(List<string> files, string language, bool isDirectoryMode, Stream stream, List<string> baseFiles = null);

        List<string> GetFiles(string directory);
    }
}