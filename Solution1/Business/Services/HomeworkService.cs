using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Business.CommonInfrastructure.Interfaces;
using Business.CommonInfrastructure;

using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;

namespace Business.Services
{
    public class HomeworkService : IHomeworkService
    {
        private IHomeworkRepository _homeworkRepository;

        public HomeworkService(IHomeworkRepository repository)
        {
            _homeworkRepository = repository;
        }       

        public string Upload(IUpload upload, IList<IFormFile> files, string uid, string mid, string obs)
        {
            if (upload == null || files == null || uid == null || mid == null || obs == null)
            {
                throw new ArgumentNullException();
            }
            string root = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            root = Path.Combine(root, "Data", "homeworks", mid, uid);
            Directory.CreateDirectory(root);

            IList<string> uploadedPaths = upload.UploadFiles(files, root);
            string notUploadedFiles = "Not uploaded files:";
            foreach (var path in uploadedPaths)
            {
                Homework homework = CreateHomeworkModel(uid, mid, obs, path);
                Homework hw = _homeworkRepository.Create(homework); 
                if (hw == null) 
                {
                    notUploadedFiles += Path.GetFileName(path) + ", ";
                }   
            }
            if (notUploadedFiles == "Not uploaded files:")
            {
                return "Upload successfully!";
            }
            return notUploadedFiles.TrimEnd(' ', ',');
        }

        private Homework CreateHomeworkModel(string uid, string mid, string obs, string url)
        {
            return new Homework
            {   
                ModuleId = Guid.Parse(mid),
                UserId = Guid.Parse(uid),
                Timestamp = DateTime.Now,
                Observations = obs, 
                Url = url
            };   
        }
    }
}