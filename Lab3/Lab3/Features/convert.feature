Feature: Convert

A short summary of the feature

@tag1
Scenario: Convert currency
	Given connect to https://neutrinoapi.net
    And create POST request to convert
    And add header User-ID with value lab3
    And add header API-Key with value s4aqYkhKYE5C0M735mEmyMK7YW9Fzz0Mr9T98diXKzff26VF
    And add parameter from-value with value 100
    And add parameter from-type with value USD
    And add parameter to-type with value EUR
    When send request
    Then response is Ok
    And response contains json with value of new currency