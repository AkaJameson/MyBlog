namespace MyBlog.Components.Layout
{
    public partial class NavMenu
    {
        public List<NavItem> NavItems { get; set; } = new List<NavItem>
        {
            new NavItem { Herf = "/posts", Icon = "fa-solid fa-book", Text = "文章管理" },
            new NavItem { Herf = "/category", Icon = "fa-solid fa-list", Text = "类别管理" },
            new NavItem { Herf = "/garbages", Icon = "fa-solid fa-trash", Text = "回收站" },
        };
    }
    public class NavItem
    {
        public string Herf { get; set; }
        public string Icon { get; set; }
        public string Text { get; set; }
    }
}
