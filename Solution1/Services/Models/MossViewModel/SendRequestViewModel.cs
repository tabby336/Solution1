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

        /// The comments for this request.
        public string Comments { get; set; }

        /// The maximum matches. The -m option sets the maximum number of times a given passage may appear 
        /// before it is ignored.
        public int MaxMatches { get; set; }

        // Gets an object representing the collection of the Base File(s) contained in this Request.
        public List<string> BaseFiles { get; set; }

        public bool IsDirectoryMode { get; set; }

        public bool IsBetaRequest { get; set; }

        // The -n option determines the number of matching files to show in the results. 
        public int NumberOfResultsToShow { get; set; }
    }
}
