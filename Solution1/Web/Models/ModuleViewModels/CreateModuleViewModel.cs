using System;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace Web.Models.ModuleViewModels
{
    public class CreateModuleViewModel
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(4096)]
        public string Description { get; set; }

        [StringLength(2048)]
        public string UrlPdf { get; set; } = "defaultModule.pdf";

        [Required]
        public bool HasHomework { get; set; } = true;

        [Required]
        public bool HasTest { get; set; } = false;

    }
}
