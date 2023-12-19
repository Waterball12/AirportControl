Feature: Airpots
    
Scenario: Viewing all Airports
    Given I am sending a GET request to the /airports endpoint
    When I send the request
    Then a list of airports with IDs and IATACodes is returned
    
Scenario: Viewing a Single Airport
    Given I am sending a GET request to the /airports/{ID} endpoint
    And I have set a valid value for the ID
    When I send the request
    Then the full information for the specified airport is returned
    

Scenario: Get the airport
    Given I am sending a GET request to the /airports/{ID} endpoint
    And I have set a valid value for the ID
    When I send the request
    Then All details of the airport should be returned

Scenario: Get specific airport
    Given I am sending a GET request to the /airports/15 endpoint
    When I send the request
    Then An error message should be returned stating that the airport could not be found