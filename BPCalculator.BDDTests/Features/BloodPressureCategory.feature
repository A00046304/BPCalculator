Feature: Blood Pressure Category
  In order to understand my blood pressure
  As a patient
  I want to see the category for my systolic and diastolic values

  Scenario Outline: Classifying valid blood pressure readings
    Given I enter a systolic value of <systolic>
    And I enter a diastolic value of <diastolic>
    When I calculate the blood pressure category
    Then the result should be "<expectedCategory>"

    Examples:
      | systolic | diastolic | expectedCategory |
      | 150      | 95        | High             |
      | 130      | 85        | PreHigh          |
      | 110      | 70        | Ideal            |
      | 80       | 55        | Low              |

  Scenario Outline: Handling invalid readings
    Given I enter a systolic value of <systolic>
    And I enter a diastolic value of <diastolic>
    When I try to validate the reading
    Then an error should be shown

    Examples:
      | systolic | diastolic |
      | 60       | 50        |   # systolic out of range
      | 200      | 90        |   # systolic out of range
      | 120      | 30        |   # diastolic out of range
      | 100      | 100       |   # systolic <= diastolic
