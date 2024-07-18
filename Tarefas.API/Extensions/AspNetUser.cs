using System.Security.Claims;

namespace Tarefas.API.Extensions
{
    public class AspNetUser(IHttpContextAccessor accessor) : IUser
    {
        public readonly IHttpContextAccessor accessor = accessor;

        public string Name => accessor.HttpContext.User.Identity.Name;

        public bool IsAuthenticated()
        {
            return accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return accessor.HttpContext.User.IsInRole(role);
        }

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? accessor.HttpContext.User.GetUserEmail() : "";
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return accessor.HttpContext.User.Claims;
        }

        public string GetUserRole()
        {
            return IsAuthenticated() ? accessor.HttpContext.User.GetUserRole() : "";
        }

        public IEnumerable<string> GetUserClaims()
        {
            return accessor.HttpContext.User.GetUserClaims();
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal is null)
            {
                throw new ArgumentException(null, principal.ToString());
            }

            Claim claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal is null)
            {
                throw new ArgumentException(null, principal.ToString());
            }

            Claim claim = principal.FindFirst(ClaimTypes.Email);

            return claim?.Value;
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            if (principal is null)
            {
                throw new ArgumentException(null, principal.ToString());
            }

            Claim role = principal.FindFirst(ClaimTypes.Role);

            return role?.Value;
        }

        public static IEnumerable<string> GetUserClaims(this ClaimsPrincipal principal)
        {
            if (principal is null)
            {
                throw new ArgumentException(null, principal.ToString());
            }

            IEnumerable<string> claims = principal.Claims.Select(x => x.Type).Distinct();

            return claims;
        }
    }
}