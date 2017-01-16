using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Business.MossService.Interfaces
{
    public interface IMossFileService
    {
       // void SendFile(string file, string language, bool isDirectoryMode, int number, NetworkStream stream);

        void SendFiles(List<string> files, string language, bool isDirectoryMode, NetworkStream stream, List<string> baseFiles = null);

        List<string> GetFiles(string directory);
    }
}