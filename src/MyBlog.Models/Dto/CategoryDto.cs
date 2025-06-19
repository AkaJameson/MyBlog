namespace MyBlog.Models.Dto
{
    public class CategoryDto
    {
        public int TotalCount { get; set; }
        public List<CategoryInfo> CategoryInfos { get; set; }
    }
    /// <summary>
    /// 分类信息
    /// </summary>
    public class CategoryInfo
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int ArticleCount { get; set; }
        public string CreateTime { get; set; }
    }
}
