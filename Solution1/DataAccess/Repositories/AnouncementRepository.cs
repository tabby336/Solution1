using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories
{
    public class AnouncementRepository : Repository<Anouncement>, IAnouncementRepository
    {
        private readonly IPlatformManagement _platformManagement;

        public AnouncementRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
            _platformManagement = platformManagement;
        }

        public IList<Anouncement> GeAllByCourseId(Guid courseId)
        {
            return _platformManagement.Anouncements
                        .Where(a => a.CourseId == courseId)
                        .ToList();
        }

        public IList<Anouncement> GetAllForPeriod(DateTime start, DateTime end)
        {
            var anouncements = _platformManagement.Anouncements
                .Where(a => a.Date >= start && a.Date <= end);
            return anouncements.ToList();
        }

        public IList<Anouncement> GetAllFilteredByPeridoAndCourseId(DateTime start, DateTime end, Guid courseId)
        {
            var anouncements = _platformManagement.Anouncements
                .Where(a => a.Date >= start && a.Date <= end)
                .Where(a => a.CourseId == courseId);
            return anouncements.ToList();
        }
    }
}