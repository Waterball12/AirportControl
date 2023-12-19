using AirportControl.Application.Apis;
using AirportControl.Application.Infrastructure.DbContexts;
using AirportControl.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var dbName = Guid.NewGuid().ToString();

builder.Services.AddDbContext<AirportControlContext>(q =>
{
    q.UseInMemoryDatabase(dbName);
});
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

using (var q = app.Services.CreateScope())
{
    var context = q.ServiceProvider.GetRequiredService<AirportControlContext>();

    await AirportControlContextSeed.SeedAsync(context);
};

app.MapAirportApi();
app.MapCountriesApi();
app.MapRoutesApi();

app.Run();



public partial class Program
{
}