using System;
using System.ComponentModel.DataAnnotations;
namespace DataAccess.Models
{
    public class Homework:ModelBase
    {
        [Required]
        public Guid ModuleId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [StringLength(1024)]
        public string Url { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        [StringLength(2048)]
        public string Observations { get; set; }
        
        public bool OwesMeMoney { get; set; }


    }
}
