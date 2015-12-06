Feature: Generating Direct Debit Remmitances
	In order to charge the bills to my club members
	As an administrative assistant
	I want to generate direct debit remitances to bank

Background: 
	
	Given My Direct Debit Initiation Contract is
	| NIF       | Name                              | BIC         | CreditorAgentName | LocalBankCode | CreditorBussinesCode | CreditorAccount          |
	| G35008770 | Real Club Náutico de Gran Canaria | CAIXESBBXXX | CAIXABANK         | 2100          | 777                  | ES5621001111301111111111 |

	Given These Club Members
	| MemberID | Name      | FirstSurname  | SecondSurname | Reference | Account              | BIC         |
	| 00001    | Francisco | Gomez-Caldito | Viseas        | 1234      | 01821111601111111111 | BBVAESMMXXX |
	| 00002    | Pedro     | Perez         | Gomez         | 1235      | 21001111301111111111 | CAIXESBBXXX |

	Given These bills
	| MemberID | TransactionConcept           | Amount |
	| 00001    | Cuota Mensual Octubre 2013   | 79     |
	| 00002    | Cuota Mensual Octubre 2013   | 79     |
	| 00002    | Cuota Mensual Noviembre 2013 | 79     |

Scenario: Create a new direct debit remmitance
	Given I have a I have a direct debit initiation contract
	When I generate a new direct debit remmitance
	Then An empty direct debit remmitance is created

Scenario: Create an empty group of direct debit payments
	Given I have a will send the payments using "COR1" local instrument
	When I generate an empty group of direct debit payments
	Then An empty group of direct debit payments using "COR1" is generated

Scenario: Create a Direct Debit Transaction from a bill as specified by a member direct debit mandate
	Given I have a member
	And The member has a bill
	And The member has a Direct Debit Mandate
	When I generate Direct Debit Transaction
	Then The direct debit transaction is correctly created

Scenario: Add a second bill to a direct debit transaction
	Given I have a direct debit with 1 bill and amount of 79
	When I add a new bill with amount of 79
	Then The direct debit transaction is updated with 2 bills and amount of 158

Scenario: Add a direct debit transaction to an empty group of payments
	Given I have a direct debit with 1 bill and amount of 79
	And I have an empty group of payments
	When I add the direct debit transaction to the group of payments
	Then The group of payments is updated with 1 direct debit and total amount of 79

Scenario: Add a second direct debit transaction to a group of payments
	Given I have a group of payments with 1 direct debit transaction and amount of 79
	When I add a new direct debit transaction with amount of 79
	Then The group of payments is updated with 2 direct debit and total amount of 158

Scenario: Add a group payments to a direct debit remmitance
	Given I have an empty direct debit remmitance
	And I have a group of payments with 1 direct debit transaction and amount of 79
	When I add the group to the direct debit remittance
	Then The direct debit remittance is updated with 1 direct debit and total amount of 79

Scenario: Generating SEPA ISO20022 XML CustomerDirectDebitInitiation Message form a Direct Debit Remmitance
	Given I have a prepared Direct Debit Remmitance
	When I generate de SEPA ISO200022 XML CustomerDirectDebitInitiation message
	Then The message is correctly created