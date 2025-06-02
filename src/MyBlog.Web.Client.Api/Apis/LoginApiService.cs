using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using System.Net.Http.Json;

namespace MyBlog.Web.Client.Api.Apis
{
    //带上这个标签会自动注入服务类
    [Api]
    public class LoginApiService
    {
        private readonly HttpClient _httpClient;

        public LoginApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("client");
        }

        public async Task<OperateResult<LoginInfo>> LoginAsync(LoginModel login)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Login/LoginAsync", login);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<LoginInfo>>();
        }

        public async Task<OperateResult> LogoutAsync()
        {
            var response = await _httpClient.PostAsync("/api/Login/LogoutAsync", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

    }
}
