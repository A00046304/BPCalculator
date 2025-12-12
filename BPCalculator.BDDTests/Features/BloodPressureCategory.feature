Feature: Blood Pressure Evaluation
    Validates readings, determines category, and returns medication message.

Scenario Outline: Category calculation works correctly
    Given I enter a systolic value of <sys>
    And I enter a diastolic value of <dia>
    When I calculate the blood pressure category
    Then the result should be "<expected>"

Examples:
    | sys | dia | expected |
    | 150 | 95  | High     |
    | 130 | 70  | PreHigh  |
    | 100 | 70  | Ideal    |
    | 80  | 55  | Low      |

Scenario Outline: Validation errors are triggered
    Given I enter a systolic value of <sys>
    And I enter a diastolic value of <dia>
    When I try to validate the reading
    Then an error should be shown

Examples:
    | sys | dia |
    | 69  | 60  |
    | 191 | 50  |
    | 120 | 39  |
    | 120 | 101 |
    | 120 | 120 |
    | 100 | 130 |

Scenario Outline: Medication advice is accurate
    Given I enter a systolic value of <sys>
    And I enter a diastolic value of <dia>
    When I request medication advice
    Then the medication message should be "<expected>"

Examples:
    | sys | dia | expected                                                                              |
    | 150 | 95  | Consider discussing blood pressure medication with a healthcare professional.         |
    | 130 | 70  | Continue regular monitoring, medication may be required soon.                         |
    | 100 | 70  | No medication needed.                                                                 |
    | 80  | 55  | Increase fluid or salt intake if advised by your doctor.                              |
