using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Business.MossService.Interfaces
{
    public interface IMossFileService
    {
        void SendFile(string file, string language, bool isDirectoryMode, int number, NetworkStream stream);

        List<string> GetFiles(string directory);
    }
}