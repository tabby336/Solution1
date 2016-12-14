using System;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace DataAccess.Repositories.CourseManagement
{
    public class Course : BaseModel
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(1024)]
        public string Description { get; set; }
        [StringLength(255)]
        public string HashTag { get; set; }
        [StringLength(1024)]
        public string PhotoUrl { get; set; }
        [StringLength(1024)]
        public string DataLink { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }

    }
}
