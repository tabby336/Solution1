using System.IO;

namespace Business.Services.Interfaces
{
    public interface IMossOptionService
    {   
        void SendOption(string option, string value, Stream stream);

        void SendOptions(long userId, bool isDirectoryMode, bool isBetaRequest, int maxMatches,
            int numberOfResultsToShow, Stream stream);
    }
}