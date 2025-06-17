using Microsoft.AspNetCore.Components;
using MyBlog.Models.Data;
using MyBlog.Models.Dto;

namespace MyBlog.Web.Client.Components
{
    public class PopularArticlesBase:ComponentBase
    {
        [Parameter]
        public List<ArticleInfo> ArticleInfos { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected override void OnParametersSet()
        {
            if (ArticleInfos == null)
            {
                ArticleInfos = new List<ArticleInfo>()
                {
                    new ArticleInfo{likes = 1000,Title = ".NET8性能优化分析",views=8000},
                    new ArticleInfo{likes = 1000,Title = "Blazor入门导航",views=8000},
                    new ArticleInfo{likes = 1000,Title = "多线程与异步",views=8000},
                    new ArticleInfo{likes = 1000,Title = "吃饭的艺术",views=8000},
                    new ArticleInfo{likes = 1000,Title = "睡觉的艺术",views=8000}
                };
            }

        }
        protected void HandleArticleClick(ArticleInfo article)
        {
            NavigationManager.NavigateTo($"/view/article/{article.Id}?LikeCount={article.likes}");
        }

    }
    
}
