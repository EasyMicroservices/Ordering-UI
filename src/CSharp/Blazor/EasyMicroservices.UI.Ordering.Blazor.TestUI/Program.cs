using EasyMicroservices.Domain.Contracts.Common;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Ordering.Blazor.TestUI;
using EasyMicroservices.UI.Ordering.ViewModels.CountingUnits;
using EasyMicroservices.UI.Ordering.ViewModels.Orders;
using EasyMicroservices.UI.Ordering.ViewModels.Products;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Ordering.GeneratedServices;
LoadLanguage("en-US");
BaseViewModel.CurrentApplicationLanguage = "en-US";
BaseViewModel.IsRightToLeft = false;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new OrderClient("http://localhost:2005", sp.GetService<HttpClient>()));
builder.Services.AddScoped(sp => new ProductClient("http://localhost:2005", sp.GetService<HttpClient>()));
builder.Services.AddScoped(sp => new CountingUnitClient("http://localhost:2005", sp.GetService<HttpClient>()));

builder.Services.AddScoped(sp => new FilterOrdersListViewModel(sp.GetService<OrderClient>()));
builder.Services.AddScoped(sp => new AddOrUpdateOrderViewModel(sp.GetService<OrderClient>()));
builder.Services.AddScoped(sp => new FilterProductsListViewModel(sp.GetService<ProductClient>()));
builder.Services.AddScoped(sp => new AddOrUpdateProductViewModel(sp.GetService<ProductClient>(), sp.GetService<CountingUnitClient>()));
builder.Services.AddScoped(sp => new FilterCountingUnitsListViewModel(sp.GetService<CountingUnitClient>()));
builder.Services.AddScoped(sp => new AddOrUpdateCountingUnitViewModel(sp.GetService<CountingUnitClient>()));
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});

await builder.Build().RunAsync();

void LoadLanguage(string languageShortName)
{ 
    BaseViewModel.AppendLanguage("Saving", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Saving"
    }); 
    BaseViewModel.AppendLanguage("Save", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Save"
    });
    BaseViewModel.AppendLanguage("Search", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Search"
    });
    BaseViewModel.AppendLanguage("Processing", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Processing"
    });
    BaseViewModel.AppendLanguage("Id", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Id"
    });
    BaseViewModel.AppendLanguage("Name", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Name"
    });
    BaseViewModel.AppendLanguage("Actions", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Actions"
    });
    BaseViewModel.AppendLanguage("Delete", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Delete"
    });
    BaseViewModel.AppendLanguage("Deleting", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Deleting"
    });
    BaseViewModel.AppendLanguage("Orders", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Orders"
    });
    BaseViewModel.AppendLanguage("Amount", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Amount"
    });
    BaseViewModel.AppendLanguage("Cancel", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Cancel"
    });
    BaseViewModel.AppendLanguage("DeleteQuestion_Content", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Do you really want to delete these records? This process cannot be undone."
    });
    BaseViewModel.AppendLanguage("Ordering_CountingUnit_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Counting Units"
    });
    BaseViewModel.AppendLanguage("Ordering_ValueAddedTax_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Value Added Tax"
    }); 
    BaseViewModel.AppendLanguage("Ordering_ExternalServiceIdentifier_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "External Service Identifier"
    });
    BaseViewModel.AppendLanguage("Ordering_AmountType_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Amount Type"
    });
    BaseViewModel.AppendLanguage("Ordering_Decimal_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Decimal"
    });
    BaseViewModel.AppendLanguage("Ordering_Percent_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Percent"
    });
    BaseViewModel.AppendLanguage("Ordering_CountingUnitType_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Counting Unit Type"
    });
    BaseViewModel.AppendLanguage("Ordering_DeleteCountingUnit_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Delete CountingUnit"
    });
    BaseViewModel.AppendLanguage("Ordering_UpdateCountingUnit_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Update CountingUnit"
    });
    BaseViewModel.AppendLanguage("Ordering_UpdateCountingUnit_Message", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "CountingUnit updated."
    });
    BaseViewModel.AppendLanguage("Ordering_AddCountingUnit_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Add CountingUnit"
    });
    BaseViewModel.AppendLanguage("Ordering_AddCountingUnit_Message", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "CountingUnit added."
    });
    BaseViewModel.AppendLanguage("Ordering_DeleteCountingUnit_Message", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "CountingUnit deleted!"
    });
    BaseViewModel.AppendLanguage("Ordering_DeleteProduct_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Delete Product"
    });
    BaseViewModel.AppendLanguage("Ordering_DeleteProduct_Message", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Product deleted!"
    });
    BaseViewModel.AppendLanguage("Ordering_UpdateProduct_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Update Product"
    });
    BaseViewModel.AppendLanguage("Ordering_UpdateProduct_Message", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Product updated."
    });
    BaseViewModel.AppendLanguage("Ordering_AddProduct_Title", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Add Product"
    });
    BaseViewModel.AppendLanguage("Ordering_AddProduct_Message", new LanguageContract()
    {
        ShortName = languageShortName,
        Value = "Product added."
    });
}