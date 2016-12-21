using System;
using System.Collections.Generic;
using System.Linq;
using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace Business.Services
{
    public class AnouncementService : IAnouncementService
    {
        private readonly IAnouncementRepository _anouncementRepository;

        public AnouncementService(IAnouncementRepository anouncementRepository)
        {
            _anouncementRepository = anouncementRepository;
        }

        public IList<Anouncement> GetAllAnouncements()
        {
            return _anouncementRepository.GetAll().ToList();
        }

        public IList<Anouncement> GetAllByCourse(Guid courseId)
        {
            return _anouncementRepository.GeAllByCourseId(courseId);
        }

        public IList<Anouncement> GetAllByPeriod(DateTime fromDate, DateTime toDate)
        {
            return _anouncementRepository.GetAllForPeriod(fromDate, toDate);
        }

        public IList<Anouncement> GetAllFilteredByCourseAndPeriod(DateTime fromDate, DateTime toDate, Guid courseId)
        {
            return _anouncementRepository.GetAllFilteredByPeridoAndCourseId(fromDate, toDate.Date, courseId);
        }

    }
}
