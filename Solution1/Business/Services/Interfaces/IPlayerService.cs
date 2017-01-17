
using DataAccess.Models;

namespace Business.Services.Interfaces
{
    public interface IPlayerService
    {
        Player GetPlayerData(string id);
        string GetImagePathForPlayerId(string id);
        void UpdatePlayer(Player player);
    }
}
