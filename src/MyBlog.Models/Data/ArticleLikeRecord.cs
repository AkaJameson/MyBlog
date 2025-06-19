namespace MyBlog.Models.Data
{
    public class ArticleLikeRecord
    {
        public int ArticleId { get; set; }
        public string IpAddress { get; set; }
        public DateTime LikeTime { get; set; }

    }
}
