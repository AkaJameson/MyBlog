using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using System.Net.Http.Json;

namespace MyBlog.Web.Client.Api.Apis
{
    [Api]
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("client");
        }

        public async Task<OperateResult> AddCategoryAsync(CategoryAdd category)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Category/AddCategory", category);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

        public async Task<OperateResult> DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.PostAsync($"/api/Category/DeleteCategory?id={id}", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

        public async Task<OperateResult<CategoryDto>> QueryCategoryAsync(CategoryQuery query)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Category/QueryCategory", query);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<CategoryDto>>();
        }

        public async Task<OperateResult> UpdateCategoryAsync(CategoryEdit category)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Category/UpdateCategory", category);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }
        public async Task<OperateResult<List<CategoryInfo>>> GetCategoryListAsync()
        {
            var response = await _httpClient.GetAsync("/api/Category/GetAllCategory");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<List<CategoryInfo>>>();
        }
    }
}