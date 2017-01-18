using System;

namespace DataAccess.Models
{
    public class PlayerCourse: ModelBase
    {
        public Guid PlayerId { get; set; }
        public Guid CourseId { get; set; }
    }
}
