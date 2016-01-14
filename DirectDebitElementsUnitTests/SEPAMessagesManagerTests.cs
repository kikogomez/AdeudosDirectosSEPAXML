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
        public void ACustomerDirectDebitRemmitanceXMLStringMessageIsCorrectlyGenerated()
        {
            DateTime creationDate = new DateTime(2015, 01, 10, 7, 15, 0);
            DateTime requestedCollectionDate = new DateTime(2015, 01, 15);

            DirectDebitRemittance directDebitRemmitance = new DirectDebitRemittance(creationDate, requestedCollectionDate, directDebitInitiationContract);
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment("CORE");
            string prefix = directDebitRemmitance.MessageID.Substring(directDebitRemmitance.MessageID.Length - 25);
            directDebitTransactionsGroupPayment.PaymentInformationID = prefix + "001";


            List<SimplifiedBill> simplifiedBills;
            string internalUniqueInstructionID;
            string mandateID;
            DateTime mandateSignatureDate;
            BankAccount debtorAccount;
            string debtorFullName;
            int transactionsCounter = 0;
            foreach (Debtor debtor in debtors.Values)
            {
                simplifiedBills = debtor.SimplifiedBills.Select(dictionaryElement => dictionaryElement.Value).ToList();
                internalUniqueInstructionID = (transactionsCounter + 1).ToString("000000");
                mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtor.DirectDebitmandates.First().Value.InternalReferenceNumber);
                mandateSignatureDate = debtor.DirectDebitmandates.First().Value.DirectDebitMandateCreationDate;
                debtorAccount = debtor.DirectDebitmandates.First().Value.BankAccount;
                debtorFullName = debtor.FullName;
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    simplifiedBills,
                    internalUniqueInstructionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    debtorFullName);
                transactionsCounter++;
                directDebitTransactionsGroupPayment.AddDirectDebitTransaction(directDebitTransaction);
            }

            directDebitRemmitance.AddDirectDebitTransactionsGroupPayment(directDebitTransactionsGroupPayment);

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            string xMLCustomerDirectDeitInitiationMessage = sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationMessage(
                creditor,
                creditorAgent,
                directDebitRemmitance);

            string xMLValidatingErrors = XMLValidator.ValidateXMLStringThroughXSDFile(
                xMLCustomerDirectDeitInitiationMessage,
                @"XSDFiles\pain.008.001.02.xsd");
            Assert.AreEqual("", xMLValidatingErrors);

            string expectedXMLString = File.ReadAllText(@"XML Test Files\pain.008.001.02\BasicDirectDebitRemmitanceExample.xml");           
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

            DateTime expectedMessageCreationDate = DateTime.Parse("2012-07-18T06:00:01");
            DateTime expectedRejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            List<string> expectedOriginalEndtoEndTransactionIdentificationList1 = new List<string>()
            {"201207010001/01002", "201207010001/02452"};
            List<string> expectedOriginalEndtoEndTransactionIdentificationList2 = new List<string>()
            {"201205270001/01650"};

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(expectedMessageCreationDate, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(expectedRejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual((decimal)220.30, paymentStatusReport.ControlSum);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitRemmitanceRejects[0].NumberOfTransactions);
            Assert.AreEqual((decimal)130.30, paymentStatusReport.DirectDebitRemmitanceRejects[0].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList1, paymentStatusReport.DirectDebitRemmitanceRejects[0].OriginalEndtoEndTransactionIdentificationList);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitRemmitanceRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)90, paymentStatusReport.DirectDebitRemmitanceRejects[1].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalEndtoEndTransactionIdentificationList);
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
            Assert.AreEqual(3, paymentStatusReport.DirectDebitRemmitanceRejects.Count);

            //Info from first DirectDectDebitRemmitance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList1 = new List<string>()
            {"15M/025450120151203", "15M/025720120151203"};
            Assert.AreEqual("2015-12-0112205515Rem.150 Ord.1", paymentStatusReport.DirectDebitRemmitanceRejects[0].OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitRemmitanceRejects[0].NumberOfTransactions);
            Assert.AreEqual((decimal)657.73, paymentStatusReport.DirectDebitRemmitanceRejects[0].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList1, paymentStatusReport.DirectDebitRemmitanceRejects[0].OriginalEndtoEndTransactionIdentificationList);

            //Info from second DirectDectDebitRemmitance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList2 = new List<string>()
            {"15M/022581120151204"};
            Assert.AreEqual("2015-12-0113442815Rem.151 Ord.1", paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitRemmitanceRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)277.45, paymentStatusReport.DirectDebitRemmitanceRejects[1].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalEndtoEndTransactionIdentificationList);

            //Info from third DirectDectDebitRemmitance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList3 = new List<string>()
            {"15M/026530120151204"};
            Assert.AreEqual("2015-12-0115153115Rem.152 Ord.1", paymentStatusReport.DirectDebitRemmitanceRejects[2].OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitRemmitanceRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)71.47, paymentStatusReport.DirectDebitRemmitanceRejects[2].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalEndtoEndTransactionIdentificationList);
        }
    }
}
