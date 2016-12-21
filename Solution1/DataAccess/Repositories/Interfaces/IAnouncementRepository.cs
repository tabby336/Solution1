using System;
using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IAnouncementRepository
    {
        IList<Anouncement> GeAllByCourseId(Guid courseId);
        IList<Anouncement> GetAllForPeriod(DateTime start, DateTime end);
        IList<Anouncement> GetAllFilteredByPeridoAndCourseId(DateTime start, DateTime end, Guid courseId);
        Anouncement Create(Anouncement p);
        void Update(Anouncement p);
        void Delete(Anouncement p);
        Anouncement GetById(Guid id);
        IEnumerable<Anouncement> GetAll();
    }
}