using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Module : ModelBase
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        [StringLength(1024)]
        public string UrlPdf { get; set; }

        public bool HasHomework { get; set; }

        public bool HasTest { get; set; }


    }
}
