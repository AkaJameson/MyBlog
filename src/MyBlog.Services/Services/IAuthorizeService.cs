using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Utilites;

namespace MyBlog.Services.Services
{
    public interface IAuthorizeService
    {
        Task<OperateResult<LoginInfo>> LoginAsync(LoginModel loginModel);
        Task<OperateResult> LogoutAsync();
    }
}