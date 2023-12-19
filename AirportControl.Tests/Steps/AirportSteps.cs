using AirportControl.Application.Dto;
using FluentAssertions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;

namespace AirportControl.Tests.Steps;

[Binding]
public class AirportSteps
{
    private const string BaseAddress = "http://localhost/3002";
    private readonly HttpClient _client;
    private readonly ScenarioContext _scenarioContext;
    
    public AirportSteps(
        WebApplicationFactory<Program> factory,
        ScenarioContext scenarioContext)
    {
        _client = factory.CreateDefaultClient(new Uri(BaseAddress));
        
        _scenarioContext = scenarioContext;
    }


    [When(@"I send the request")]
    public async Task WhenISendTheRequest()
    {
        var url = _scenarioContext.Get<string>("Url");

        var response = await _client.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();

        _scenarioContext.Add("RequestContent", content);
    }

    [Given(@"I have set a valid value for the ID")]
    public void GivenIHaveSetAValidValueForTheId()
    {
        var id = _scenarioContext.Get<int>("Id");

        id.Should().BeInRange(1, 3);
    }

    [Given(@"I am sending a GET request to the /airports endpoint")]
    public void GivenIAmSendingAgetRequestToTheAirportsEndpoint()
    {
        _scenarioContext.Add("Url", "airports");
    }

    [Given(@"I am sending a GET request to the /airports/\{ID} endpoint")]
    public void GivenIAmSendingAgetRequestToTheAirportsIdEndpoint()
    {
        int id = 2;

        _scenarioContext.Add("Url", "airports/" + id);
        _scenarioContext.Add("Id", id);
    }

    [Then(@"a list of airports with IDs and IATACodes is returned")]
    public void ThenAListOfAirportsWithIDsAndIataCodesIsReturned()
    {
        var content = _scenarioContext.Get<string>("RequestContent");

        var airports = JsonSerializer.Deserialize<IEnumerable<AirportWithIdIATACodeDto>>(content);

        airports.Should().NotBeNull();
    }

    [Then(@"the full information for the specified airport is returned")]
    public void ThenTheFullInformationForTheSpecifiedAirportIsReturned()
    {
        var content = _scenarioContext.Get<string>("RequestContent");

        var airport = JsonSerializer.Deserialize<Airport>(content);

        airport.Should().NotBeNull();
    }


    [Then(@"All details of the airport should be returned")]
    public void ThenAllDetailsOfTheAirportShouldBeReturned()
    {
        var content = _scenarioContext.Get<string>("RequestContent");

        var result = JsonSerializer.Deserialize<Airport>(content);
        
        result.Should().NotBeNull();
    }

    [Then(@"An error message should be returned stating that the airport could not be found")]
    public void ThenAnErrorMessageShouldBeReturnedStatingThatTheAirportCouldNotBeFound()
    {
        var content = _scenarioContext.Get<string>("RequestContent");

        content.Should().BeEmpty();
    }

    [Given(@"I am sending a GET request to the /airports/(.*) endpoint")]
    public void GivenIAmSendingAgetRequestToTheAirportsEndpoint(int p0)
    {
        int id = 15;

        _scenarioContext.Add("Url", "airports/" + id);
        _scenarioContext.Add("Id", id);
    }
}