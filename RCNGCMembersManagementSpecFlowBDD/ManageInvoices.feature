Feature: Manage Invoices
	In order to control the debt
	As a an administrtative assistant
	I want to manage the created invoices

Background: 
	Given Last generated InvoiceID is "INV2013000023"
	
	Given A Club Member
	| MemberID | Name      | FirstSurname  | SecondSurname |
	| 00001    | Francisco | Gomez-Caldito | Viseas        |

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


Scenario: In some special cases, an invoice can be cancelled
	Given I have an invoice for the service "Rent a kajak"
	When I cancel the invoice
	Then The invoice state is "Cancelled"
	And All the pending bills are marked as Cancelled
	And The bill total amount to be paid is 0
	And An amending invoice is created for the negative value of the original invoice: -53.50 
	And The taxes devolution (-3.50) is separated from the base cost devolution (-50)
	And The amending invoice ID is the same than the original invoice with different prefix: "AMN2013000023"



