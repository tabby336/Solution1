using System;
using System.IO;
using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace Business.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private const string DefaultPlayerAvatarPath = "defaultPlayer.png";

        public PlayerService(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }

        public Player GetPlayerData(string id)
        {
            Guid playerId;
            Guid.TryParse(id, out playerId);
            return _playerRepository.GetById(playerId);
        }

        public string GetImagePathForPlayerId(string id)
        {
            Guid playerId;
            Guid.TryParse(id, out playerId);
            var player = _playerRepository.GetById(playerId);
            if (player == null) return null;

            var root = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Avatars\Players\");
            var path = root + id + @"\" + player.PhotoUrl;
            if (!File.Exists(path))
                path = root + DefaultPlayerAvatarPath;

            return path;
        }

        public void UpdatePlayer(Player player)
        {
            _playerRepository.Update(player);
        }
    }
}
