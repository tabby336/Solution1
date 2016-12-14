using System;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace DataAccess.Repositories.ModuleManagement
{
    public class Module : BaseModel
    {
        [Required]
        public new Guid Id { get; set; }

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
