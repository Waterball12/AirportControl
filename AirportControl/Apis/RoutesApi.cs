using AirportControl.Application.Dto;
using AirportControl.Application.Model;
using AirportControl.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AirportControl.Application.Apis;

public static class RoutesApi
{
    public static IEndpointRouteBuilder MapRoutesApi(this IEndpointRouteBuilder app)
    {
        // Routes for querying catalog items.
        app.MapGet("/routes", GetAllItems);
        app.MapPost("/routes", CreateRoute);
    
        return app;
    }

    public static async Task<Results<Ok<AirportRoute>, BadRequest>> CreateRoute(IDatabaseService service, [FromBody] CreateRouteRequest request, CancellationToken cancellationToken)
    {
        var result =
            await service.CreateRouteAsync(request.DepartureAirportID, request.ArrivalAirportID, cancellationToken);

        if (result is null)
            return TypedResults.BadRequest();

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<IEnumerable<AirportRoute>>> GetAllItems(IDatabaseService service, CancellationToken cancellationToken)
    {
        var routes = await service.GetAllRoute(cancellationToken);

        return TypedResults.Ok(routes);
    }
}