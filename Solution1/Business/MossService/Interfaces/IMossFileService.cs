using System.Collections.Generic;
using System.Net.Sockets;

namespace Business.MossService.Interfaces
{
    public interface IMossFileService
    {
        void SendFiles(List<string> files, string language, bool isDirectoryMode, NetworkStream stream, List<string> baseFiles = null);

        List<string> GetFiles(string directory);
    }
}