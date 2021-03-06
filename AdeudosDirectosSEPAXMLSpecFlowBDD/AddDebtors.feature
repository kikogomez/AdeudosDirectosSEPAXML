﻿Feature: Add debtors
	In order to manage the debtors
	As a administrative assistant
	I want to add new debtors to the system

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


