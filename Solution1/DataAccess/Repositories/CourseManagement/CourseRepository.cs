namespace DataAccess.Repositories.CourseManagement
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(PlatformManagement platformManagement) : base(platformManagement)
        {
        }
    }
}
