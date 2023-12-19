using AirportControl.Application.Model;
using Microsoft.EntityFrameworkCore;

namespace AirportControl.Application.Infrastructure.DbContexts;

public class AirportControlContext : DbContext
{
    public AirportControlContext(DbContextOptions<AirportControlContext> options) : base(options)
    {
    }

    public DbSet<Airport> Airport { get; set; }

    public DbSet<GeographyLevel1> GeographyLevel1s { get; set; }

    public DbSet<Model.AirportRoute> Route { get; set; }


}