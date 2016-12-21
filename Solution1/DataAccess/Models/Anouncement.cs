using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Anouncement : ModelBase
    {
        [Required]
        public Guid CourseId { get; set; }
        [StringLength(1024)]
        public string Title { get; set; }
        [StringLength(8064)]
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
