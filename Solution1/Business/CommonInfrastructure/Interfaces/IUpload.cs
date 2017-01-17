using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Business.CommonInfrastructure.Interfaces
{
    public interface IUpload
    {
        IList<string> UploadFiles(IList<IFormFile> files, string path);
    }
}