using AirportControl.Application.Dto;
using AirportControl.Application.Model;

namespace AirportControl.Application.Services;

public interface IDatabaseService
{
    Task<IEnumerable<Airport>> GetAllAirportAsync(CancellationToken cancellationToken = default);

    Task<Airport?> GetAirportByIdAsync(int id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<GeographyLevel1>> GetAllCountries(CancellationToken cancellationToken = default);

    Task<GeographyLevel1?> CreateCountryAsync(string name, CancellationToken cancellationToken = default);
    
    Task<GeographyLevel1?> DeleteCountryAsync(int id, CancellationToken cancellationToken = default);

    Task<AirportRoute?> CreateRouteAsync(int startAirport, int endAirport,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<AirportRoute>> GetAllRoute(CancellationToken cancellationToken = default);
}