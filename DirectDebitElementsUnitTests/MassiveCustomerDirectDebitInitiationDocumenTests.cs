using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Billing;
using DirectDebitElements;
using ReferencesAndTools;
using XMLSerializerValidator;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class MassiveCustomerDirectDebitInitiationDocumenTests
    {
        static DateTime creationDate;
        static DateTime requestedCollectionDate;
        static string messageID;
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;
        static DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator;
        //static bool singleUnstructuredConcept;

        

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            creationDate = new DateTime(2015, 01, 10, 7, 15, 0);
            requestedCollectionDate = new DateTime(2015, 01, 15);
            messageID = "PREG1234567815011007:15:00";
            creditor = new Creditor("G12345678", "NOMBRE ACREEDOR PRUEBAS");
            creditorAgent = new CreditorAgent(new BankCode("2100", "CaixaBank", "CAIXESBBXXX"));
            directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")),
                creditor.NIF,
                "011",
                creditorAgent);
            directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);
            //singleUnstructuredConcept = false;
        }

        [TestMethod]
        [Ignore]
        public void CreateAXMLCustomerDirectDebitInitiationStringMessageFromAListOf8000TransactionsInOnePaymentInstruction()
        {
            System.Diagnostics.Stopwatch testStopWatch = new System.Diagnostics.Stopwatch();
            testStopWatch.Start();

            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>();
            DirectDebitPaymentInstruction directDebitPaymentInstruction = CreateAutomatedDirectDebitPaymentInstruction(
                1, "CORE", false, 8000);
            directDebitPaymentInstructions.Add(directDebitPaymentInstruction);
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                messageID,
                creationDate,
                requestedCollectionDate,
                directDebitInitiationContract,
                directDebitPaymentInstructions);

            bool singleUnstructuredConcept = false;
            bool conceptIncludesAmount = false;
            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            string xMLCustomerDirectDeitInitiationMessage = sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationStringMessage(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept,
                conceptIncludesAmount);

            testStopWatch.Stop();

            Assert.IsTrue(testStopWatch.ElapsedMilliseconds<1000);     //It should no take more than a second
        }

        [TestMethod]
        [Ignore]
        public void CreateAXMLCustomerDirectDebitInitiationFileMessageFromAListOf8000TransactionsInOnePaymentInstruction()
        {
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>();
            DirectDebitPaymentInstruction directDebitPaymentInstruction = CreateAutomatedDirectDebitPaymentInstruction(
                1, "CORE", false, 8000);
            directDebitPaymentInstructions.Add(directDebitPaymentInstruction);
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                messageID,
                creationDate,
                requestedCollectionDate,
                directDebitInitiationContract,
                directDebitPaymentInstructions);

            bool singleUnstructuredConcept = false;
            bool conceptsIncludeAmounts = false;
            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationFileMessage(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept,
                conceptsIncludeAmounts,
                @"XML Test Files\pain.008.001.02\HugeDirectDebitInitiation.xml");

            Assert.IsTrue(true);  //If test reaches here, it's ok
        }

        private DirectDebitPaymentInstruction CreateAutomatedDirectDebitPaymentInstruction(
            int paymentIntntructionIDIndex,
            string localInstrument,
            bool firstDebits,
            int numberOfTransactions)
        {
            string paymentInstructionID = messageID + "-" + paymentIntntructionIDIndex.ToString("00");
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>();
            for (int transactionsCounter = 1; transactionsCounter <= 8000; transactionsCounter++)
            {
                string formattedCounter = transactionsCounter.ToString("00000");
                string debtorName = "NOMBRE DEUDOR DE PRUEBAS " + formattedCounter;

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
                paymentInstructionID, localInstrument, firstDebits, directDebitTransactions);
            directDebitPaymentInstructions.Add(directDebitPaymentInstruction);

            return directDebitPaymentInstruction;
        }
    }
}
