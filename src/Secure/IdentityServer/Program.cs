using IdentityServer;
using IdentityServer4.Models;
using IdentityServer4.Test;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName }.json", true, true);
//builder.Services.AddOcelot().AddCacheManager(
//    s => s.WithDictionaryHandle());

ConfigureApplicationServices(builder.Services, builder.Configuration);

var app = builder.Build();
app.UseRouting();
app.UseIdentityServer();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

//await app.UseOcelot();


app.Run();

void ConfigureApplicationServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddIdentityServer()
     .AddInMemoryClients(Config.Clients)
     .AddInMemoryIdentityResources(Config.IdentityResources)
     .AddInMemoryApiResources(Config.ApiResources)
     .AddInMemoryApiScopes(Config.ApiScopes)
     .AddTestUsers(Config.TestUsers)
     .AddDeveloperSigningCredential();
    


}