using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName }.json", true, true);
builder.Services.AddOcelot().AddCacheManager(
    s=>s.WithDictionaryHandle());

ConfigureApplicationServices(builder.Services, builder.Configuration);

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

await app.UseOcelot();


app.Run();

void ConfigureApplicationServices(IServiceCollection services, ConfigurationManager configuration)
{


}