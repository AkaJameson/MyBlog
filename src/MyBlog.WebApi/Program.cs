using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Utilites;
using Si.Logging;
using System.Text;
using Microsoft.AspNetCore.Builder;

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
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidIssuer = jwtSettings["Issuer"],
                     ValidateAudience = true,
                     ValidAudience = jwtSettings["Audience"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.Zero
                 };
                 options.Events = new JwtBearerEvents()
                 {
                     OnMessageReceived = context =>
                     {
                         var token = context.Request.Cookies["Access-Token"];
                         if (!string.IsNullOrEmpty(token))
                         {
                             context.Token = token;
                         }
                         return Task.CompletedTask;
                     }
                 };
             });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath=""
            });
            app.MapFallbackToFile("index.html");
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
