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
        private IMarkRepository _markRepository;
        
        public MarkService(IMarkRepository repository)
        {
            _markRepository = repository;
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
            } catch(Exception e)
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
            catch(DbUpdateException e)
            {
                // If already exists, update
                _markRepository.Update(mark);
            }
            return true;
            
        }
    }
}