namespace MyBlog.Models.Dto
{
    public class ThoughtInfo
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string PublishTime { get; set; }
    }

    public class ThoughtDto
    {
        public int TotalCount { get; set; }
        public List<ThoughtInfo> ThoughtInfos { get; set; }
    }
}
