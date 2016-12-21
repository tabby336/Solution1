using System;
using System.Collections.Generic;
using DataAccess.Models;

namespace Business.Services.Interfaces
{
    public interface IAnouncementService
    {
        IList<Anouncement> GetAllAnouncements();
        IList<Anouncement> GetAllByCourse(Guid courseId);
        IList<Anouncement> GetAllByPeriod(DateTime fromDate, DateTime toDate);
        IList<Anouncement> GetAllFilteredByCourseAndPeriod(DateTime fromDate, DateTime toDate, Guid courseId);
    }
}