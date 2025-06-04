namespace MyBlog.Models.Models
{
    public class ArticleQuery
    {
        public string? Title { get; set; }
        public string? CategoryName { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public bool IsPublished { get; set; } = true;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
