Feature: Generating invoices
	In order to bill the club members
	As an administrtative assistant
	I want to generate invoices

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

Scenario: Generate an invoice for a service charge
	Given The member uses the club service "Rent a kajak"
	When I generate an invoice for the service
	Then An invoice is created for the cost of the service: 53.50
	And The invoice state is "To be paid"
	And The invoice is assigned to the Club Member

Scenario: Generate an invoice for a sale
	Given The member buys a "Pennant"
	When I generate an invoice for the sale
	Then An invoice is created for the cost of the sale: 10.70
	And The invoice state is "To be paid"
	And The invoice is assigned to the Club Member

Scenario: The invoices ID must allways be consecutive
	Given The member uses the club service "Rent a kajak"
	When I generate an invoice for the service
	Then The generated Invoice ID should be "INV2013000023"
	Then The next invoice sequence number should be 24

Scenario: Up to 999999 invoices in a year
	Given Last generated InvoiceID is "INV2013999999"
	Given The member uses the club service "Rent a mouring"
	When I try to generate an invoice for the service
	Then The application doesn't accept more than 999999 invoices in the year

Scenario: Generate an invoice for multiple transactions with one tax type
	Given This set of service charge transactions
	| Units | Service Name   | Description              | Unit Cost | Tax          | Discount |
	| 1     | Rent a kajak   | Rent a kajak for one day | 50.00     | IGIC General | 0        |
	| 2     | Rent a mouring | Mouring May-June         | 150.00    | IGIC General | 0        |
	When I generate an invoice for this/these transaction/s
	Then An invoice is created for the cost of the service: 374.50
	And The invoice state is "To be paid"

Scenario: Generate an invoice for multiple transactions with different tax type
	Given This set of service charge transactions
	| Units | Service Name                | Description      | Unit Cost | Tax          | Discount |
	| 1     | Full Membership Monthly Fee | Monthly Fee June | 79.00     | No IGIC      | 0        |
	| 2     | Rent a mouring              | Mouring May-June | 150.00    | IGIC General | 0        |
	When I generate an invoice for this/these transaction/s
	Then An invoice is created for the cost of the service: 400.00
	And The invoice state is "To be paid"

Scenario: Discounts on transactions must be applied before taxes
	Given This set of service charge transactions
	| Units | Service Name   | Description      | Unit Cost | Tax          | Discount |
	| 1     | Rent a mouring | Mouring May-June | 150.00    | IGIC General | 20       |
	When I generate an invoice for this/these transaction/s
	Then An invoice is created for the cost of the service: 128.40
	And The invoice state is "To be paid"

Scenario: Rounding: Round to two decimals Away From Zero
	Given This set of service charge transactions
	| Units | Service Name   | Description      | Unit Cost | Tax          | Discount |
	| 1     | Rent a mouring | Mouring May-June | 150.00    | IGIC General | 15       |
	When I generate an invoice for this/these transaction/s
	Then An invoice is created for the cost of the service: 136.43
	And The invoice state is "To be paid"

Scenario: Rounding: First calculate discount on unit, then round, then tax unit, then round, then sum units
	Given This set of service charge transactions
	| Units | Service Name     | Description                  | Unit Cost | Tax          | Discount |
	| 2     | Rent a katamaran | Renta a katamaran for 2 days | 100.55    | IGIC General | 15       |
	When I generate an invoice for this/these transaction/s
	Then An invoice is created for the cost of the service: 182.90
	And The invoice state is "To be paid"

Scenario: Transactions can have differnt cost and tax than default service ones
	Given This set of service charge transactions
	| Units | Service Name     | Description                  | Unit Cost | Tax     | Discount |
	| 1     | Rent a katamaran | Renta a katamaran for 2 days | 90        | No IGIC | 0        |
	When I generate an invoice for this/these transaction/s
	Then An invoice is created for the cost of the service: 90.00
	And The invoice state is "To be paid"

Scenario: We can mix services charges and sales in a single invoice
	Given This set of service charge transactions
	| Units | Service Name     | Description                  | Unit Cost | Tax          | Discount |
	| 2     | Rent a katamaran | Renta a katamaran for 2 days | 50        | IGIC General | 0        |
	| 2     | Rent a mouring   | Mouring May-June             | 150.00    | IGIC General | 20       |
	Given This set of sale transactions
	| Units | Product Name   | Description            | Unit Cost | Tax          | Discount |
	| 1     | Cup            | Blue Cup               | 10        | IGIC General | 0        |
	| 1     | Member ID Card | Lost ID Card Reprinted | 1.50      | No IGIC      | 50       |
	When I generate an invoice for this/these transaction/s
	Then An invoice is created for the cost of the service: 375.25
	And The invoice state is "To be paid"


