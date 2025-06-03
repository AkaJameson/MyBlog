using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Pages
{
    public partial class BlogManagePage
    {
        [Inject]
        private ArticleApiService ArticleApiService { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }
        private int? CategoryId { get; set; }
        private string? StartTime { get; set; }
        private string? EndTime { get; set; }
        private bool IsPublish { get; set; } = true;
        public string? Title { get; set; }
        protected async Task<QueryData<Blog>> OnQueryAsync(QueryPageOptions options)
        {
            var queryData = new QueryData<Blog>() { TotalCount = 0 };
            var result = await ArticleApiService.QueryArticleAsync(new Models.Models.ArticleQuery
            {
                CategoryId = CategoryId,
                StartTime = StartTime,
                EndTime = EndTime,
                IsPublished = IsPublish,
                PageIndex = options.PageIndex,
                PageSize = options.PageItems,
                Title = Title
            });
            if (result.Code == 200)
            {
                queryData.TotalCount = result.Data.TotalCount;
                queryData.Items = result.Data.articleInfos.Select(p => new Blog
                {
                    CategoryId = p.CategroyId,
                    CategoryName = p.CategoryName,
                    CreateTime = p.CreateTime,
                    Title = p.Title,
                    views = p.views,
                    Id = p.Id,
                });
            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Color = Color.Danger,
                    Content = result.Message,
                });
            }
            return queryData;
        }
    }
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int views { get; set; }
        public string CreateTime { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
