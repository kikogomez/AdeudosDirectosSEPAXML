Feature: Manage account numbers
	In order to create direct debits
	As an administrative assistant
	I want to process account numbers
	I want to store old incomplete bank account fields from previuos database
	I want to accept only valid accounts if CCC or IBAN are provided

Scenario: When I provide a valid bank account it is stored and CCC and IBAN is created
	Given This bank account "1234", "5678", "06", "1234567890" 
	When I process the bank account
	Then the bank account is considered "valid"
	 And the bank account is "stored"
	 And The CCC "12345678061234567890" is created
	 And The spanish IBAN code "ES6812345678061234567890" is created

Scenario: When I provide an invalid bank account it is stored but no CCC nor IBAN are created
	Given This bank account "1234", "5678", "05", "1234567890" 
	When I process the bank account
	Then the bank account is considered "invalid"
	 But the bank account is "stored"
	 And The CCC "" is created
	 And The spanish IBAN code "" is created

Scenario: When I provide an incomplete bank account it is stored but no CCC nor IBAN are created
	Given This bank account "", "5678", "05", "1234567890" 
	When I process the bank account
	Then the bank account is considered "invalid"
	 But the bank account is "stored"
	 And The CCC "" is created
	 And The spanish IBAN code "" is created

Scenario: When I provide a too long bank account it is not stored
	Given This bank account "1234", "5678", "06", "12345678901111" 
	When I process the bank account
	Then the bank account is considered "invalid"
	 And the bank account is "not stored"

Scenario: When I provide a valid CCC it is stored, bank account fields are created, and IBAN is created
	Given This CCC "12345678061234567890" 
	When I process the CCC
	Then the CCC is considered "valid"
	 And the CCC is "stored"
	 And the bank account "1234", "5678", "06", "1234567890" is created
	 And The spanish IBAN code "ES6812345678061234567890" is created

Scenario: When I provide a valid spanish IBAN it is stored, bank account fields are created, and CCC is created
	Given This IBAN "ES6812345678061234567890" 
	When I process the IBAN
	Then the IBAN is considered "valid"
	 And the IBAN is "stored"
	 And the bank account "1234", "5678", "06", "1234567890" is created
	 And The CCC "12345678061234567890" is created

Scenario: When I provide an ivalid CCC no info is stored nor created
	Given This CCC "12345678051234567890" 
	When I process the CCC
	Then the CCC is considered "invalid"
	 And the CCC is "not stored"

Scenario: When I provide a invalid spanish IBAN no info is stored nor created
	Given This IBAN "ES6812345678051234567890" 
	When I process the IBAN
	Then the IBAN is considered "invalid"
	 And the IBAN is "not stored"

Scenario Outline: Theese are the results when processing theese bank accounts
	Given This bank account <Bank>, <Office>, <ControlDigit>, <AccountNumber> 
	When I process the bank account
	Then the bank account is considered <valid>
	 And the bank account is <stored>
	 And The CCC <CCC> is created
	 And The spanish IBAN code <IBAN> is created

Scenarios:
| Bank   | Office | ControlDigit | AccountNumber  | valid     | stored      | CCC                    | IBAN                       |
| "1234" | "5678" | "06"         | "1234567890"   | "valid"   | "stored"    | "12345678061234567890" | "ES6812345678061234567890" |
| "1234" | "5678" | "05"         | "1234567890"   | "invalid" | "stored"    | ""                     | ""                         |
| ""     | ""     | "05"         | "1234567890"   | "invalid" | "stored"    | ""                     | ""                         |
| "1234" | "5678" | "06"         | "1234/56-0"    | "invalid" | "stored"    | ""                     | ""                         |
| "1234" | "5678" | "06"         | "123456789011" | "invalid" | "no stored" | ""                     | ""                         |

