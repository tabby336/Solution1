using System.Net.Sockets;


namespace Business.MossService
{
    public interface IMossService
    {
        void SendFile(string file,string language, bool IsDirectoryMode, int number);
        void SendOption(string option, string value);
        void SetSocket(Socket socket);
    }
}