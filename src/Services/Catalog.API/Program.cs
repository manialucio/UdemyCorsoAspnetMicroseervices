using Catalog.API.Data;
using Catalog.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(action =>
{
   action.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Catalog.API", Version = "v1" });
});

ConfigureApplicationServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI( action => action.SwaggerEndpoint("/swagger/v1/swagger.json","Catalog.API v1"));
}
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureApplicationServices(IServiceCollection services)
{
    services.AddScoped<ICatalogContext, CatalogContext>();
    services.AddScoped<IProductRepository, ProductRepository>();
};