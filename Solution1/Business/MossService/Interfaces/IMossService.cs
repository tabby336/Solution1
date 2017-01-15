using System.Net.Sockets;

namespace Business.MossService.Interfaces
{
    public interface IMossService
    {
     
        void SendOption(string option, string value, NetworkStream stream);

        void SendOptions(long userId, bool isDirectoryMode, bool isBetaRequest, int maxMatches,
            int numberOfResultsToShow, NetworkStream stream);

        // for debugging 
        int GetOff();

    }
}