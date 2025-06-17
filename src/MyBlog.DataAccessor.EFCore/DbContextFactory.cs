using Microsoft.EntityFrameworkCore;

namespace MyBlog.DataAccessor.EFCore
{
    public class DbContextFactory : IDbContextFactory<BlogDbContext>
    {
        public BlogDbContext CreateDbContext()
        {
            var connectionString = "Server=8.140.193.236;Database=MyBlog;User=root;Port=3306;Password=CaoNiMa99373;";
            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
            optionsBuilder.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));

            return new BlogDbContext(optionsBuilder.Options);
        }
    }
}
