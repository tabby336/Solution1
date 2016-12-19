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

        public string FilterMarksByUser(string uid) 
        {
            string ret = "";
            List<Mark> marks = _markRepository.GetMarksByUserId(Guid.Parse(uid)).ToList();
            foreach(Mark m in marks)
            {
                ret = ret + m.Value + " ";
            }
            return ret;
        }
    }
}