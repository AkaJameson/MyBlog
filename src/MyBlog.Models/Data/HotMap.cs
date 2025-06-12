using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.Data
{
    public class HotMap
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Key]
        public DateTime Date { get; set; }
        /// <summary>
        /// 操作次数
        /// </summary>
        public int OperateCount { get; set; }
    }
}
