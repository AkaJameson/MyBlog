using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Pages
{
    public  class ArticlesPageBase:ComponentBase
    {
        [Inject]
        public ArticleApiService ArticleApiService { get; set; }

        protected List<ArticleInfo> Articles { get; set; } = new();
        protected int CurrentPage { get; set; } = 1;
        protected int PageSize { get; set; } = 10;
        protected int TotalCount { get; set; }
        protected int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        protected override async Task OnParametersSetAsync()
        {
            await LoadArticles();
        }

        private async Task LoadArticles()
        {
            var result = await ArticleApiService.QueryPublishArticleAsync(new Models.Models.ArticleQuery
            {
                PageIndex = CurrentPage,
                PageSize = PageSize
            });
            if (result.Code == 200)
            {
                Articles = result.Data.articleInfos;
                TotalCount = result.Data.TotalCount;
                StateHasChanged();
            }
        }

        protected async Task ChangePage(int page)
        {
            if (page < 1 || page > TotalPages) return;
            CurrentPage = page;
            await LoadArticles();
        }
    }
}
