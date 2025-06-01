namespace MyBlog.Models.Dto
{
    public class ArticleDto
    {
        public int TotalCount { get; set; }
        public List<ArticleInfo> articleInfos { get; set; }
    }
    public class ArticleInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 文章Id
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 文章分类Id
        /// </summary>
        public int CategroyId { get; set; }
        /// <summary>
        /// 文章分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 浏览数
        /// </summary>
        public int views { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
