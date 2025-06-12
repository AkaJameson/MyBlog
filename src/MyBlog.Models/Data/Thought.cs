using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Models.Data
{
    public class Thought
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        public DateTime PublishTime { get; set; }

        public int Like { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
