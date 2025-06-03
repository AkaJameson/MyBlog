using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyBlog.Web.Client.Api;
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
