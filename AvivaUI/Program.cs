using AvivaUI;
using AvivaUI.Components;
using AvivaUI.Proxies;
using AvivaUI.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Configurations
IConfiguration config = (IConfiguration)builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

CConfig cconfig = new(config);
builder.Services.AddSingleton<CConfig>(cconfig);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();

builder.Services.AddHttpClient("AvivaApi", client =>
{
    client.BaseAddress = new Uri(cconfig.ApiAddress);
});

builder.Services.AddScoped<IProductsProxy,ProductsProxy>();
builder.Services.AddScoped<IProductServices,ProductServices>();
builder.Services.AddScoped<IOrderPagoProxy,OrderPagoProxy>();
builder.Services.AddScoped<IOrderPagoServices, OrderPagoServices>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
