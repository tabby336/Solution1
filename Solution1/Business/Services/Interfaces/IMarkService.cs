using DataAccess.Models;
using System.Collections.Generic;

namespace Business.Services.Interfaces
{
    public interface IMarkService
    {
        List<Mark> FilterMarksByUser(string uid);
        List<Dictionary<string,string>> GetHumanReadableMarks(string uid);
        bool MarkHomework(string modueleId, string playerId, string creatorId, string value);
    }
}