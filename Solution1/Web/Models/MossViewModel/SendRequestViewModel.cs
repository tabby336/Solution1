using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.MossViewModel
{
    public class SendRequestViewModel
    {
        [Required]
        public long UserId { get; set; }
        
        [Required]
        public string Language { get; set; }


        public List<string> Files { get; set; }


        public Uri Response { get; set; }


        public string Comments { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Max matches range 1-100")]
        public int MaxMatches { get; set; }


        public List<string> BaseFiles { get; set; }


        public bool IsDirectoryMode { get; set; }


        public bool IsBetaRequest { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "The number of results to show can be in range 1-1000")]
        public int NumberOfResultsToShow { get; set; }
    }
}
