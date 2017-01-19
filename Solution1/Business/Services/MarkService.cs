using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Business.Services
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepository _markRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IModuleRepository _moduleRepository;
        
        public MarkService(IMarkRepository mark, ICourseRepository course, IModuleRepository module)
        {
            _markRepository = mark;
            _courseRepository = course;
            _moduleRepository = module;
        }

        public List<Mark> FilterMarksByUser(string uid)
        {
            try
            {
                IEnumerable<Mark> marks = _markRepository.GetMarksByUserId(Guid.Parse(uid));
                return marks?.ToList();
            } catch
            {
                return null;
            }
        }

        public List<Dictionary<string,string>> GetHumanReadableMarks(string uid)
        {
            List<Dictionary<string,string>> marksExplained = new List<Dictionary<string,string>>();
            try
            {
                var marks = _markRepository.GetMarksByUserId(Guid.Parse(uid));
                foreach(var m in marks)
                {
                    var explained = new Dictionary<string, string>();

                    var module = _moduleRepository.GetById(m.HomeworkId);
                    explained["ModuleName"] = module.Title;
                    explained["Value"] = m.Value.ToString(CultureInfo.InvariantCulture);
                    explained["CourseName"] = _courseRepository.GetById(module.CourseId).Title;
                    marksExplained.Add(explained);
                }
                return marksExplained;
            } 
            catch(Exception)
            {
                return null;
            }
        }

        public bool MarkHomework(string moduleId, string playerId, string creatorId, string value)
        {
            var mark = new Mark
            {
                HomeworkId = Guid.Parse(moduleId),
                UserId = Guid.Parse(playerId),
                CreatorId = Guid.Parse(creatorId),
                Description = "",
                HasComment = false,
                HasContestation = false,
                Id = Guid.NewGuid(),
                Timestamp = DateTime.Now,
                Value = float.Parse(value)
            };
            try 
            {
                var m = _markRepository.Create(mark);
                return (m == mark);
            }
            catch(DbUpdateException)
            {
                // If already exists, update
                _markRepository.Update(mark);
            }
            return true;
            
        }
    }
}