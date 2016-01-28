using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Billing;
using DirectDebitElements;
using ReferencesAndTools;
using XMLSerializerValidator;


namespace DirectDebitElementsUnitTests
{
    public class MassiveCustomerDirectDebitInitiationDocumenTests
    {
        static Dictionary<string, Debtor> debtors;
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;
        static DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {

            debtors = new Dictionary<string, Debtor>()
            {
                {"00001", new Debtor("00001", "Francisco", "Gómez-Caldito", "Viseas")},
                {"00002", new Debtor("00002", "Pedro", "Pérez", "Gómez")}
            };

            creditor = new Creditor("G12345678", "NOMBRE ACREEDOR PRUEBAS");
            creditorAgent = new CreditorAgent(new BankCode("2100", "CaixaBank", "CAIXESBBXXX"));
            directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")),
                creditor.NIF,
                "011",
                creditorAgent);
            directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);

            var directDebitmandateslist = new[]
            {
                new {debtorID = "00001", internalReference = 1234, ccc = "21002222002222222222" },
                new {debtorID = "00002", internalReference = 1235, ccc = "21003333802222222222" }
            };

            foreach (var ddM in directDebitmandateslist)
            {
                DirectDebitMandate directDebitMandate = new DirectDebitMandate(
                    ddM.internalReference,
                    new DateTime(2013, 11, 11),
                    new BankAccount(new ClientAccountCodeCCC(ddM.ccc)),
                    debtors[ddM.debtorID].FullName);
                debtors[ddM.debtorID].AddDirectDebitMandate(directDebitMandate);
            }

            var billsList = new[]
            {
                new {debtorID = "00001", billID= "00001/01", Amount = 79, transactionDescription = "Cuota Social Octubre 2013" },
                new {debtorID = "00002", billID= "00002/01",Amount = 79, transactionDescription="Cuota Social Octubre 2013" },
                new {debtorID = "00002", billID= "00002/02",Amount = 79, transactionDescription="Cuota Social Noviembre 2013"}
            };

            foreach (var bill in billsList)
            {

                Debtor debtor = debtors[bill.debtorID];
                SimplifiedBill bills = new SimplifiedBill(
                    bill.billID,
                    bill.transactionDescription,
                    bill.Amount,
                    DateTime.Today,
                    DateTime.Today.AddMonths(1));
                debtor.AddSimplifiedBill(bills);
            }
        }

        [TestMethod]
        [Ignore]
        public void CreateACustomerDirectDebitInitiationTestFromAListofTransactionsWith100TransactionsInOnePaymentInstruction()
        {
            DateTime creationDate = new DateTime(2015, 01, 10, 7, 15, 0);
            string messageID = "PREG1234567815011007:15:00";
            DateTime requestedCollectionDate = new DateTime(2015, 01, 15);
            Creditor creditor = new Creditor("G12345678", "NOMBRE ACREEDOR PRUEBAS");
            CreditorAgent creditorAgent = new CreditorAgent(new BankCode("2100", "CaixaBank", "CAIXESBBXXX"));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")),
                creditor.NIF,
                "011",
                creditorAgent);
            string paymentInstructionID = messageID + "-01";

            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>();
            for (int transactionsCounter = 1; transactionsCounter <= 100; transactionsCounter++)
            {
                string formattedCounter = transactionsCounter.ToString("00000");
                string debtorName = "NOMBRE ACREEDOR DE PRUEBAS" + formattedCounter;

                SimplifiedBill simplifiedBill = new SimplifiedBill("RecId" + formattedCounter, "Cuota Mensual Enero", 80, new DateTime(2016, 01, 01), new DateTime(2016, 02, 01));
                string transactionID = paymentInstructionID + formattedCounter;
                string debtorMandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(10000 + transactionsCounter);
                DateTime mandateSignaturaDate = new DateTime(2009, 10, 31);
                BankAccount debtorAccount = new 
                    BankAccount(new ClientAccountCodeCCC(BankAccountNumberChecker.CalculateCCC("2100", "9999", "12345" + formattedCounter)));
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    new List<SimplifiedBill>() { simplifiedBill },
                    transactionID,
                    debtorMandateID,
                    mandateSignaturaDate,
                    debtorAccount,
                    debtorName,
                    null,
                    false);
                directDebitTransactions.Add(directDebitTransaction);
            }
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>();
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInstructionID, "CORE", false, directDebitTransactions);
            directDebitPaymentInstructions.Add(directDebitPaymentInstruction);
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                messageID,
                creationDate,
                requestedCollectionDate,
                directDebitInitiationContract);

            bool singleUnstructuredConcept = false;

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            string xMLCustomerDirectDeitInitiationMessage = sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationMessage(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept);

            //string xMLValidatingErrors = XMLValidator.ValidateXMLStringThroughXSDFile(
            //    xMLCustomerDirectDeitInitiationMessage,
            //    @"XSDFiles\pain.008.001.02.xsd");
            //Assert.AreEqual("", xMLValidatingErrors);
            //string expectedXMLString = File.ReadAllText(@"XML Test Files\pain.008.001.02\BasicDirectDebitRemittanceExample.xml");
            //Assert.AreEqual(expectedXMLString, xMLCustomerDirectDeitInitiationMessage);
        }
    }
}
