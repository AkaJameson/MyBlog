using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace MyBlog.Web.Client.Api.Identity
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly Session _session;
        public ApiAuthenticationStateProvider(Session session)
        {
            _session = session;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!_session.IsAuthenticated)
            {
                var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                return Task.FromResult(new AuthenticationState(anonymousUser));
            }
            var identity = new ClaimsIdentity(new[]
            {
                  new Claim(ClaimTypes.Name, _session.userName)
            }, "apiauth");

            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(
              new ClaimsIdentity(identity))));
        }
        public void NotifyUserAuthentication()
        {
            var authState = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var anonymous = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(anonymous);
        }
    }
}
