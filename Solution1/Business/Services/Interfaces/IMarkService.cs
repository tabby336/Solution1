using DataAccess.Models;
using System.Collections.Generic;

namespace Business.Services.Interfaces
{
    public interface IMarkService
    {
        List<Mark> FilterMarksByUser(string uid);
    }
}