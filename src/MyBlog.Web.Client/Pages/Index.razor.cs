using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Web.Client.Api.Apis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Web.Client.Pages
{
    public partial class IndexBase : ComponentBase
    {
        [Inject]
        private ArticleApiService ArticleService { get; set; }

        [Inject]
        private CategoryApiService CategoryService { get; set; }

        [Inject]
        private ThoughtApiService ThoughtService { get; set; }

        protected bool isLoading = true;
        protected List<ArticleInfo> latestArticles = new();
        protected List<ThoughtInfo> latestThoughts = new();
        protected List<CategoryInfo> categories = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            isLoading = false;
        }

        private async Task LoadData()
        {
            // 获取最新文章
            var articleResult = await ArticleService.QueryPublishArticleAsync(new ArticleQuery
            {
                PageIndex = 1,
                PageSize = 3
            });

            if (articleResult.Succeeded && articleResult.Data != null)
            {
                latestArticles = articleResult.Data.articleInfos;
            }

            // 获取最新碎碎念
            var thoughtResult = await ThoughtService.GetThoughtListAsync();
            if (thoughtResult.Succeeded && thoughtResult.Data != null)
            {
                latestThoughts = thoughtResult.Data.Take(5).ToList();
            }

            // 获取分类
            var categoryResult = await CategoryService.GetCategoryListAsync();
            if (categoryResult.Succeeded && categoryResult.Data != null)
            {
                categories = categoryResult.Data;
            }
        }
    }
}
