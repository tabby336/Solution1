using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Business.MossService.Interfaces
{
    public interface IMossConnectionService
    {
        IPEndPoint GetEndPoint(string server, int port);

        string GetResponse(NetworkStream stream);
    }
}