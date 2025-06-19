using Microsoft.EntityFrameworkCore;
using MyBlog.Models.Data;

namespace MyBlog.DataAccessor.EFCore
{
    public class BlogDbContext : DbContext
    {

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Thought> Thoughts { get; set; }
        public DbSet<HotMap> HotMap { get; set; }
        public DbSet<ArticleLikeRecord> ArticleLikeRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ArticleLikeRecord>().HasKey(p => new { p.ArticleId, p.IpAddress });
        }
    }
}
