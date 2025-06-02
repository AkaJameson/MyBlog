using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Services.Services;
using MyBlog.Utilites;

namespace MyBlog.WebApi.Controllers
{
    [ApiController]
    public class LoginController : DefaultController
    {
        private readonly IAuthorizeService _authorizeService;
        public LoginController(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }

        [HttpPost]
        public async Task<OperateResult<LoginInfo>> LoginAsync([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed<LoginInfo>("登录失败，请检查输入");
            }
            var result =await _authorizeService.LoginAsync(loginModel: login);
            return result;
        }
        [Authorize]
        [HttpPost]
        public async Task<OperateResult> LogoutAsync()
        {
            var result = await _authorizeService.LogoutAsync();
            return result;
        }


    }
}
