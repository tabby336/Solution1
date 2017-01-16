using System.Collections.Generic;
using DataAccess.Models;

namespace Web.Models.AnouncementViewModel
{
    public class AnouncementViewModel
    {
        public IList<Anouncement> Anouncements { get; set; }
    }
}
