
using DataAccess.Models;

namespace Business.Services.Interfaces
{
    public interface IPlayerService
    {
        Player GetPlayerData(string id, bool includeCourses = false);
        string GetImagePathForPlayerId(string id);
        void UpdatePlayer(Player player);
    }
}
