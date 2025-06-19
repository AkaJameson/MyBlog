using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Layout
{
    public partial class FrontView
    {
        protected DateTime CurrentTime = DateTime.Now;
        [Inject]
        private ArticleApiService ArticleApiService { get; set; }
        [Inject]
        private HotMapApiService HotMapApiService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private List<ArticleInfo> ArticleInfos { get; set; }
        private Timer? timer;
        private BlogDetailCount BlogDetailCount { get; set; } = new();
        protected override async Task OnParametersSetAsync()
        {
            var result = await ArticleApiService.QueryPopularArticle();
            if (result.Code == 200)
            {
                ArticleInfos = result.Data;
            }
            await GetBlogDetailCount();

        }
        private async Task<Dictionary<DateTime, int>> QueryHotMap(int year)
        {
            var result = await HotMapApiService.GetHotMapData(year);
            return result;
        }
        protected override void OnInitialized()
        {
            timer = new Timer(UpdateTime, null, 0, 1000); // 每秒更新
        }

        protected async Task GetBlogDetailCount()
        {
            var result = await ArticleApiService.GetBlogDetailCount();
            if (result.Code == 200)
            {
                BlogDetailCount = result.Data;
            }
        }

        private void UpdateTime(object? state)
        {
            CurrentTime = DateTime.Now;
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
