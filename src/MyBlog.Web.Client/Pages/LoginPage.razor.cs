using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MyBlog.Web.Client.Api.Apis;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MyBlog.Web.Client.Pages
{
    public partial class LoginPage
    {
        [Inject]
        [NotNull]
        private NavigationManager? NavigationManager { get; set; }
        [Inject]
        [NotNull]
        private MessageService? MessageService { get; set; }
        [Inject]
        public LoginApiService LoginService { get; set; }

        private LoginModel Model { get; set; } = new LoginModel();

        public class LoginModel
        {
            [Required(ErrorMessage = "请输入账号")]
            public string Account { get; set; } = string.Empty;

            [Required(ErrorMessage = "请输入密码")]
            public string Password { get; set; } = string.Empty;
        }

        private async Task LoginClick(EditContext context)
        {
            try
            {
                var result = await LoginService.LoginAsync(new Models.Models.LoginModel
                {
                    Account = Model.Account,
                    Password = Model.Password
                });
                if (result?.Succeeded == true)
                {
                    NavigationManager.NavigateTo("/dashboard");
                }
                else
                {
                    await MessageService.Show(new MessageOption()
                    {
                        Content = result?.Message ?? "未知错误",
                        Icon = "fa - light fa - xmark",
                        Color = Color.Warning
                    });
                }
            }
            catch (Exception ex)
            {
                await MessageService.Show(new MessageOption()
                {
                    Content = "内部异常",
                    Icon = "fa - light fa - xmark",
                    Color = Color.Warning
                });
            }
        }
    }
}
