using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;

namespace MyBlog.Web.Client.Components
{
    public partial class ArticleCard
    {
        [Parameter] public ArticleInfo Article { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private void NavigateToDetail(int articleId)
        {
            NavigationManager.NavigateTo($"/view/article/{articleId}");
        }
    }
}
