using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public static class ControllerExtensions
    {
        public static string GetLoggedInUserId(this Controller controller)
        {
            return controller.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}
