using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Pages
{
    public class ArticleViewListBase : ComponentBase
    {
        [Inject]
        public ArticleApiService ArticleApiService { get; set; }

        protected int PageIndex { get; set; } = 1;
        protected int PageSize { get; set; } = 20;
        protected int TotalPage { get; set; }

        protected List<ArticleInfo> ArticleInfos { get; set; }
        protected Pagination Pegination { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var result = await ArticleApiService.QueryPublishArticleAsync(new Models.Models.ArticleQuery
            {
                PageIndex = 1,
                PageSize = 20
            });
            if (result.Code != 200)
            {
                TotalPage = 0;
                ArticleInfos = new List<ArticleInfo>();
            }
            else
            {
                TotalPage = ((int)result.Data.TotalCount / PageSize) + 1;
                PageIndex = 1;
                PageSize = 20;
                ArticleInfos = result.Data.articleInfos;
            }
            await base.OnInitializedAsync();
        }

      


    }
}
