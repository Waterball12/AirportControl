using System.Net.Http.Json;
using AirportControl.Application.Dto;
using AirportControl.Application.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;

namespace AirportControl.Tests.Steps;

[Binding]
public class RoutesSteps
{
    private const string BaseAddress = "http://localhost/3002";
    private readonly HttpClient _client;
    private readonly ScenarioContext _scenarioContext;
    
    public RoutesSteps(
        WebApplicationFactory<Program> factory,
        ScenarioContext scenarioContext)
    {
        _client = factory.CreateDefaultClient(new Uri(BaseAddress));
        
        _scenarioContext = scenarioContext;
    }
    
    [Given(@"I am sending a GET request to the /routes endpoint")]
    public void GivenIAmSendingAgetRequestToTheRoutesEndpoint()
    {
        _scenarioContext.Add("Url", "/routes");
        _scenarioContext.Add("Method", "GET");
    }

    [Then(@"a list of routes with IDs and airport IDs is returned")]
    public async Task ThenAListOfRoutesWithIDsAndAirportIDsIsReturned()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("ResponseMessage");

        var routes = await response.Content.ReadFromJsonAsync<IEnumerable<AirportRoute>>();

        routes.Should().NotBeNull();
    }

    [Given(@"I am sending a POST request to create a new route to the /routes endpoint")]
    public void GivenIAmSendingApostRequestToCreateANewRouteToTheRoutesEndpoint()
    {
        _scenarioContext.Add("Url", "/countries");
        _scenarioContext.Add("Method", "POST");
    }

    [Given(@"the route does not already exist")]
    public async Task GivenTheRouteDoesNotAlreadyExist()
    {
        int startRoute = 1;
        int endRoute = 3;
        
        var routes = await _client.GetFromJsonAsync<IEnumerable<AirportRoute>>("routes");

        if (routes.Any(x => x.DepartureAirportID == startRoute && x.ArrivalAirportID == endRoute))
            Assert.Fail("Route exist");
        
        _scenarioContext.Add("StartRoute", startRoute);
        _scenarioContext.Add("EndRoute", endRoute);
    }

    [Given(@"the airports IDs exist")]
    public async Task GivenTheAirportsIDsExist()
    {
        _scenarioContext.TryGetValue("StartRoute", out int startRoute);
        _scenarioContext.TryGetValue("EndRoute", out int endRoute);

        var startAirport = await _client.GetFromJsonAsync<Airport>("airports/" + startRoute);
        var endAirport = await _client.GetFromJsonAsync<Airport>("airports/" + endRoute);
        
        if (startAirport is null || endAirport is null)
        {
            Assert.Fail("Airport does not exist");
        }
        
        _scenarioContext.Add("StartAirport", startAirport);
        _scenarioContext.Add("EndAirport", endAirport);
    }

    [Given(@"the airports are valid choices for departure and arrival")]
    public void GivenTheAirportsAreValidChoicesForDepartureAndArrival()
    {
        _scenarioContext.TryGetValue("StartAirport", out Airport startRoute);
        _scenarioContext.TryGetValue("EndAirport", out Airport endRoute);

        if (!startRoute.Type.Contains("Departure"))
        {
            Assert.Fail("Start airport is not a departure airport");
        }
        
        if (!endRoute.Type.Contains("Arrival"))
        {
            Assert.Fail("End airport is not a arrival airport");
        }
    }

    [Then(@"the route is successfully created")]
    public async Task ThenTheRouteIsSuccessfullyCreated()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("ResponseMessage");
    }

    [When(@"I send the request for route")]
    public async Task WhenISendTheRequestForRoute()
    {
        var url = _scenarioContext.Get<string>("Url");
        var method = _scenarioContext.Get<string>("Method");;
        _scenarioContext.TryGetValue("StartAirport", out Airport startRoute);
        _scenarioContext.TryGetValue("EndAirport", out Airport endRoute);

        var response = method.ToUpperInvariant() switch
        {
            "GET" => await _client.GetAsync(url),
            "POST" => await _client.PostAsJsonAsync(url, new CreateRouteRequest()
            {
                DepartureAirportID = startRoute.AirportID,
                ArrivalAirportID = endRoute.AirportID
            }),
            _ => null
        };


        _scenarioContext.Add("ResponseMessage", response);
    }
}