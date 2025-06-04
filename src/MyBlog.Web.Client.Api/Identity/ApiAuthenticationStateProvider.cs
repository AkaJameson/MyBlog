using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace MyBlog.Web.Client.Api.Identity
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        public ApiAuthenticationStateProvider(ILocalStorageService session)
        {
            localStorageService = session;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var _session = await localStorageService.GetItemAsync<Session>("identity");
            if (_session == null || !_session.IsAuthenticated)
            {
                var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymousUser);
            }
            var identity = new ClaimsIdentity(new[]
            {
                  new Claim(ClaimTypes.Name, _session.userName)
            }, "apiauth");

            return new AuthenticationState(new ClaimsPrincipal(
              new ClaimsIdentity(identity)));
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
