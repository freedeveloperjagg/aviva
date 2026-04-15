using AvivaApi;
using AvivaApi.Bo;
using AvivaApi.Data;
using AvivaApi.Facade;
using AvivaApi.Proxy;
using AvivaApi.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);

// Create Configurations...........................
IConfiguration config = (IConfiguration)builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

CConfig cconfig = new(config);
builder.Services.AddSingleton<CConfig>(cconfig);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Aviva API",
        Version = "v1",
        Description = "Aviva backend REST API",
        Contact = new OpenApiContact
        {
            Name = "Jose Alfredo Garcia Guirado",
            Email = "freedeveloper@hotmail.com"
        }
    });
});

builder.Services.AddHttpClient("Payment");

// Create and open the SQLite in-memory connection
var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
keepAliveConnection.Open();

builder.Services.AddDbContext<AvivaDbContext>(options =>
{
    options.UseSqlite(keepAliveConnection);
});


builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IProductsBo, ProductsBo>();
builder.Services.AddScoped<IProductsFacade, ProductsFacade>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentProxy, PaymentProxy>();
builder.Services.AddScoped<IPaymentCompaniesFacade, PaymentCompaniesFacade>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IPaymentBo, PaymentBo>();
var app = builder.Build();

// Ensure the schema is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AvivaDbContext>();
    db.Database.EnsureCreated();
    db.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Aviva API v1");
        options.RoutePrefix = string.Empty;   // ← Swagger loads at root: https://localhost:{port}/
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API is running");

app.Run();
