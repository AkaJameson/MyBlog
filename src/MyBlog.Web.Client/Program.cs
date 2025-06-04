using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyBlog.Web.Client.Api;
using MyBlog.Web.Client.Api.Identity;
namespace MyBlog.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddBootstrapBlazor();
            builder.Services.AddTransient<CredentialsHandler>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorageAsSingleton();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            var apiAddress = builder.Configuration.GetValue<string>("ApiAddress");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiAddress) });
            builder.Services.AddHttpClient("client", client =>
            {
                client.BaseAddress = new Uri(apiAddress);
            }).AddHttpMessageHandler<CredentialsHandler>();
            builder.Services.AddAllApis();

            await builder.Build().RunAsync();
        }
    }
}
