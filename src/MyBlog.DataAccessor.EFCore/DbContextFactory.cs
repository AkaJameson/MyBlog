using Microsoft.EntityFrameworkCore;

namespace MyBlog.DataAccessor.EFCore
{
    public class DbContextFactory : IDbContextFactory<BlogDbContext>
    {
        public BlogDbContext CreateDbContext()
        {
            var connectionString = "Server=127.0.0.1;Port=3306;Database=MyBlog;User=root;Password=123456;Charset=utf8mb4";
            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
            optionsBuilder.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));

            return new BlogDbContext(optionsBuilder.Options);
        }
    }
}
