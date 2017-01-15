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
        private IHostingEnvironment _hostingEnv;
        private IHomeworkRepository _homeworkRepository;

        public HomeworkService(IHostingEnvironment env, IHomeworkRepository repository)
        {
            _hostingEnv = env;
            _homeworkRepository = repository;
        }       

        public string Upload(IList<IFormFile> files, string uid, string mid, string obs)
        {
            Homework homework = CreateHomeworkModel(uid, mid, obs);
            string res = "";

            string root = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            root = Path.Combine(root, "Data", "homeworks", mid, uid);
            Directory.CreateDirectory(root);

            IUpload upload = new Upload(new FileDataSource());
            IList<string> uploadedPaths = upload.UploadFiles(files, root);

            foreach (var path in uploadedPaths)
            {
                homework.Url = path;
                Homework hw = _homeworkRepository.Create(homework); 
                if (hw != null) 
                {
                    res += String.Join(" ", Path.GetFileName(path), "uploaded");
                }   
            }
            return res;
        }

        private Homework CreateHomeworkModel(string uid, string mid, string obs)
        {
            return new Homework
            {
                ModuleId = Guid.Parse(mid),
                UserId = Guid.Parse(uid),
                Timestamp = DateTime.Now,
                Observations = obs
            };   
        }
    }
}