using AirportControl.Application.Infrastructure.DbContexts;
using AirportControl.Application.Model;
using Microsoft.EntityFrameworkCore;

namespace AirportControl.Application.Services;

public class DatabaseService : IDatabaseService
{
    private readonly AirportControlContext _context;

    public DatabaseService(AirportControlContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Airport>> GetAllAirportAsync(CancellationToken cancellationToken = default)
    {
        return await _context
            .Airport
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Airport?> GetAirportByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Airport.FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<GeographyLevel1>> GetAllCountries(CancellationToken cancellationToken = default)
    {
        return await _context
            .GeographyLevel1s
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<GeographyLevel1?> CreateCountryAsync(string name, CancellationToken cancellationToken = default)
    {
        var exist = await _context.GeographyLevel1s.AnyAsync(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase), cancellationToken: cancellationToken);

        if (exist)
            return null;
        
        var result = await _context.GeographyLevel1s.AddAsync(new()
        {
            Name = name
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task<GeographyLevel1?> DeleteCountryAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.GeographyLevel1s.FindAsync(id, cancellationToken);

        if (entity is null)
            return null;

        var isInUse = await _context.Route.AnyAsync(x =>
            x.ArrivalAirportID == entity.GeographyLevel1ID || x.DepartureAirportID == entity.GeographyLevel1ID, cancellationToken: cancellationToken);

        if (isInUse)
            return null;
        
        _context.GeographyLevel1s.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<AirportRoute>> GetAllRoute(CancellationToken cancellationToken = default)
    {
        return await _context.Route
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<AirportRoute?> CreateRouteAsync(int startAirportId, int endAirportId,
        CancellationToken cancellationToken = default)
    {
        var routeExist =
            await _context.Route.AnyAsync(x => x.DepartureAirportID == startAirportId && x.ArrivalAirportID == endAirportId, cancellationToken);

        if (routeExist)
            return null;

        var airports = await _context.Airport.Where(x => x.AirportID == startAirportId || x.AirportID == endAirportId).ToListAsync(cancellationToken: cancellationToken);

        var startAirport = airports.FirstOrDefault(x => x.AirportID == startAirportId);
        var endAirport = airports.FirstOrDefault(x => x.AirportID == endAirportId);

        if (startAirport is null || !startAirport.Type.Contains("Departure"))
            return null;

        if (endAirport is null || !endAirport.Type.Contains("Arrival"))
            return null;
        
        var result = await _context.Route.AddAsync(new AirportRoute()
        {
            ArrivalAirportID = endAirportId,
            DepartureAirportID = startAirportId
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        
        return result.Entity;
    }
}