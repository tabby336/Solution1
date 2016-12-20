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
            IEnumerable<Mark> marks = _markRepository.GetMarksByUserId(Guid.Parse(uid));
            if (marks == null)
            {
                return null;
            }
#pragma warning disable CS1701 // Assuming assembly reference matches identity
                return marks.ToList();
#pragma warning restore CS1701 // Assuming assembly reference matches identity
        }
    }
}