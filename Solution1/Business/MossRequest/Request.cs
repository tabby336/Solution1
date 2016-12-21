
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Business.MossRequest
{

    public class Request
    {
        /// The user identifier.
        private long UserId { get; set; }

        /// The language for this request.
        private string Language { get; set; }

        /// The comments for this request.
        private string Comments { get; set; }

        /// The maximum matches. The -m option sets the maximum number of times a given passage may appear 
        /// before it is ignored.
        private int MaxMatches { get; set; }

        /// Gets an object representing the collection of the Source File(s) contained in this Request.
        private List<string> Files { get; set; }

        /// Gets an object representing the collection of the Base File(s) contained in this Request.
        private List<string> BaseFiles { get; set; }

        private bool IsDirectoryMode { get; set; }

        /// The -n option determines the number of matching files to show in the results. 
        private int NumberOfResultsToShow { get; set; }

        private string Server { get; set; }

        private int Port { get; set; }

        /// The moss socket.
        private Socket MossSocket { get; set; }

        public Request(long userId, IEnumerable<string> files, IEnumerable<string> baseFiles)
        {
            UserId = userId;
            Files = new List<string>(files);
            BaseFiles = new List<string>(baseFiles);
            
            // Some default values
            MaxMatches = 10; 
            Comments = string.Empty;
            IsDirectoryMode = false;
            NumberOfResultsToShow = 250;
            Language = "cc";
            Port = 7690;
            Server = "moss.stanford.edu";
        }

        public bool SendRequest()
        {
            throw new NotImplementedException();
        }
 
    }
}