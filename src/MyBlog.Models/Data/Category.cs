using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Models.Data
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
