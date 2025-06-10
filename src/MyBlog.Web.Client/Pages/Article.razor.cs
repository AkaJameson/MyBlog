using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Pages
{
    public partial class ArticleBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private ArticleApiService ArticleService { get; set; }

        protected bool isLoading = true;
        protected ArticleInfo article;
        protected string errorMessage;

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            await LoadArticle();
            isLoading = false;
        }

        private async Task LoadArticle()
        {
            var result = await ArticleService.QuerySingleArticle(Id);

            if (result.Succeeded)
            {
                article = result.Data;
            }
            else
            {
                errorMessage = result.Message;
            }
        }

        protected void ShareArticle(string platform)
        {
            switch (platform)
            {
                case "weibo":
                    break;
                case "wechat":
                    break;
                case "twitter":
                    break;
            }
        }
        protected void ShareToWeibo() => ShareArticle("weibo");
        protected void ShareToWechat() => ShareArticle("wechat");
        protected void ShareToTwitter() => ShareArticle("twitter");

    }
}
