namespace MyBlog.Web.Client.Layout
{
    public partial class NavMenu
    {
        public List<NavItem> NavItems { get; set; } = new()
        {
            new NavItem { Herf ="/write",  Icon="fa-solid fa-pen" ,  Text = "写博客"},
            new NavItem { Herf = "/posts", Icon = "fa-solid fa-book", Text = "文章管理" },
            new NavItem { Herf = "/category", Icon = "fa-solid fa-list", Text = "类别管理" },
            new NavItem { Herf = "/thoughts", Icon = "fa-solid fa-trash", Text = "碎碎念" },
            new NavItem { Herf = "/garbages", Icon = "fa-solid fa-trash", Text = "回收站" }, 
        };

        private bool IsCollapsed { get; set; } = false;

        private void ToggleSidebar()
        {
            IsCollapsed = !IsCollapsed;
        }
    }
    public class NavItem
    {
        public string Herf { get; set; }
        public string Icon { get; set; }
        public string Text { get; set; }
    }
}
