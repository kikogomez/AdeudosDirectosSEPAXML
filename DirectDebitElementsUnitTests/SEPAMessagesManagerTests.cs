using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Billing;
using DirectDebitElements;
using ReferencesAndTools;
using XMLSerializerValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class SEPAMessagesManagerTests
    {
        static Dictionary<string, Debtor> debtors;
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;
        static DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator;

        static string messageID;
        static string paymentInformationID1;
        static string paymentInformationID2;
        static DateTime creationDate;
        static DateTime requestedCollectionDate;

        static DirectDebitAmendmentInformation mandateIDAmendment;
        static DirectDebitAmendmentInformation bankAccountAmendment_SameBank;
        static DirectDebitAmendmentInformation bankAccountAmendment_DifferentBank;

        DirectDebitTransaction directDebitTransaction1;
        DirectDebitTransaction directDebitTransaction2;
        DirectDebitTransaction directDebitTransactionWithMandateIDAmendment;
        DirectDebitTransaction directDebitTransactionWithBankAcountAmendment_SameBank;
        DirectDebitTransaction directDebitTransactionWithBankAcountAmendment_DifferentBank;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            debtors = new Dictionary<string, Debtor>()
            {
                {"00001", new Debtor("00001", "Francisco", "Gómez-Caldito", "Viseas")},
                {"00002", new Debtor("00002", "Pedro", "Pérez", "Gómez")},
                {"00003", new Debtor("00003", "Rodrigo", "Rodríguez", "Rodríguez")},
                {"00004", new Debtor("00004", "Domingo", "Domínguez", "Domínguez")}
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
                new {debtorID = "00001", internalReference = 1234, ccc = BankAccountNumberChecker.CalculateCCC("2100", "2222", "2222222222"), bIC="CAIXESBBXXX"},
                new {debtorID = "00002", internalReference = 1235, ccc = BankAccountNumberChecker.CalculateCCC("2100", "3333", "2222222222"), bIC="CAIXESBBXXX"},
                new {debtorID = "00003", internalReference = 1236, ccc = BankAccountNumberChecker.CalculateCCC("2100", "4444", "2222222222"), bIC="CAIXESBBXXX"},
                new {debtorID = "00004", internalReference = 1237, ccc = BankAccountNumberChecker.CalculateCCC("2100", "5555", "2222222222"), bIC="CAIXESBBXXX"}
            };

            foreach (var ddM in directDebitmandateslist)
            {
                DirectDebitMandate directDebitMandate = new DirectDebitMandate(
                    ddM.internalReference,
                    new DateTime(2013, 11, 11),
                    new BankAccount(new ClientAccountCodeCCC(ddM.ccc)),
                    ddM.bIC,
                    debtors[ddM.debtorID].FullName);
                debtors[ddM.debtorID].AddDirectDebitMandate(directDebitMandate);
            }

            var billsList = new[]
            {
                new {debtorID = "00001", billID= "00001/01", amount = 79.00, transactionDescription = "Cuota Social Octubre 2014" },
                new {debtorID = "00002", billID= "00002/01",amount = 79.00, transactionDescription="Cuota Social Octubre 2014" },
                new {debtorID = "00002", billID= "00002/02",amount = 79.00, transactionDescription="Cuota Social Noviembre 2014"},
                new {debtorID = "00003", billID= "00003/01",amount = 79.00, transactionDescription="Cuota Social Noviembre 2014"},
                new {debtorID = "00004", billID= "00004/01",amount = 79.50, transactionDescription="Cuota Social Noviembre 2014"}
            };

            foreach (var bill in billsList)
            {

                Debtor debtor = debtors[bill.debtorID];
                SimplifiedBill bills = new SimplifiedBill(
                    bill.billID,
                    bill.transactionDescription,
                    (decimal)bill.amount,
                    DateTime.Today,
                    DateTime.Today.AddMonths(1));
                debtor.AddSimplifiedBill(bills);
            }

            messageID = messageID = "PREG1234567815011007:15:00";
            paymentInformationID1 = "PREG1234567815011007:15:00-01";
            paymentInformationID2 = "PREG1234567815011007:15:00-02";
            creationDate = new DateTime(2015, 01, 10, 7, 15, 0);          
            requestedCollectionDate = new DateTime(2015, 01, 15);

            mandateIDAmendment = new DirectDebitAmendmentInformation(
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(1000),
                null);

            bankAccountAmendment_SameBank = new DirectDebitAmendmentInformation(
                null,
                new BankAccount(new ClientAccountCodeCCC(BankAccountNumberChecker.CalculateCCC("2100", "0000", "1234567890"))));

            bankAccountAmendment_DifferentBank = new DirectDebitAmendmentInformation(
                null,
                new BankAccount(new ClientAccountCodeCCC(BankAccountNumberChecker.CalculateCCC("0182","0000", "1234567890"))));
        }

        [TestInitialize]
        public void InitializeTransacions()
        {
            // La inicializacion de las transacciones no se hace con variables estáticas
            // pues la suscripcion a eventos interacciona entre los tests

            directDebitTransaction1 = new DirectDebitTransaction(
                debtors["00001"].SimplifiedBills.Values.ToList(),
                paymentInformationID1 + "00001",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00001"].DirectDebitmandates[1234].InternalReferenceNumber),
                debtors["00001"].DirectDebitmandates[1234].DirectDebitMandateCreationDate,
                debtors["00001"].DirectDebitmandates[1234].BankAccount,
                debtors["00001"].FullName,
                null,
                false);

            directDebitTransaction2 = new DirectDebitTransaction(
                debtors["00002"].SimplifiedBills.Values.ToList(),
                paymentInformationID1 + "00002",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00002"].DirectDebitmandates[1235].InternalReferenceNumber),
                debtors["00002"].DirectDebitmandates[1235].DirectDebitMandateCreationDate,
                debtors["00002"].DirectDebitmandates[1235].BankAccount,
                debtors["00002"].FullName,
                null,
                false);

            directDebitTransactionWithMandateIDAmendment = new DirectDebitTransaction(
                debtors["00002"].SimplifiedBills.Values.ToList(),
                paymentInformationID1 + "00002",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00002"].DirectDebitmandates[1235].InternalReferenceNumber),
                debtors["00002"].DirectDebitmandates[1235].DirectDebitMandateCreationDate,
                debtors["00002"].DirectDebitmandates[1235].BankAccount,
                debtors["00002"].FullName,
                mandateIDAmendment,
                false);

            directDebitTransactionWithBankAcountAmendment_SameBank = new DirectDebitTransaction(
                debtors["00003"].SimplifiedBills.Values.ToList(),
                paymentInformationID1 + "00003",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00003"].DirectDebitmandates[1236].InternalReferenceNumber),
                debtors["00003"].DirectDebitmandates[1236].DirectDebitMandateCreationDate,
                debtors["00003"].DirectDebitmandates[1236].BankAccount,
                debtors["00003"].FullName,
                bankAccountAmendment_SameBank,
                false);

            directDebitTransactionWithBankAcountAmendment_DifferentBank = new DirectDebitTransaction(
                debtors["00004"].SimplifiedBills.Values.ToList(),
                paymentInformationID2 + "00001",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00004"].DirectDebitmandates[1237].InternalReferenceNumber),
                debtors["00004"].DirectDebitmandates[1237].DirectDebitMandateCreationDate,
                debtors["00004"].DirectDebitmandates[1237].BankAccount,
                debtors["00004"].FullName,
                bankAccountAmendment_DifferentBank,
                true);
        }

        [TestMethod]
        public void ACustomerDirectDebitRemittanceXMLStringMessageIsCorrectlyGenerated()
        {
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);
            DirectDebitPaymentInstruction directDebitPaymentInstruction1 = new DirectDebitPaymentInstruction(
                paymentInformationID1,
                "CORE",
                false,
                new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 });

            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction1);

            bool singleUnstructuredConcept = false;

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            string xMLCustomerDirectDebitInitiationMessage = sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationStringMessage(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept);

            string xMLValidatingErrors = XMLValidator.ValidateXMLStringThroughXSDFile(
                xMLCustomerDirectDebitInitiationMessage,
                @"XSDFiles\pain.008.001.02.xsd");
            Assert.AreEqual("", xMLValidatingErrors);
            string expectedXMLString = File.ReadAllText(@"XML Test Files\pain.008.001.02\BasicDirectDebitRemittanceExample.xml");
            Assert.AreEqual(expectedXMLString, xMLCustomerDirectDebitInitiationMessage);
        }

        [TestMethod]
        public void ACustomerDirectDebitRemittanceXMLStringMessageWithConceptsJoinedIsCorrectlyGenerated()
        {
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);
            DirectDebitPaymentInstruction directDebitPaymentInstruction1 = new DirectDebitPaymentInstruction(
                paymentInformationID1,
                "CORE",
                false,
                new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 });

            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction1);

            bool singleUnstructuredConcept = true;

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            string xMLCustomerDirectDeitInitiationMessage = sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationStringMessage(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept);

            string xMLValidatingErrors = XMLValidator.ValidateXMLStringThroughXSDFile(
                xMLCustomerDirectDeitInitiationMessage,
                @"XSDFiles\pain.008.001.02.xsd");
            Assert.AreEqual("", xMLValidatingErrors);
            string expectedXMLString = File.ReadAllText(@"XML Test Files\pain.008.001.02\BasicDirectDebitRemittanceExampleWithConceptsJoined.xml");           
            Assert.AreEqual(expectedXMLString, xMLCustomerDirectDeitInitiationMessage);
        }

        [TestMethod]
        public void ACustomerDirectDebitRemittanceXMLStringMessageWithTwoPaymentInstructionsAndBankAccountAmendmentsFromSameAndDifferentDebtorAgents()
        {
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);
            DirectDebitPaymentInstruction directDebitPaymentInstruction1 = new DirectDebitPaymentInstruction(
                paymentInformationID1,
                "CORE",
                false,
                new List<DirectDebitTransaction>() {
                    directDebitTransaction1,
                    directDebitTransactionWithMandateIDAmendment,
                    directDebitTransactionWithBankAcountAmendment_SameBank});

            DirectDebitPaymentInstruction directDebitPaymentInstruction2 = new DirectDebitPaymentInstruction(
                paymentInformationID2,
                "CORE",
                true,
                new List<DirectDebitTransaction>() { directDebitTransactionWithBankAcountAmendment_DifferentBank });

            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction1);
            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction2);

            bool singleUnstructuredConcept = true;

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            string xMLCustomerDirectDeitInitiationMessage = sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationStringMessage(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept);

            string xMLValidatingErrors = XMLValidator.ValidateXMLStringThroughXSDFile(
                xMLCustomerDirectDeitInitiationMessage,
                @"XSDFiles\pain.008.001.02.xsd");
            Assert.AreEqual("", xMLValidatingErrors);
            string expectedXMLString = File.ReadAllText(@"XML Test Files\pain.008.001.02\DirectDebitRemmitanceWithVariousPaymentInstructionsAndAmendments.xml");
            Assert.AreEqual(expectedXMLString, xMLCustomerDirectDeitInitiationMessage);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyInitializedFromAXMLStringMessage()
        {
            string xMLFilePath = @"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example1.xml";
            StreamReader fileReader = new StreamReader(xMLFilePath);
            string paymentStatusReportXMLStringMessage = fileReader.ReadToEnd();

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            PaymentStatusReport paymentStatusReport = sEPAMessagesManager.ReadISO20022PaymentStatusReportMessage(paymentStatusReportXMLStringMessage);

            //General info from file
            DateTime expectedMessageCreationDate = DateTime.Parse("2012-07-18T06:00:01");
            DateTime expectedRejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(expectedMessageCreationDate, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(expectedRejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual((decimal)220.30, paymentStatusReport.ControlSum);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitPaymentInstructionRejects.Count);

            //Info from first DirectDebitPaymentInstructionReject
            List<string> expectedOriginalEndtoEndTransactionIdentificationList1 = new List<string>()
            {"201207010001/01002", "201207010001/02452"};
            Assert.AreEqual("PRE201207010001", paymentStatusReport.DirectDebitPaymentInstructionRejects[0].OriginalPaymentInformationID);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].NumberOfTransactions);
            Assert.AreEqual((decimal)130.30, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList1, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].OriginalEndtoEndTransactiontransactionIDList);

            //Info from second DirectDebitPaymentInstructionReject
            List<string> expectedOriginalEndtoEndTransactionIdentificationList2 = new List<string>()
            {"201205270001/01650"};
            Assert.AreEqual("PRE201205270001", paymentStatusReport.DirectDebitPaymentInstructionRejects[1].OriginalPaymentInformationID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)90, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].OriginalEndtoEndTransactiontransactionIDList);
        }

        [TestMethod]
        public void AnotherPaymentStatusReportIsCorrectlyInitializedFromAXMLStringMessage()
        {
            string xMLFilePath = @"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example2.xml";
            StreamReader fileReader = new StreamReader(xMLFilePath);
            string paymentStatusReportXMLStringMessage = fileReader.ReadToEnd();

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            PaymentStatusReport paymentStatusReport = sEPAMessagesManager.ReadISO20022PaymentStatusReportMessage(paymentStatusReportXMLStringMessage);

            //General info from file
            DateTime expectedMessageCreationDate = DateTime.Parse("2015-12-08T06:00:01");
            DateTime expectedRejectAccountChargeDateTime = DateTime.Parse("2015-12-08");
            Assert.AreEqual("DAG35008770033201512080511490000677", paymentStatusReport.MessageID);
            Assert.AreEqual(expectedMessageCreationDate, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(expectedRejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(4, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual((decimal)1006.65, paymentStatusReport.ControlSum);
            Assert.AreEqual(3, paymentStatusReport.DirectDebitPaymentInstructionRejects.Count);

            //Info from first DirectDectDebitRemittance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList1 = new List<string>()
            {"15M/025450120151203", "15M/025720120151203"};
            Assert.AreEqual("2015-12-0112205515Rem.150 Ord.1", paymentStatusReport.DirectDebitPaymentInstructionRejects[0].OriginalPaymentInformationID);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].NumberOfTransactions);
            Assert.AreEqual((decimal)657.73, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList1, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].OriginalEndtoEndTransactiontransactionIDList);

            //Info from second DirectDectDebitRemittance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList2 = new List<string>()
            {"15M/022581120151204"};
            Assert.AreEqual("2015-12-0113442815Rem.151 Ord.1", paymentStatusReport.DirectDebitPaymentInstructionRejects[1].OriginalPaymentInformationID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)277.45, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].OriginalEndtoEndTransactiontransactionIDList);

            //Info from third DirectDectDebitRemittance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList3 = new List<string>()
            {"15M/026530120151204"};
            Assert.AreEqual("2015-12-0115153115Rem.152 Ord.1", paymentStatusReport.DirectDebitPaymentInstructionRejects[2].OriginalPaymentInformationID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)71.47, paymentStatusReport.DirectDebitPaymentInstructionRejects[2].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].OriginalEndtoEndTransactiontransactionIDList);
        }
    }
}
