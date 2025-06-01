using MyBlog.Components;
using MyBlog.Utilites;
using Si.Logging;
using MyBlog.DataAccessor.EFCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.DataAccessor.EFCore.UnitofWork;
namespace MyBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.AddLogging();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddBootstrapBlazor();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            builder.Services.AddAllApisByScan();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
            builder.Services.AddAuthentication()
                .AddCookie("X-Auth-Cookie", options =>        
                {
                    options.Cookie.Name = "X-Auth-Cookie";    
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/NotFound";
                    options.LogoutPath = "/logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                });
            builder.Services.AddAuthorization();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
