using AirportControl.Application.Model;

namespace AirportControl.Application.Infrastructure.DbContexts;

public static class AirportControlContextSeed
{
    public static async Task SeedAsync(AirportControlContext context)
    {
        await context.Database.EnsureCreatedAsync();

        var airports = new List<Airport>()
        {
            new Airport()
            {
                AirportID = 1,
                IATACpde = "LGW",
                GeographyLevel1ID = 1,
                Type = "Departure and Arrival"
            },
            new Airport()
            {
                AirportID = 2,
                IATACpde = "PMI",
                GeographyLevel1ID = 2,
                Type = "Arrival Only"
            },
            new Airport()
            {
                AirportID = 3,
                IATACpde = "LAX",
                GeographyLevel1ID = 3,
                Type = "Arrival Only"
            }
        };

        var geopgrahy = new List<GeographyLevel1>()
        {
            new() { GeographyLevel1ID = 1, Name = "United Kingdom" },
            new() { GeographyLevel1ID = 2, Name = "Spain" },
            new() { GeographyLevel1ID = 3, Name = "United States" },
            new() { GeographyLevel1ID = 4, Name = "Turkey" }
        };

        var route = new Model.AirportRoute()
        {
            RouteId = 1,
            DepartureAirportID = 1,
            ArrivalAirportID = 2
        };

        context.Airport.AddRange(airports);
        context.GeographyLevel1s.AddRange(geopgrahy);
        context.Route.Add(route);
        await context.SaveChangesAsync();
    }
}