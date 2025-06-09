using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyBlog.Web.Client.Api.Apis
{
    [Api]
    public class ThoughtApiService
    {
        private readonly HttpClient _httpClient;

        public ThoughtApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("client");
        }

        public async Task<OperateResult> AddThoughtAsync(ThoughtAdd thought)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Thought/AddThought", thought);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

        public async Task<OperateResult> DeleteThoughtAsync(int id)
        {
            var response = await _httpClient.PostAsync($"/api/Thought/DeleteThought?id={id}", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

        public async Task<OperateResult<ThoughtDto>> QueryThoughtsAsync(ThoughtQuery query)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Thought/QueryThoughts", query);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<ThoughtDto>>();
        }

        public async Task<OperateResult> UpdateThoughtAsync(ThoughtEdit thought)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Thought/UpdateThought", thought);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

        public async Task<OperateResult<List<ThoughtInfo>>> GetThoughtListAsync()
        {
            var response = await _httpClient.GetAsync("/api/Thought/GetThoughtList");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<List<ThoughtInfo>>>();
        }
    }
}