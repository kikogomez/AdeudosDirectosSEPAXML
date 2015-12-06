Feature: Add members
	In order to manage the club members
	As a administrative assistant
	I want to add new members to the system

Scenario Outline: Second surname is optional but given name and first surname are mandatory
	Given These names <GivenName>, <FirstSurname>, <SecondSurname> 
	When I process the names
	Then The name is considered <valid>

Scenarios:
| GivenName   | FirstSurname    | SecondSurname | valid     |
| "Francisco" | "Gomez-Caldito" | "Viseas"      | "valid"   |
| "Francisco" | "Gomez-Caldito" | ""            | "valid"   |
| "Francisco" | ""              | "Viseas"      | "invalid" |
| ""          | "Gomez-Caldito" | "Viseas"      | "invalid" |
| "Francisco" | ""              | ""            | "invalid" |
| ""          | "Gomez-Caldito" | ""            | "invalid" |
| ""          | ""              | "Viseas"      | "invalid" |
| ""          | ""              | ""            | "invalid" |

Scenario: The members ID are consecutive
	Given The current memberID sequence number is 2
	When I add a new member
	Then The current memberID sequence number is 3

Scenario: Up to 99999 members
	Given The current memberID sequence number is 100000
	When I add a new member
	Then The new member is not created

