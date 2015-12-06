Feature: Generating pro forma invoices
	In order to give estimates to the club members
	As an administrtative assistant
	I want to generate pro forma invoices

Background: 
	Given Last generated pro forma invoice ID is "PRF2013000023"
			
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


Scenario: Generating a pro forma invoice for a set of service charges and sales
	Given This set of service charge transactions
	| Units | Service Name     | Description                  | Unit Cost | Tax          | Discount |
	| 2     | Rent a katamaran | Renta a katamaran for 2 days | 50        | IGIC General | 0        |
	| 2     | Rent a mouring   | Mouring May-June             | 150.00    | IGIC General | 20       |
	Given This set of sale transactions
	| Units | Product Name   | Description            | Unit Cost | Tax          | Discount |
	| 1     | Cup            | Blue Cup               | 10        | IGIC General | 0        |
	| 1     | Member ID Card | Lost ID Card Reprinted | 1.50      | No IGIC      | 50       |
	When I generate a pro forma invoice for this/these transaction/s
	Then A pro forma invoice is created for the cost of the service: 375.25

Scenario: A proforma invoice has no bill associated
	Given This set of service charge transactions
	| Units | Service Name     | Description                  | Unit Cost | Tax          | Discount |
	| 2     | Rent a katamaran | Renta a katamaran for 2 days | 50        | IGIC General | 0        |
	| 2     | Rent a mouring   | Mouring May-June             | 150.00    | IGIC General | 20       |
	When I generate a pro forma invoice for this/these transaction/s
	Then No bills are created for a pro forma invoice

Scenario: The invoice detail of a pro forma invoice can be edited
	Given I generate a pro forma invoice for this/these transaction/s
	| Units | Service Name   | Description      | Unit Cost | Tax          | Discount |
	| 2     | Rent a mouring | Mouring May-June | 150.00    | IGIC General | 0        |
	When I change the invoice detail to these values
	| Units | Service Name   | Description      | Unit Cost | Tax          | Discount |
	| 2     | Rent a mouring | Mouring May-June | 150.00    | IGIC General | 20       |
	Then The pro forma invoice is modified reflecting the new value: 256.80

