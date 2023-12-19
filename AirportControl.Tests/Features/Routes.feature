Feature: Routes
    
Scenario: Viewing All Routes
    Given I am sending a GET request to the /routes endpoint
    When I send the request for route
    Then a list of routes with IDs and airport IDs is returned
    
Scenario: Adding a New Route
    Given I am sending a POST request to create a new route to the /routes endpoint
    And the route does not already exist
    And the airports IDs exist
    And the airports are valid choices for departure and arrival
    When I send the request for route
    Then the route is successfully created