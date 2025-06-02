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
            var apiAddress = builder.Configuration.GetValue<string>("ApiAddress");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiAddress) });
            builder.Services.AddAllApis()
                            .AddHttpClient("client")
                            .ConfigureHttpClient(client => client.BaseAddress = new Uri(apiAddress));
            await builder.Build().RunAsync();
        }
    }
}
