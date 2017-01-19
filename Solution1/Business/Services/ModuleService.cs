using System;
using System.Collections.Generic;
using System.IO;
using Business.CommonInfrastructure;
using Business.CommonInfrastructure.Interfaces;
using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Business.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IPlayerService _playerService;

        private const string DefaultModulePdfPath = "defaultModule.pdf";

        public ModuleService(IModuleRepository moduleRepository, IPlayerService playerService)
        {
            _moduleRepository = moduleRepository;
            _playerService = playerService;
        }

        public Module GetModule(string id)
        {
            Guid moduleId;
            Guid.TryParse(id, out moduleId);
            return _moduleRepository.GetByIdWithCourse(moduleId);
        }

        public string GetPdfPathForModule(string id)
        {
            Guid moduleId;
            Guid.TryParse(id, out moduleId);
            var module = _moduleRepository.GetById(moduleId);
            if (module == null) return null;

            var root = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Modules\");
            var path = root + id + @"\" + module.UrlPdf;
            if (!File.Exists(path))
                path = root + DefaultModulePdfPath;

            return path;
        }

        public Module CreateModule(string userid, Guid courseId, string title, string description, IList<IFormFile> files, bool hasHomework, bool hasTest)
        {
            try
            {
                var moduleFromData = new Module()
                {
                    CourseId = courseId,
                    Title = title,
                    Description = description,
                    HasHomework = hasHomework,
                    HasTest = hasTest
                };

                moduleFromData = _moduleRepository.Create(moduleFromData);
                IUpload uploader = new Upload(new FileDataSource());
                var success = Upload(uploader, files, moduleFromData.Id.ToString());
                if (!success)
                {
                    _moduleRepository.Delete(moduleFromData);
                    throw new Exception();
                }

                // finally set the image
                moduleFromData.UrlPdf = files[0].FileName;
                _moduleRepository.Update(moduleFromData);
                return moduleFromData;
            }
            catch
            {
                return null;
            }
        }
        public bool Upload(IUpload upload, IList<IFormFile> files, string courseId)
        {
            if (upload == null || courseId == null)
                return false;

            var root = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Modules", courseId);

            var uploadedPaths = upload.UploadFiles(files, root);
            return uploadedPaths.Count == files.Count;
        }
    }
}
