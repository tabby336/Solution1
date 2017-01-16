using System;
using System.Net.Sockets;

namespace Business.MossService.Interfaces
{
    public interface IMossService
    {
     
        void SendOption(string option, string value, NetworkStream stream);

        void SendOptions(long userId, bool isDirectoryMode, bool isBetaRequest, int maxMatches,
            int numberOfResultsToShow, NetworkStream stream);

        string GetUrl(string response);

        // for debugging 
        int GetOff();

    }
}