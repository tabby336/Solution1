using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Business.MossService.Interfaces;

namespace Business.MossService
{
    public class MossConnectionService : IMossConnectionService
    {
        public IPEndPoint GetEndPoint(string server, int port)
        {
            var hostEntry = Dns.GetHostAddressesAsync(server);
            var address = hostEntry.Result.First();
            return new IPEndPoint(address, port);
        }


        public string GetResponse(NetworkStream stream)
        {
            var responseBytes = new byte[512];
            var bytesRead = stream.Read(responseBytes, 0, 512);
            var response = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);
            return response;
        }
    }
}