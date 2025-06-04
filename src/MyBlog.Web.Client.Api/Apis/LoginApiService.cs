using Blazored.LocalStorage;
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
        private ApiAuthenticationStateProvider stateProvider;
        private readonly ILocalStorageService localStorageService;
        public LoginApiService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClientFactory.CreateClient("client");
            this.localStorageService = localStorageService;
            stateProvider = authenticationStateProvider as ApiAuthenticationStateProvider;
        }

        public async Task<OperateResult<LoginInfo>> LoginAsync(LoginModel login)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Login/Login", login);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperateResult<LoginInfo>>();
            var session = new Session() { userName = result?.Data?.Account, ExpiresAt = result.Data.Expired };
            await localStorageService.SetItemAsync<Session>("identity", session);
            stateProvider.NotifyUserAuthentication();
            return result;
        }

        public async Task<OperateResult> LogoutAsync()
        {
            var response = await _httpClient.PostAsync("/api/Login/Logout", null);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperateResult>();
            await localStorageService.RemoveItemAsync("identity");
            stateProvider.NotifyUserAuthentication();
            return result;
        }

    }
}
