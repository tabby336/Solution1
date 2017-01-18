using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                if (marks != null)
                {
                    return marks.ToList();
                }
                return null;
            } catch
            {
                return null;
            }
        }

        public List<Dictionary<string,string>> GetHumanReadableMarks(string uid)
        {
            List<Dictionary<string,string>> MarksExplained = new List<Dictionary<string,string>>();
            try
            {
                IEnumerable<Mark> marks = _markRepository.GetMarksByUserId(Guid.Parse(uid));
                foreach(Mark m in marks)
                {
                    var explained = new Dictionary<string, string>();

                    var module = _moduleRepository.GetById(m.HomeworkId);
                    explained["ModuleName"] = module.Title;
                    explained["Value"] = m.Value.ToString();
                    explained["CourseName"] = _courseRepository.GetById(module.CourseId).Title;
                    MarksExplained.Add(explained);
                }
                return MarksExplained;
            } 
            catch(Exception e)
            {
                return null;
            }
        }

        public bool MarkHomework(string moduleId, string playerId, string creatorId, string value)
        {
            Mark mark = new Mark
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
                Mark m = _markRepository.Create(mark);
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