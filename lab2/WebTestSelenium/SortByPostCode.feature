Feature: Sort Customers by Post Code
  As a bank manager
  I want to be able to sort the list of customers by Post Code
  So that I can view the list in descending order

  Background:
    Given I am logged in as a Bank Manager

  Scenario: Sort customers by Post Code in descending order
    When I navigate to the Customers page
    And I sort customers by Post Code
    Then the customer list should be sorted by Post Code
