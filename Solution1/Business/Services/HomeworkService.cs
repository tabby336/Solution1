using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Business.CommonInfrastructure.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace Business.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly string _root;
        private readonly string _tmp;

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
            
            var hwdir = Path.Combine(_root, mid, uid);

            var uploadedPaths = upload.UploadFiles(files, hwdir);
            var notUploadedFiles = (from path in uploadedPaths let homework = CreateHomeworkModel(uid, mid, obs, path) let hw = _homeworkRepository.Create(homework) where hw == null select path).Aggregate("Not uploaded files:", (current, path) => current + (Path.GetFileName(path) + ", "));
            return notUploadedFiles == "Not uploaded files:" ? "Upload successfully!" : notUploadedFiles.TrimEnd(' ', ',');
        }

        public string Archive(string uid, string mid)
        {
            var hwpath = Path.Combine(_root, mid, uid);
    
            // empty the temp folder
            Directory.EnumerateFiles(_tmp).ToList().ForEach(File.Delete);
            
            //TODO: change with actual student name
            Path.Combine("Data", "tmp");
            var archiveName = uid + ".zip";
            var archivePath = Path.Combine(_tmp, archiveName);
            ZipFile.CreateFromDirectory(hwpath, archivePath, CompressionLevel.Fastest, true);

            return archivePath;
        }

        public IEnumerable<Player> GetPlayersThatUploaded(string mid)
        {
            var hws = _homeworkRepository.GetHomeworksByModuleId(Guid.Parse(mid));
            var players = hws.Select(hw => _playerRepository.GetById(hw.UserId)).ToList();
            return players.Distinct();
        }


        private static Homework CreateHomeworkModel(string uid, string mid, string obs, string url)
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