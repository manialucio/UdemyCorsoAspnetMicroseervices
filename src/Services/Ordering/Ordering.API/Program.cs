using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
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

app.UseAuthorization();

app.MapControllers();

MigrationDatabase(app);

app.Run();
void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddApplicationServices();
    services.AddInfrastructureServices(configuration);

};

void MigrationDatabase(WebApplication app)
{
    app.MigrateDatabase<OrderContext>(
    (context, services) =>
    {
        var logger = services.GetService<ILogger<OrderContextSeed>>()?? throw new Exception ("Non trovo il logger in migration database");
        OrderContextSeed
            .SeedAsync(context, logger)
            .Wait();
    });
}