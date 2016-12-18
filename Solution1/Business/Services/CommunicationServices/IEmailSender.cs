using System.Threading.Tasks;

namespace Business.Services.CommunicationServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
