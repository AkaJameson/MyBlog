using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.Models
{
    public class ArticalAdd
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublished { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }

    }
}
