using System;
using System.Security.Claims;

namespace Web.Controllers
{
    public static class UserExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }

        public static bool IsStudent(this ClaimsPrincipal user)
        {
            return user.IsInRole("Student");
        }

        public static bool IsProfessor(this ClaimsPrincipal user)
        {
            return user.IsInRole("Professor");
        }

        public static Guid Id(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
