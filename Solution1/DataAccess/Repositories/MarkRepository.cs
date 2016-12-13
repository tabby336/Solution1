using Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MarkRepository : Repository<Mark>
    {
        public MarkRepository(PlatformManagement platformManagement) : base(platformManagement)
        {
        }

        public IEnumerable<Mark> GetStudentGrade(Guid studentId)
        {
            if (studentId == null)
            {
                throw new ArgumentNullException();
            }
            return context.Marks.Where(mark => mark.StudentId == studentId);
        }
    }
}
