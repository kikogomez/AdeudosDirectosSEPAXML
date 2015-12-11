Feature: Manage Debtors Billing Information
	In order be able to bill my debtors
	As a administrative assistant
	I want to manage my debtors billing registered data

Background:

	Given A debtor
	| DebtorID | Name      | FirstSurname  | SecondSurname |
	| 00001    | Francisco | Gomez-Caldito | Viseas        |

	Given These Direct Debit Mandates
	| DirectDebitInternalReferenceNumber | RegisterDate | IBAN                     |
	| 2345                               | 2013/11/20   | ES6812345678061234567890 |
	| 2346                               | 2013/11/30   | ES3011112222003333333333 |

	Given These Account Numbers
	| IBAN                     |
	| ES6812345678061234567890 |
	| ES3011112222003333333333 |

Scenario: I can change the member default payment method
	Given I have a debtor
	And The debtor has associated cash as payment method
	When I set direct debit as new payment method
	Then The new payment method is correctly updated

Scenario: I can assign a new direct debit mandate to a debtor
	Given I have a debtor
	When I add a new direct debit mandate to the debtor
	Then The new direct debit mandate is correctly assigned

Scenario: I can change the account number associated to a direct debit
	Given I have a debtor
	And I have a direct debit associated to the debtor
	When I change the account number of the direct debit
	Then The account number is correctly changed
	And The old account number is stored in the account numbers history
