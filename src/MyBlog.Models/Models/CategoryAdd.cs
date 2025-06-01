using System.Dynamic;

namespace MyBlog.Models.Models
{
    /// <summary>
    /// 新增
    /// </summary>
    public class CategoryAdd
    {
        public string CategoryName { get; set; }
    }
    /// <summary>
    /// 更新
    /// </summary>
    public class CategoryEdit
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
    /// <summary>
    /// 查询
    /// </summary>
    public class CategoryQuery
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 30;
    }
}
