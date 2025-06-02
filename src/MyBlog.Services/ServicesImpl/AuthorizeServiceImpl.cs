using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Services.Services;
using MyBlog.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

            var identity = new ClaimsIdentity(claims, "X-Auth-Cookie");
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(principal);
            return OperateResult.Successed(new LoginInfo() { Account = loginModel.Account });
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<OperateResult> LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync("X-Auth-Cookie");
            return OperateResult.Successed();
        }
    }
}
