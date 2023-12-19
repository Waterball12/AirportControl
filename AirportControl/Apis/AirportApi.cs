using AirportControl.Application.Dto;
using AirportControl.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AirportControl.Application.Apis;

public static class AirportApi
{
    public static IEndpointRouteBuilder MapAirportApi(this IEndpointRouteBuilder app)
    {
        // Routes for querying catalog items.
        app.MapGet("/airports", GetAllItems);
        app.MapGet("/airports/{id:int}", GetItemsByIds);

        return app;
    }

    public static async Task<Results<Ok<Airport>, NotFound>> GetItemsByIds(IDatabaseService service, int id, CancellationToken cancellationToken)
    {
        var result = await service.GetAirportByIdAsync(id, cancellationToken);
        
        if (result is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<IEnumerable<AirportWithIdIATACodeDto>>> GetAllItems(IDatabaseService service, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAirportAsync(cancellationToken);

        return TypedResults.Ok(result.Select(x => new AirportWithIdIATACodeDto()
        {
            AirportID = x.AirportID,
            IATACpde = x.IATACpde
        }));
    }
}