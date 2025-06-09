using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Pages.Manage
{
    public partial class ThoughtManagePage
    {
        [Inject]
        private ThoughtApiService ThoughtApiService { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }

        private List<ThoughtInfo> Thoughts { get; set; } = new List<ThoughtInfo>();
        private int PageIndex { get; set; } = 1;
        private int PageSize { get; set; } = 20;
        private int TotalCount { get; set; }
        private int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);

        private string ThoughtContent;
        private Modal? ModalRef { get; set; }
        private Modal? AddModalRef { get; set; }
        private int CurrentThoughtId { get; set; }

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
            await base.OnInitializedAsync();
            await QueryThoughts();
        }

        private async Task OnPageLinkClick(int index)
        {
            PageIndex = index;
            await QueryThoughts();
        }

        private async Task OnPageSizeChanged(SelectedItem item)
        {
            await QueryThoughts();
        }

        private async Task QueryThoughts()
        {
            var result = await ThoughtApiService.QueryThoughtsAsync(new ThoughtQuery
            {
                PageIndex = PageIndex,
                PageSize = PageSize
            });

            if (result.Succeeded)
            {
                TotalCount = result.Data.TotalCount;
                Thoughts = result.Data.ThoughtInfos ?? new List<ThoughtInfo>();
            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Content = $"查询失败: {result.Message}",
                    Color = Color.Danger
                });
            }
        }

        private async Task UpdateThought(int id)
        {
            if (string.IsNullOrWhiteSpace(ThoughtContent))
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "说说内容不能为空",
                    Color = Color.Warning
                });
                return;
            }

            var originalThought = Thoughts.FirstOrDefault(t => t.Id == id);
            if (originalThought == null || ThoughtContent == originalThought.Content)
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "说说内容未修改",
                    Color = Color.Warning
                });
                return;
            }

            var result = await ThoughtApiService.UpdateThoughtAsync(new ThoughtEdit
            {
                Id = id,
                Content = ThoughtContent
            });

            if (result.Succeeded)
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "说说更新成功",
                    Color = Color.Success
                });
                await QueryThoughts();
            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Content = $"更新失败: {result.Message}",
                    Color = Color.Danger
                });
            }
        }

        private async Task DeleteThought(int id)
        {
            var result = await ThoughtApiService.DeleteThoughtAsync(id);

            if (result.Succeeded)
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "说说删除成功",
                    Color = Color.Success
                });
                PageIndex = 1;
                await QueryThoughts();
            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Content = $"删除失败: {result.Message}",
                    Color = Color.Danger
                });
            }
        }

        private async Task AddThought()
        {
            if (string.IsNullOrWhiteSpace(ThoughtContent))
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "说说内容不能为空",
                    Color = Color.Warning
                });
                return;
            }

            var result = await ThoughtApiService.AddThoughtAsync(new ThoughtAdd
            {
                Content = ThoughtContent
            });

            if (result.Succeeded)
            {
                await MessageService.Show(new MessageOption
                {
                    Content = "说说添加成功",
                    Color = Color.Success
                });
                PageIndex = 1;
                await QueryThoughts();
            }
            else
            {
                await MessageService.Show(new MessageOption
                {
                    Content = $"添加失败: {result.Message}",
                    Color = Color.Danger
                });
            }
        }

        private Task OnDialogClosed()
        {
            ThoughtContent = string.Empty;
            CurrentThoughtId = 0;
            return Task.CompletedTask;
        }
    }
}