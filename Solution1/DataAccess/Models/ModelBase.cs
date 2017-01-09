using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class ModelBase
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
    }
}
