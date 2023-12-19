using AirportControl.Application.Dto;
using AirportControl.Application.Model;
using AirportControl.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AirportControl.Application.Apis;

public static class CountriesApi
{
    
    public static IEndpointRouteBuilder MapCountriesApi(this IEndpointRouteBuilder app)
    {
        // Routes for querying catalog items.
        app.MapGet("/countries", GetAllItems);
        app.MapPost("/countries", CreateCountry);
        app.MapDelete("/countries", DeleteCountry);
    
        return app;
    }

    public static async Task<Results<Ok<GeographyLevel1>, NotFound>> DeleteCountry(IDatabaseService service, [FromQuery] int id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteCountryAsync(id, cancellationToken);

        if (result is null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(result);
    }

    public static async Task<Ok<IEnumerable<GeographyLevel1>>> GetAllItems(IDatabaseService service, CancellationToken cancellationToken)
    {
        var result = await service.GetAllCountries(cancellationToken);
        
        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<GeographyLevel1>, BadRequest>> CreateCountry(IDatabaseService service, [FromBody] CreateCountryRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateCountryAsync(request.Name, cancellationToken);

        if (result is null)
            return TypedResults.BadRequest();
            
        return TypedResults.Ok(result);
    }
}