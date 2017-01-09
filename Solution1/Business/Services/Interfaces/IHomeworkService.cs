using DataAccess.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Interfaces
{
    public interface IHomeworkService 
    {
        string Upload(IList<IFormFile> files, string uid, string mid, string obs);
    }
}