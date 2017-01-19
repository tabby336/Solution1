using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.ModuleViewModels
{
    public class CreateModuleViewModel
    {
        [Required]
        [Display(Name = "Select one of your Courses")]
        public Guid CourseId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(4096)]
        public string Description { get; set; }

        [Required]
        public bool HasHomework { get; set; } = true;

        [Required]
        public bool HasTest { get; set; } = false;

    }
}
