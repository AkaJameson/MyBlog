using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Pages.Manage
{
    public partial class CategoryManagePage
    {
        [Inject]
        private CategoryApiService CategoryApiService { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }
        private List<CategoryInfo> Categories { get; set; } = new List<CategoryInfo>();
        private int PageIndex { get; set; } = 1;
        private int PageSize { get; set; } = 20;
        private int TotalCount { get; set; }
        private int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);
        private string CategoryName;
        private Modal ModalRef { get; set; }
        private Modal AddModalRef { get; set; }
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
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await QueryCategroies();
        }

        private async Task OnPageLinkClick(int index)
        {
            PageIndex = index;
            await QueryCategroies();
        }
        private async Task OnPageSizeChanged(SelectedItem item)
        {
            await QueryCategroies();
        }
        private async Task QueryCategroies()
        {
            var result = await CategoryApiService.QueryCategoryAsync(new Models.Models.CategoryQuery
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
            });
            TotalCount = result.Data.TotalCount;
            Categories = result.Data.CategoryInfos ?? new List<CategoryInfo>();
        }
        private async Task UpdateCategory(int id)
        {
            if (CategoryName == Categories.FirstOrDefault(c => c.Id == id).CategoryName)
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "分类名称未修改",
                    Color = Color.Warning,
                });
                return;
            }
            var result = await CategoryApiService.UpdateCategoryAsync(new Models.Models.CategoryEdit
            {
                Id = id,
                CategoryName = CategoryName
            });
            if (result.Succeeded)
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "分类修改成功",
                    Color = Color.Success,
                });
                PageIndex = 1;
                await QueryCategroies();
            }
        }
        private async Task DeleteCategory(int id)
        {
            var result = await CategoryApiService.DeleteCategoryAsync(id);
            if (result.Succeeded)
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "分类删除成功",
                    Color = Color.Success,
                });
                PageIndex = 1;
                await QueryCategroies();
            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Content = result.Message,
                    Color = Color.Warning,
                });
            }
        }
        private async Task AddCategory()
        {
            var result = await CategoryApiService.AddCategoryAsync(new Models.Models.CategoryAdd
            {
                CategoryName = CategoryName
            });
            if (result.Succeeded)
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "分类添加成功",
                    Color = Color.Success,
                });
                PageIndex = 1;
                await QueryCategroies();
            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Content = result.Message,
                    Color = Color.Warning,
                });
            }
        }
        private Task OnDialogClosed()
        {
            CategoryName = string.Empty;
            return Task.CompletedTask;
        }

    }
}
