
using DataAccess.Models;

namespace Business.Services.Interfaces
{
    public interface IModuleService
    {
        Module GetModule(string id);
        string GetPdfPathForModule(string id);
    }
}
