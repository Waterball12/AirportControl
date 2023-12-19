using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AirportControl.Application.Dto;
using AirportControl.Application.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;

namespace AirportControl.Tests.Steps;

[Binding]
public class CountriesSteps
{
    private const string BaseAddress = "http://localhost/3002";
    private readonly HttpClient _client;
    private readonly ScenarioContext _scenarioContext;
    
    public CountriesSteps(
        WebApplicationFactory<Program> factory,
        ScenarioContext scenarioContext)
    {
        _client = factory.CreateDefaultClient(new Uri(BaseAddress));
        
        _scenarioContext = scenarioContext;
    }
    
    [Given(@"I am sending a GET request to the /countries endpoint")]
    public void GivenIAmSendingAgetRequestToTheCountriesEndpoint()
    {
        _scenarioContext.Add("Url", "/countries");
        _scenarioContext.Add("Method", "GET");
    }

    [Then(@"a list of countries with IDs and names is returned")]
    public async Task ThenAListOfCountriesWithIDsAndNamesIsReturned()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("ResponseMessage");

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<GeographyLevel1>>();

        content.Should().NotBeNull();
    }

    [Given(@"I am sending a POST request to add a country to the /countries endpoint")]
    public void GivenIAmSendingApostRequestToAddACountryToTheCountriesEndpoint()
    {
        _scenarioContext.Add("Url", "/countries");
        _scenarioContext.Add("Method", "POST");
    }

    [Given(@"the country name does not already exist")]
    public async Task GivenTheCountryNameDoesNotAlreadyExist()
    {
        _scenarioContext.Add("CountryName", Guid.NewGuid().ToString());
    }

    [Then(@"the country is created")]
    public async Task ThenTheCountryIsCreated()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("ResponseMessage");
        var content = await response.Content.ReadFromJsonAsync<GeographyLevel1>();
        content.Should().NotBeNull();
        content!.Name.Should().NotBeNull();
    }

    [Then(@"a successful response status code is returned")]
    public void ThenASuccessfulResponseStatusCodeIsReturned()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("ResponseMessage");

        response.EnsureSuccessStatusCode();
    }

    [Given(@"I am sending a DELETE request to delete a country to the /countries endpoint")]
    public void GivenIAmSendingAdeleteRequestToDeleteACountryToTheCountriesEndpoint()
    {
        _scenarioContext.Add("Url", "/countries");
        _scenarioContext.Add("Method", "DELETE");
    }

    [Given(@"the country ID exists")]
    public async Task GivenTheCountryIdExists()
    {
        var all = await _client.GetFromJsonAsync<IEnumerable<GeographyLevel1>>("countries");
        _scenarioContext.Add("CountryId", all.FirstOrDefault().GeographyLevel1ID);
    }

    [Given(@"the country is not in use for an airport")]
    public async Task GivenTheCountryIsNotInUseForAnAirport()
    {
        var all = await _client.GetFromJsonAsync<IEnumerable<GeographyLevel1>>("countries");
        _scenarioContext.Set(all.LastOrDefault().GeographyLevel1ID, "CountryId");
    }

    [Then(@"the country is deleted")]
    public async Task ThenTheCountryIsDeleted()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("ResponseMessage");
        var content = await response.Content.ReadFromJsonAsync<GeographyLevel1>();
        content.Should().NotBeNull();
        content!.Name.Should().NotBeNull();
    }

    [When(@"I send the request with method")]
    public async Task WhenISendTheRequestWithMethod()
    {
        var url = _scenarioContext.Get<string>("Url");
        var method = _scenarioContext.Get<string>("Method");
        _scenarioContext.TryGetValue<string>("CountryName", out string countryName);
        _scenarioContext.TryGetValue<int>("CountryId", out int countryId);

        var response = method.ToUpperInvariant() switch
        {
            "GET" => await _client.GetAsync(url),
            "POST" => await _client.PostAsJsonAsync(url, new CreateCountryRequest()
            {
                Name = countryName
            }),
            "DELETE" => await _client.DeleteAsync(url + "?id=" + countryId),
            _ => null
        };


        _scenarioContext.Add("ResponseMessage", response);
    }

    [Given(@"the country name is portugal")]
    public void GivenTheCountryNameIsPortugal()
    {
        _scenarioContext.Add("CountryName", "Portugal");
    }

    [Given(@"the country name is spain")]
    public void GivenTheCountryNameIsSpain()
    {
        _scenarioContext.Add("CountryName", "Spain");
    }

    [Then(@"response is not success")]
    public void ThenResponseIsNotSuccess()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("ResponseMessage");

        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }

    [Given(@"the id is (.*)")]
    public void GivenTheIdIs(int p0)
    {
        _scenarioContext.Add("CountryId", p0);
    }
}