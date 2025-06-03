namespace MyBlog.Web.Client.Api.Identity
{
    public class Session
    {
        public string userName { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; } = DateTime.MinValue;
        public bool IsAuthenticated
        {
            get
            {
                return DateTime.Now < ExpiresAt;
            }
        }
    }

}
