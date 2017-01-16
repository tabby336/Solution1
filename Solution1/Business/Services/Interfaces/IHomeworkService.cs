using DataAccess.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Business.CommonInfrastructure.Interfaces;

namespace Business.Services.Interfaces
{
    public interface IHomeworkService 
    {
        string Upload(IUpload upload, IList<IFormFile> files, string uid, string mid, string obs);
    }
}