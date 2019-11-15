using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Foo.Api
{
    public class SpecialAccessRequirement : IAuthorizationRequirement
    {
        public string MarkerForSemiProtectedEndpoints { get; }

        public SpecialAccessRequirement(string markerForSemiProtectedEndpoints)
        {
            MarkerForSemiProtectedEndpoints = markerForSemiProtectedEndpoints;
        }
    }

    public class RegularClientAuthorizationHandler : AuthorizationHandler<SpecialAccessRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SpecialAccessRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "client_id" && c.Value == "m2m"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class ShortClientAuthorizationHandler : AuthorizationHandler<SpecialAccessRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ShortClientAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SpecialAccessRequirement requirement)
        {
            var path = _httpContextAccessor.HttpContext.Request.Path.Value;

            if (context.User.HasClaim(c => c.Type == "client_id" && c.Value == "m2m.short")
                && path?.Contains(requirement.MarkerForSemiProtectedEndpoints) == true)
            {
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class Policies
    {
        public static AuthorizationPolicy MyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new SpecialAccessRequirement("semi-protected"))
                .Build();
        }
    }
}
