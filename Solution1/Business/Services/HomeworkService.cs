using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

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
            foreach (var file in files)
            {
                string filename = ContentDispositionHeaderValue
                             .Parse(file.ContentDisposition)
                             .FileName
                             .Trim('"');
                string root = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
                string directoryPath = Path.Combine(root, "Data", "homeworks", mid, uid);
                Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                homework.Url = filePath;
                Homework hw = _homeworkRepository.Create(homework); 
                if (hw != null) 
                {
                    res += String.Join(" ", filename, "uploaded");
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