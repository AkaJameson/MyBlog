using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.Models
{
    public class ThoughtAdd
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
    }

    public class ThoughtEdit
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
    }

    public class ThoughtQuery 
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
