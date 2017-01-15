using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        public byte[] ReadBytes(NetworkStream stream)
        {
            var buffer = new byte[512];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}