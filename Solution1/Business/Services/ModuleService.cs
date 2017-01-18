using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace Business.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private const string DefaultModulePdfPath = "defaultModule.pdf";

        public ModuleService(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
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
    }
}
