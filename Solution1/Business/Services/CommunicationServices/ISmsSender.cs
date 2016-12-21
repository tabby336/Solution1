using System.Threading.Tasks;

namespace Business.Services.CommunicationServices
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
