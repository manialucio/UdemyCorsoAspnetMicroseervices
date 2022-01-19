using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using System.Reflection;

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
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddScoped<IConsumer<BasketCheckoutEvent>, BasketCheckoutConsumer>();
    services.AddMediatR(Assembly.GetExecutingAssembly());
    services.AddMassTransit(
        config =>
        {
            config.AddConsumer<BasketCheckoutConsumer>();
            config.UsingRabbitMq((contextMQ, configMQ) =>
            {
                configMQ.Host(configuration["EventBusSettings:HostAddress"]);
                configMQ.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                {
                    c.ConfigureConsumer<BasketCheckoutConsumer>(contextMQ);
                });
            });
        });
    services.AddMassTransitHostedService();
};

void MigrationDatabase(WebApplication app)
{
    app.MigrateDatabase<OrderContext>(
    (context, services) =>
    {
        var logger = services.GetService<ILogger<OrderContextSeed>>() ?? throw new Exception("Non trovo il logger in migration database");
        OrderContextSeed
            .SeedAsync(context, logger)
            .Wait();
    });
}