using System;
using System.Globalization;
using System.IO;
using System.Text;
using Business.Services.Interfaces;

namespace Business.Services
{
    public class MossOptionService : IMossOptionService
    {
        public void SendOption(string option, string value, Stream stream)
        {
            if (option == null)
            {
                throw new ArgumentNullException();
            }         
            var package = Encoding.UTF8.GetBytes(string.Format("{0} {1}\n", option, value));
            stream.Write(package, 0, package.Length);
        }

        public void SendOptions(long userId, bool isDirectoryMode, bool isBetaRequest, int maxMatches,
            int numberOfResultsToShow, Stream stream)
        {
            const string optionsFormatString = "G";
            SendOption(
                        Options.moss.ToString(optionsFormatString),
                        userId.ToString(CultureInfo.InvariantCulture), stream);

                SendOption(
                    Options.directory.ToString(optionsFormatString),
                    isDirectoryMode ? "1" : "0", stream);

                SendOption(
                    Options.X.ToString(optionsFormatString),
                    isBetaRequest ? "1" : "0", stream);

                SendOption(
                    Options.maxmatches.ToString(optionsFormatString),
                    maxMatches.ToString(CultureInfo.InvariantCulture), stream);

                SendOption(
                   Options.show.ToString(optionsFormatString),
                   numberOfResultsToShow.ToString(CultureInfo.InvariantCulture), stream);

        }
    }
}