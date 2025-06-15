using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;

namespace MyBlog.Web.Client.Pages
{
    public partial class HomePage
    {
        [CascadingParameter]
        public List<ArticleInfo> Articles { get; set; }
    }
}
