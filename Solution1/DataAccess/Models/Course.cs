using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Course : ModelBase
    {
        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(4096)]
        public string Description { get; set; }

        [StringLength(128)]
        public string HashTag { get; set; } = "course";

        [Required]
        [StringLength(512)]
        public string Author { get; set; }


        [Required]
        [StringLength(1024)]
        public string PhotoUrl { get; set; } = "defaultCourse.png";

        [Required]
        [StringLength(2048)]
        public string DataLink { get; set; } = "#";

        [Required]
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}
