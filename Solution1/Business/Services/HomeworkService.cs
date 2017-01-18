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
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace Business.Services
{
    public class HomeworkService : IHomeworkService
    {
        private IHomeworkRepository _homeworkRepository;
        private IPlayerRepository _playerRepository;
        private string _root;
        private string _tmp;

        public HomeworkService(IHomeworkRepository hw, IPlayerRepository player)
        {
            _homeworkRepository = hw;
            _playerRepository = player;

            _root = Directory.GetCurrentDirectory();
            _tmp = Path.Combine(_root, "Data", "tmp");
            _root = Path.Combine(_root, "Data", "homeworks");
            Directory.CreateDirectory(_tmp);
        }       

        public string Upload(IUpload upload, IList<IFormFile> files, string uid, string mid, string obs)
        {
            if (upload == null || files == null || uid == null || mid == null)
            {
                throw new ArgumentNullException();
            }
            
            string hwdir = Path.Combine(_root, mid, uid);

            IList<string> uploadedPaths = upload.UploadFiles(files, hwdir);
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

        public string Archive(string uid, string mid)
        {
            string hwpath = Path.Combine(_root, mid, uid);
    
            // empty the temp folder
            Directory.EnumerateFiles(_tmp).ToList().ForEach(f => System.IO.File.Delete(f));
            
            //TODO: change with actual student name
            string relativeTmpPath = Path.Combine("Data", "tmp");
            string archiveName = uid + ".zip";
            string archivePath = Path.Combine(_tmp, archiveName);
            ZipFile.CreateFromDirectory(hwpath, archivePath, CompressionLevel.Fastest, true);

            return archivePath;
        }

        public IEnumerable<Player> GetPlayersThatUploaded(string mid)
        {
            IEnumerable<Homework> hws = _homeworkRepository.GetHomeworksByModuleId(Guid.Parse(mid));
            List<Player> players = new List<Player>();
            foreach(Homework hw in hws)
            {
                players.Add(_playerRepository.GetById(hw.UserId));
            }
            return players.Distinct();
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