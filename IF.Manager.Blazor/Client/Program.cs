using Arbimed.Core.ApiClient;
using IF.Manager.Blazor.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
builder.Services.AddScoped(sp => http);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton(new ApiClient(http, builder.HostEnvironment.BaseAddress));


builder.Services.AddMudServices();

await builder.Build().RunAsync();
