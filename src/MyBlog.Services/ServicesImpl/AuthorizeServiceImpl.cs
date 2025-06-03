using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Services.Services;
using MyBlog.Utilites;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MyBlog.Services.ServicesImpl
{
    [Api(ServiceLifetimeOption.Scoped)]
    public class AuthorizeServiceImpl : IAuthorizeService
    {
        private readonly IUnitOfWork<BlogDbContext> _unitOfWork;
        private IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        public AuthorizeServiceImpl(IUnitOfWork<BlogDbContext> unitOfWork,
                                    IHttpContextAccessor httpContextAccessor,
                                    IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<OperateResult<LoginInfo>> LoginAsync(LoginModel loginModel)
        {
            if (loginModel == null || loginModel.Account == null || loginModel.Password == null)
            {
                return OperateResult.Failed<LoginInfo>("登录失败，请检查输入");
            }
            var account = _configuration.GetSection("Credentials").GetValue<string>("Account");
            var password = _configuration.GetSection("Credentials").GetValue<string>("Password");
            if (string.IsNullOrEmpty(account) ||string.IsNullOrEmpty(password))
            {
                return OperateResult.Failed<LoginInfo>("登录失败，请联系管理员");
            }
            if ( account != loginModel.Account ||password != loginModel.Password)
            {
                return OperateResult.Failed<LoginInfo>("登录失败，密码错误");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Account),
                new Claim(ClaimTypes.Role, "Admin"),

            };
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                Expires = DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"])),
                SigningCredentials = credentials,
                Claims = claims.ToDictionary(c => c.Type, c => (object)c.Value)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("Access-Token", tokenHandler.WriteToken(token), new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"])),
                SameSite = SameSiteMode.Lax
            });
            return OperateResult.Successed(new LoginInfo() { Account = loginModel.Account });
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<OperateResult> LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync("X-Auth-Token");
            return OperateResult.Successed();
        }
    }
}
