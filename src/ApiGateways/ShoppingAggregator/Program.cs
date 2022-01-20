using Microsoft.OpenApi.Models;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureApplicationServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddHttpClient<ICatalogService, CatalogService>(c =>
      c.BaseAddress = new Uri(configuration["ApiSettings:CatalogUrl"]));

    services.AddHttpClient<IBasketService, BasketService>(c =>
        c.BaseAddress = new Uri(configuration["ApiSettings:BasketUrl"]));

    services.AddHttpClient<IOrderingService, OrderingService>(c =>
        c.BaseAddress = new Uri(configuration["ApiSettings:OrderingUrl"]));
    services.AddControllers();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping.Aggregator", Version = "v1" });
    });

};