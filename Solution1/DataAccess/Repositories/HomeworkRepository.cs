using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class HomeworkRepository : Repository<Homework>, IHomeworkRepository

    {
        public HomeworkRepository(PlatformManagement platformManagement) : base(platformManagement)
        {
        }

        public IEnumerable<Homework> GetHomeworksByUserId(Guid uid)
        {
            return context.Homeworks.Where(homework => homework.UserId == uid);
        }

        public bool Upload(Homework homework)
        {
            homework.Id = Guid.NewGuid();
            Homework hw = this.Create(homework);
            return homework.Equals(hw);
        }
    }
}
