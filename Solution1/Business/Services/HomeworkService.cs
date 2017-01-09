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
                // TBD: root of uploaded homeworks directory
                string directoryPath = Path.Combine(_hostingEnv.WebRootPath, "homeworks", mid, uid);
                Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                homework.Url = filePath;
                bool successful = _homeworkRepository.Upload(homework); 
                // TBD: what/if to return
                if (successful) 
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