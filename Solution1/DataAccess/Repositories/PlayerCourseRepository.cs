
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories
{
    public class PlayerCourseRepository : Repository<PlayerCourse>, IPlayerCourseRepository
    {
        private readonly IPlatformManagement _platformManagement;

        public PlayerCourseRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
            _platformManagement = platformManagement;
        }

        public PlayerCourse GetByPlayerAndCourse(Guid playerId, Guid courseId)
        {
            return
                _platformManagement.PlayerCourses
                    .FirstOrDefault(pc => pc.PlayerId == playerId && pc.CourseId == courseId);
        }

        public IEnumerable<Course> GetCoursesForPlayer(Guid playerId)
        {
            var coursesIds =
                _platformManagement.PlayerCourses.Where(pc => pc.PlayerId == playerId).Select(pc => pc.CourseId).ToList();
            var courses = (from c in _platformManagement.Courses.ToList() where coursesIds.Contains(c.Id) select c);
            return courses;
        }
    }
}
