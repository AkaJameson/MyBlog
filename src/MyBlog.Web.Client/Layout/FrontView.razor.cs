using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Layout
{
    public partial class FrontView
    {
        [Inject]
        private ArticleApiService ArticleApiService { get; set; }
        [Inject]
        private HotMapApiService HotMapApiService { get; set; }
        [Inject]
        private IConfiguration Configuration { get; set; }
        private List<ArticleInfo> ArticleInfos { get; set; }
        private string BeiAnHao { get; set; }
        private BlogDetailCount BlogDetailCount { get; set; } = new();
        protected override async Task OnParametersSetAsync()
        {
            var result = await ArticleApiService.QueryPopularArticle();
            if (result.Code == 200)
            {
                ArticleInfos = result.Data;
            }
            await GetBlogDetailCount();
            BeiAnHao = Configuration.GetValue<string>("BeiAnHao")?? "";

        }
        private async Task<Dictionary<DateTime, int>> QueryHotMap(int year)
        {
            var result = await HotMapApiService.GetHotMapData(year);
            return result;
        }
        protected async Task GetBlogDetailCount()
        {
            var result = await ArticleApiService.GetBlogDetailCount();
            if (result.Code == 200)
            {
                BlogDetailCount = result.Data;
            }
        }
    }
}
