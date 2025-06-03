namespace MyBlog.Models.Dto
{
    public class LoginInfo
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; }
        public DateTime Expired { get; set; }
    }
}
