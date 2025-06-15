using System.Net.Http.Json;

namespace MyBlog.Web.Client.Api.Apis
{
    [Api]
    public class HotMapApiService
    {
        private HttpClient _httpClient;
        public HotMapApiService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("client");
        }
        public async Task<Dictionary<DateTime,int>> GetHotMapData(int year)
        {
            var response = await _httpClient.GetAsync($"/api/HotMap/GetHotMap?year={year}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperateResult<Dictionary<string, int>>>();
            var parsed = result.Data.ToDictionary(
              kv => DateTime.Parse(kv.Key),
              kv => kv.Value
            );
            return parsed;
        }
    }
}
