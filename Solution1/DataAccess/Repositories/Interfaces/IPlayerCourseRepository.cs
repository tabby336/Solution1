using System;
using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlayerCourseRepository : IRepository<PlayerCourse>
    {
        PlayerCourse GetByPlayerAndCourse(Guid playerId, Guid courseId);
        IEnumerable<Course> GetCoursesForPlayer(Guid playerId);
    }
}
