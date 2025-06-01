namespace MyBlog.Models.Models
{
    public class ArticleUpdate
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsPublished { get; set; }

    }
}
