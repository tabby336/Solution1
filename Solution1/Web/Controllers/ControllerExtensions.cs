using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
