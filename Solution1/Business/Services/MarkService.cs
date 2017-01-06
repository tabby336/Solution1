using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

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
    }
}