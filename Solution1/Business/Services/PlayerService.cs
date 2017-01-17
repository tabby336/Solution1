using System;
using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace Business.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

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
            return player == null ? null : @"Data\Avatars\" + player.PhotoUrl;
        }

        public void UpdatePlayer(Player player)
        {
            _playerRepository.Update(player);
        }
    }
}
