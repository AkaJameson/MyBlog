using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Pages.Manage
{
    public partial class BlogGarbageManage
    {
        [Inject]
        private ArticleApiService ArticleApiService { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }
        private List<Blog> Blogs { get; set; } = new List<Blog>();
        private string? CategoryName { get; set; }
        private DateTime? StartTime { get; set; }
        private DateTime? EndTime { get; set; }
        public string? Title { get; set; }
        private int PageIndex { get; set; } = 1;
        private int PageSize { get; set; } = 20;
        private int TotalCount { get; set; }
        private int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);
        private SelectedItem Status = new SelectedItem("1", "已发布");
        private List<SelectedItem> StatusOptions = new()
        {
            new SelectedItem("1", "已发布"),
            new SelectedItem("0", "草稿")
        };
        // 每页条数选项
        private List<SelectedItem> PageSizeOptions = new()
        {
            new SelectedItem("20", "20 条/页"),
            new SelectedItem("50", "50 条/页"),
            new SelectedItem("100", "100 条/页")
        };
        private string PageInfoText => $"每页 {PageSize} 条，共 {TotalCount} 条数据";
        private string PageSizeStr
        {
            get => PageSize.ToString();
            set
            {
                PageSize = int.Parse(value);
                PageIndex = 1; // 重置到第一页
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await QueryBlogAsync();
        }
        private async Task OnPageLinkClick(int index)
        {
            PageIndex = index;
            await QueryBlogAsync();
        }
        private async Task OnPageSizeChanged(SelectedItem item)
        {
            await QueryBlogAsync();
        }
        protected async Task QueryBlogAsync()
        {
            var result = await ArticleApiService.QueryGarbageArticleAsync(new Models.Models.ArticleQuery
            {
                CategoryName = CategoryName,
                StartTime = StartTime.HasValue ? StartTime.Value.ToString("yyyy-MM-dd") : null,
                EndTime = EndTime.HasValue ? EndTime.Value.ToString("yyyy-MM-dd") : null,
                IsPublished = Status.Value == "1" ? true : false,
                PageIndex = PageIndex,
                PageSize = PageSize,
                Title = Title
            });
            if (result.Code == 200)
            {
                TotalCount = result.Data.TotalCount;
                Blogs = result.Data.articleInfos.Select(p => new Blog
                {
                    CategoryId = p.CategroyId,
                    CategoryName = p.CategoryName,
                    CreateTime = p.CreateTime,
                    Title = p.Title,
                    views = p.views,
                    Id = p.Id,
                }).ToList();

            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Color = Color.Danger,
                    Content = result.Message,
                });
            }
        }

        /// <summary>
        /// 打开文章查看页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task RecoverPost(int id)
        {
            var result = await ArticleApiService.RecoverArticleAsync(id);
            if (result.Code == 200)
            {
                await MessageService.Show(new MessageOption
                {
                    Color = Color.Success,
                    Content = "恢复成功"
                });
                PageIndex = 1;
                await QueryBlogAsync();
            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Color = Color.Danger,
                    Content = result.Message
                });
            }
        }
    }
}
