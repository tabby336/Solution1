
using System;
using System.Linq;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        private readonly IPlatformManagement _platformManagement;
        private readonly ICourseRepository _courseRepository;
        private readonly IPlayerCourseRepository _playerCourseRepository;

        public PlayerRepository(IPlatformManagement platformManagement, IPlayerCourseRepository playerCourseRepository, ICourseRepository courseRepository) : base(platformManagement)
        {
            _platformManagement = platformManagement;
            _courseRepository = courseRepository;
            _playerCourseRepository = playerCourseRepository;
        }

        public Player GetByIdWithCourses(Guid id, bool includeModules = false)
        {
            Player player = null;

            if (!includeModules)
            {
                player = _platformManagement.Players.FirstOrDefault(p => p.Id == id);
                player.Courses = _playerCourseRepository.GetCoursesForPlayer(id).ToList();
            }
            else
            {
                player = _platformManagement.Players.FirstOrDefault(p => p.Id == id);
                player.Courses = _playerCourseRepository.GetCoursesForPlayer(id).ToList();
                for (var i = 0; i < player.Courses.Count; i++)
                {
                    player.Courses.ElementAt(i).Modules =
                        _courseRepository.GetModulesForCourse(player.Courses.ElementAt(i).Id).ToList();
                }
            }

            return player;
        }
    }
}
