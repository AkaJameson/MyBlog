using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Web.Client.Api.Apis;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Web;

namespace MyBlog.Web.Client.Pages.Manage
{
    public partial class BlogWritePage
    {
        [Inject]
        private CategoryApiService CategoryApiService { get; set; }
        [Inject]
        private ArticleApiService ArticleApiService { get; set; }
        private ValidateForm _form;
        [Parameter]
        [SupplyParameterFromQuery]
        public string ModelJson { get; set; }
        [Parameter]
        public BlogWriteModel Model { get; set; } = new();
        private List<SelectedItem> _categories = new();
        private List<SelectedItem> publishStatus = new()
        {
            new SelectedItem("true", "发布"),
            new SelectedItem("false", "草稿")
        };

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(ModelJson))
            {
                var decoded = HttpUtility.UrlDecode(ModelJson);
                Model = JsonSerializer.Deserialize<BlogWriteModel>(decoded);
            }

            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            var result = await CategoryApiService.GetCategoryListAsync();
            if (result.Code == 200)
            {
                _categories = result.Data.Select(p => new SelectedItem
                {
                    Text = p.CategoryName,
                    Value = p.Id.ToString()
                }).ToList();
            }
            StateHasChanged();
        }

        private async Task HandleValidSubmit()
        {
            if (_form != null)
            {
                if (_form.Validate())
                {
                    if (Model.Id.HasValue)
                    {
                        var updateModel = new ArticleUpdate
                        {
                            Id = Model.Id.Value,
                            Title = Model.Title,
                            Content = Model.Content,
                            CategoryId = Model.CategoryId,
                            IsPublished = Model.IsPublished
                        };
                        var updResult = await ArticleApiService.UpdateArticleAsync(updateModel);
                        if (updResult.Code == 200)
                        {
                            Model = new BlogWriteModel();
                        }
                    }
                    else
                    {

                        var addModel = new ArticalAdd
                        {
                            Title = Model.Title,
                            Content = Model.Content,
                            CategoryId = Model.CategoryId,
                            IsPublished = Model.IsPublished
                        };

                        var result = await ArticleApiService.AddArticleAsync(addModel);
                        if (result.Code == 200)
                        {

                            Model = new BlogWriteModel(); // 重置表单
                        }
                    }
                }
            }

        }


    }
    public class BlogWriteModel
    {
        [Required(ErrorMessage = "请输入标题")]
        public string Title { get; set; }
        [Required(ErrorMessage = "请输入内容")]
        public string Content { get; set; }
        [Required(ErrorMessage = "请选择分类")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "请选择标签")]
        public bool IsPublished { get; set; }
        public int? Id { get; set; }
    }
}
