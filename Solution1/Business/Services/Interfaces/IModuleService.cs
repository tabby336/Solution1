
using System;
using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Interfaces
{
    public interface IModuleService
    {
        Module GetModule(string id);
        string GetPdfPathForModule(string id);
        Module CreateModule(string userid, Guid courseId, string title, string description, IList<IFormFile> files, bool hasHomework, bool hasTest);

    }
}
