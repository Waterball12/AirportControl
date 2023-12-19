Feature: Countries
    
Scenario: Viewing All Countries
    Given I am sending a GET request to the /countries endpoint
    When I send the request with method
    Then a list of countries with IDs and names is returned
    
Scenario: Adding a Country
    Given I am sending a POST request to add a country to the /countries endpoint
    And the country name does not already exist
    When I send the request with method
    Then the country is created
    And a successful response status code is returned
    
Scenario: Deleting a Country
    Given I am sending a DELETE request to delete a country to the /countries endpoint
    And the country ID exists
    And the country is not in use for an airport
    When I send the request with method
    Then the country is deleted
    And a successful response status code is returned
    

Scenario: Adding Portugal
    Given I am sending a POST request to add a country to the /countries endpoint
    And the country name is portugal
    When I send the request with method
    Then the country is created
    And a successful response status code is returned
    
Scenario: Adding Spain
    Given I am sending a POST request to add a country to the /countries endpoint
    And the country name is spain
    When I send the request with method
    Then response is not success
    
Scenario: Delete the country with ID 1
    Given I am sending a POST request to add a country to the /countries endpoint
    And the id is 1
    When I send the request with method
    Then response is not success