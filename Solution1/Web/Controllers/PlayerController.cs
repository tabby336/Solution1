
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ProfileViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class PlayerController: Controller
    {
        private readonly IPlayerService _playerService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PlayerController(IPlayerService playerService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _playerService = playerService;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public IActionResult Index(string id = null)
        { 
            if (id == null)
            {
                id = this.GetLoggedInUserId();
                if(id == null)
                    return NotFound();
            }

            var player = _playerService.GetPlayerData(id);
            if(player == null)
                return NotFound();

            var playerData = new BasicProfileViewModel()
            {
                Player = player,
                FullName = player.FirstName + " " + player.LastName
            };
            return PartialView("../Home/Profile", playerData);
        }

        public IActionResult Image(string id = null)
        {
            if (id == null)
                return NotFound();

            var filePath = _playerService.GetImagePathForPlayerId(id);
            if(filePath == null)
                return NotFound();

            var redirectUrl = string.Format(@"/UpDown/Download?path={0}", filePath);
            return Redirect(redirectUrl);
        }
    }
}
