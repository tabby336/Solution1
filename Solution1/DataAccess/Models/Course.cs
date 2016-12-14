using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Course : ModelBase
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
