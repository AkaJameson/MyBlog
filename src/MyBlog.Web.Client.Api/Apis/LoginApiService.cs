using Microsoft.AspNetCore.Components.Authorization;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Web.Client.Api.Identity;
using System.Net.Http.Json;

namespace MyBlog.Web.Client.Api.Apis
{
    //带上这个标签会自动注入服务类
    [Api]
    public class LoginApiService
    {
        private readonly HttpClient _httpClient;
        private readonly Session _session;
        private ApiAuthenticationStateProvider stateProvider;
        public LoginApiService(IHttpClientFactory httpClientFactory, Session session,AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClientFactory.CreateClient("client");
            _session = session;
            stateProvider = authenticationStateProvider as ApiAuthenticationStateProvider;
        }

        public async Task<OperateResult<LoginInfo>> LoginAsync(LoginModel login)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Login/Login", login);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperateResult<LoginInfo>>();
            _session.userName = result.Data.Account;
            _session.ExpiresAt = result.Data.Expired;
            stateProvider.NotifyUserAuthentication();
            return result;
        }

        public async Task<OperateResult> LogoutAsync()
        {
            var response = await _httpClient.PostAsync("/api/Login/Logout", null);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperateResult>();
            _session.ExpiresAt = DateTime.MinValue;
            stateProvider.NotifyUserAuthentication();
            return result;
        }

    }
}
