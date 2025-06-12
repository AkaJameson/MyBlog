using Microsoft.AspNetCore.Components;

namespace MyBlog.Web.Client.Shared
{
    public partial class ProfileCard
    {
        [Parameter]
        public int Article { get; set; }
        [Parameter]
        public int Category { get; set; }
        [Parameter]
        public int Thought { get; set; }
    }
}
