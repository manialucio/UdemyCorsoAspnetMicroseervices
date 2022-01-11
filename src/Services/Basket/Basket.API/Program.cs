using Basket.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureApplicationServices(builder.Services,builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureApplicationServices(IServiceCollection services,IConfiguration configuration)
{
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString"); // configuration == connection string
    });
    services.AddScoped<IBasketRepository, BasketRepository>();
 };