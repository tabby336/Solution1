using System.ComponentModel.DataAnnotations;

namespace Web.Models.ErrorViewModels
{
    public class ErrorViewModel
    {
        [Required]
        public string Title { get; set; } = "Error";

        [Required]
        public string Description { get; set; } = "Something went seriously wrong.";

        public int? ErrorCode { get; set; }
    }
}
