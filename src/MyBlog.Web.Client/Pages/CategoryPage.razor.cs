using Microsoft.AspNetCore.Components;
using MyBlog.Models.Data;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Web.Client.Api.Apis;
using System.Linq;

namespace MyBlog.Web.Client.Pages
{
    public class CategoryPageBase : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private CategoryApiService CategoryApiService { get; set; }

        [Inject]
        private ArticleApiService ArticleApiService { get; set; }
        protected CategoryDto CategoryData { get; set; } = new();
        protected List<ArticleInfo> Articles { get; set; }
        protected CategoryInfo SelectedCategory { get; set; }
        protected bool ShowPopup { get; set; }
        protected bool IsLoading { get; set; }
        protected string ErrorMessage { get; set; }

        // 分类分页参数
        protected int CurrentPage { get; set; } = 1;
        protected int ItemsPerPage { get; set; } = 6;
        protected int TotalPages => CategoryData.TotalCount > 0 ?
            (int)Math.Ceiling((double)CategoryData.TotalCount / ItemsPerPage) : 1;

        // 文章分页参数
        protected int ArticleCurrentPage { get; set; } = 1;
        protected int ArticleItemsPerPage { get; set; } = 5;
        protected int ArticleTotalPages { get; set; } = 1;

        protected override async Task OnInitializedAsync()
        {
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var query = new CategoryQuery
                {
                    PageIndex = CurrentPage,
                    PageSize = ItemsPerPage
                };

                var result = await CategoryApiService.QueryCategoryAsync(query);

                if (result != null && result.Code == 200 && result.Data != null)
                {
                    CategoryData = result.Data;
                }
                else
                {
                    ErrorMessage = result?.Message ?? "获取分类数据失败";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"加载分类时出错: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task ShowArticles(CategoryInfo category)
        {
            SelectedCategory = category;
            ShowPopup = true;
            ArticleCurrentPage = 1; // 重置文章分页
            await LoadArticles();
        }

        private async Task LoadArticles()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                // 假设有获取分类下文章的API
                var articleQuery = new ArticleQuery
                {
                    CategoryName = SelectedCategory.CategoryName,
                    PageIndex = ArticleCurrentPage,
                    PageSize = ArticleItemsPerPage
                };

                var result = await ArticleApiService.QueryPublishArticleAsync(articleQuery);

                if (result != null && result.Code == 200 && result.Data != null)
                {
                    Articles = result.Data.articleInfos;
                    ArticleTotalPages = (int)Math.Ceiling((double)result.Data.TotalCount / ArticleItemsPerPage);
                }
                else
                {
                    ErrorMessage = result?.Message ?? "获取文章列表失败";
                    Articles = new List<ArticleInfo>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"加载文章时出错: {ex.Message}";
                Articles = new List<ArticleInfo>();
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void ClosePopup()
        {
            ShowPopup = false;
            Articles = null;
        }

        protected void GoToArticle(ArticleInfo article)
        {
            NavigationManager.NavigateTo($"/view/article/{article.Id}?LikeCount={article.likes}");
            ClosePopup();
        }

        // 分类分页方法
        protected async Task ChangePage(int page)
        {
            if (page < 1 || page > TotalPages) return;
            CurrentPage = page;
            await LoadCategories();
        }

        // 文章分页方法
        protected async Task ChangeArticlePage(int page)
        {
            if (page < 1 || page > ArticleTotalPages) return;
            ArticleCurrentPage = page;
            await LoadArticles();
        }
    }
}
