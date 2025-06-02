using Microsoft.EntityFrameworkCore;
using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Utilites;
using Si.Logging;

namespace MyBlog.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddLogging();
         
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddCors(option =>
                {
                    option.AddPolicy("AllowAll", builder =>
                    {
                        builder.SetIsOriginAllowed(_ => true)
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
                });
            }
            else
            {
                var hostUrls = builder.Configuration.GetValue<string>("AllowedHosts").Split(";").ToArray();
                builder.Services.AddCors(option =>
                {
                    option.AddPolicy("AllowAll", builder =>
                    {
                        builder.WithOrigins(hostUrls)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                    });
                });
            }
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<BlogDbContext>(option =>
            {
                option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddAllApisByScan();
            builder.Services.AddAuthorization();
            builder.Services.AddAntiforgery();
            builder.Services.AddAuthentication("X-Auth-Token")
            .AddCookie("X-Auth-Token", option =>
            {
                option.LoginPath = "/Login";
                option.ExpireTimeSpan = TimeSpan.FromHours(3);
                option.SlidingExpiration = true;
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseAntiforgery();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                    if (exception != null)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsJsonAsync(new OperateResult<object>
                        {
                            Code = 500,
                            Message = "An unexpected error occurred.",
                            Data = app.Environment.IsDevelopment() ? exception.Error.Message : string.Empty
                        });
                    }
                });
            });
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
