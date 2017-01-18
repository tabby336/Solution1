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
        public HomeworkRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
        }

        public IEnumerable<Homework> GetHomeworksByUserId(Guid uid)
        {
            return context.Homeworks.Where(homework => homework.UserId == uid);
        }

        public IEnumerable<Homework> GetHomeworksByModuleId(Guid mid)
        {
            return context.Homeworks.Where(homework => homework.ModuleId == mid);
        }

        public override Homework Create(Homework homework)
        {
            if (homework == null)
            {
                throw new ArgumentNullException();
            }
            homework.Id = Guid.NewGuid();
            Homework hw = base.Create(homework);
            return homework;
        }
    }
}
