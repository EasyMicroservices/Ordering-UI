using EasyMicroservices.UI.Ordering;
using EasyMicroservices.UI.Ordering.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Ordering.GeneratedServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new OrderClient("http://localhost:2005", sp.GetService<HttpClient>()));
builder.Services.AddScoped(sp => new FilterOrdersListViewModel(sp.GetService<OrderClient>()));
builder.Services.AddScoped(sp => new AddOrUpdateOrderViewModel(sp.GetService<OrderClient>()));

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});

await builder.Build().RunAsync();
