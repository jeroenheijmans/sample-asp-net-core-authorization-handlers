using Microsoft.AspNetCore.Authorization;

namespace Foo.Api
{
    public static class Policies
    {
        public static AuthorizationPolicy MyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        }
    }
}
