Feature: Manage bills
	In order charge my invoices
	As an administrative assistant
	I want reate and manage bills for my invoices

Background: 
	Given Last generated InvoiceID is "INV2013000023"
	
	Given A Club Member with a default Payment method
	| MemberID | Name      | FirstSurname  | SecondSurname | Default Payment method | Spanish IBAN Bank Account          | Direct Debit Reference Number |
	| 00001    | Francisco | Gomez-Caldito | Viseas        | Direct Debit           | IBAN ES68 1234 5678 0612 3456 7890 | 12345                         |

	Given This set of taxes
	| Tax Type            | Tax Value |
	| No IGIC             | 0         |
	| IGIC Reducido 1     | 2.75      |
	| IGIC Reducido 2     | 3.00      |
	| IGIC General        | 7.00      |
	| IGIC Incrementado 1 | 9.50      |
	| IGIC Incrementado 2 | 13.50     |
	| IGIC Especial       | 20.00     |

	Given These services
	| Service Name                | Default Cost | Default Tax  |
	| Rent a kajak                | 50.00        | IGIC General |
	| Rent a katamaran            | 100.55       | IGIC General |
	| Rent a mouring              | 150.00       | IGIC General |
	| Full Membership Monthly Fee | 79.00        | No IGIC      |

	Given These products
	| Product Name   | Default Cost | Default Tax  |
	| Pennant        | 10.00        | IGIC General |
	| Cup            | 15.00        | IGIC General |
	| Member ID Card | 1.50         | No IGIC      |

Scenario: A single bill is automatically created for a new invoice
	Given The member uses the club service "Rent a kajak"
	When I generate an invoice for the service
	Then An invoice is created for the cost of the service: 53.50
	And A single bill To Collect is generated for the total amount of the invoice: 53.50
	And The bill ID is "INV2013000023/001"
	And By default no payment method is associated to bill

Scenario: No bills are created for a pro forma invoice
	Given The member uses the club service "Rent a kajak"
	When I generate an pro-forma invoice for the service
	Then A pro-forma invoice is created for the cost of the service: 53.50
	And No bills are created for a pro-forma invoice

Scenario: A bill can be renegotiated into instalments
	Given I have an invoice with cost 650 with a single bill with ID "INV2013000023/001"
	When I renegotiate the bill "INV2013000023/001" into three instalments: 200, 200, 250 to pay in 30, 60 and 90 days with agreement terms "Payment Agtreement"
	Then The bill "INV2013000023/001" is marked as renegotiated
	And The renegotiated bill "INV2013000023/001" has associated the agreement terms "Payment Agtreement" to it
	And A bill with ID "INV2013000023/002" and cost of 200 to be paid in 30 days is created
	And The new bill "INV2013000023/002" has associated the agreement terms "Payment Agtreement" to it
	And A bill with ID "INV2013000023/003" and cost of 200 to be paid in 60 days is created
	And The new bill "INV2013000023/003" has associated the agreement terms "Payment Agtreement" to it
	And A bill with ID "INV2013000023/004" and cost of 250 to be paid in 90 days is created
	And The new bill "INV2013000023/004" has associated the agreement terms "Payment Agtreement" to it

Scenario: I can assign an specific expected payment method for a single bill
	Given I have an invoice with cost 650 with a single bill with ID "INV2013000023/001"
	When I assign to be paid with a direct debit
	Then The new payment method is correctly assigned

Scenario: A bill to collect is paid in cash
	Given I have an invoice with some bills
	And I have a bill to collect in the invoice
	When The bill is paid in cash
	Then The bill state is set to "Paid"
	And The bill payment method is set to "Cash"
	And The bill payment date is stored
	And The bill amount is deduced form the invoice total amount

Scenario: A bill to collect is paid by bank transfer
	Given I have an invoice with some bills
	And I have a bill to collect in the invoice
	When The bill is paid by bank transfer
	Then The bill state is set to "Paid"
	And The bill payment method is set to "Bank Transfer"
	And The transferor account is stored
	And The transferee account is stored
	And The bill payment date is stored
	And The bill amount is deduced form the invoice total amount

Scenario: A bill to collect is paid by direct debit
	Given I have an invoice with some bills
	And I have a bill to collect in the invoice
	When The bill is paid by direct debit
	Then The bill state is set to "Paid"
	And The bill payment method is set to "Direct Debit"
	And The direct debit initiation ID is stored
	And The bill payment date is stored
	And The bill amount is deduced form the invoice total amount

Scenario: All the bills of an invoice are paid
	Given I have an invoice with some bills
	When All the bills are paid
	Then The invoice state is set as "Paid"

Scenario: A bill is past due date
	Given I have an invoice with some bills
	And I have a bill to collect in the invoice
	When The bill is past due date
	Then The bill is marked as "Unpaid"
	And The invoice containig the bill is marked as "Unpaid"

Scenario: A bill with an associated agreement is past due date
	Given I have an invoice with some bills with agreements
	And I have a bill to collect in the invoice with a payment agreement
	When The bill is past due date
	Then The bill is marked as "Unpaid"
	And The invoice containig the bill is marked as "Unpaid"
	And The associated payment agreement is set to "NotAcomplished" for all bills involved on the agreement
	And The associated payment agreement is set to "NotAcomplished" for the invoice
	 
Scenario: A bill due date can be extended
	Given I have an invoice with some bills
	And I have a bill to collect in the invoice
	When I renew the due date
	Then The new due date is assigned to the bill

Scenario: A past due bill due date can be renewed
	Given I have an invoice with some bills
	And I have a bill to collect in the invoice
	And The bill is past due date
	When I renew the due date
	Then The new due date is assigned to the bill
	And The bill is marked as "ToCollect"
	And If there are no other bills marked as "Unpaid" the invoice is marked "ToBePaid"




