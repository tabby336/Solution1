using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.MossViewModel
{
    public class SendRequestViewModel
    {
        [Required]
        public long UserId { get; set; }

        public string Language { get; set; }

        public List<string> Files { get; set; }

        public Uri Response { get; set; }
    }
}
