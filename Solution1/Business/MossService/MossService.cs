using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using Business.MossService.Interfaces;

namespace Business.MossService
{
    public enum Options
    {
        moss,
        directory,
        X,
        maxmatches,
        show,
        query,
        end
    };

    public class MossService : IMossService
    {
        private const string OptionsFormatString = "G";
        private int _offset = 0;

        public string GetUrl(string response)
        {
            Uri url;
            if (Uri.TryCreate(response, UriKind.Absolute, out url))
            {
                return url.ToString().IndexOf("\n", StringComparison.Ordinal) > 0
                    ? url.ToString().Split('\n')[0]
                    : url.ToString();
            }
            return "Invalid response";
        }

        public int GetOff()
        {
            return _offset;
        }

       

        public void SendOption(string option, string value, NetworkStream stream)
        {
            try
            {
                var package = Encoding.UTF8.GetBytes(string.Format("{0} {1}\n", option, value));
                var size = package.GetLength(0);
                stream.Write(package, 0, size);
                _offset += size;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SendOptions(long userId, bool isDirectoryMode, bool isBetaRequest, int maxMatches,
            int numberOfResultsToShow, NetworkStream stream)
        {
                SendOption(
                        Options.moss.ToString(OptionsFormatString),
                        userId.ToString(CultureInfo.InvariantCulture), stream);

                SendOption(
                    Options.directory.ToString(OptionsFormatString),
                    isDirectoryMode ? "1" : "0", stream);

                SendOption(
                    Options.X.ToString(OptionsFormatString),
                    isBetaRequest ? "1" : "0", stream);

                SendOption(
                    Options.maxmatches.ToString(OptionsFormatString),
                    maxMatches.ToString(CultureInfo.InvariantCulture), stream);

                SendOption(
                   Options.show.ToString(OptionsFormatString),
                   numberOfResultsToShow.ToString(CultureInfo.InvariantCulture), stream);

        }
    }
}