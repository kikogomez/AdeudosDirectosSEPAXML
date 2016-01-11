using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;
using Billing;
using ReferencesAndTools;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class DirectDebitRemmitanceUnitTest
    {
        static Dictionary<string, Debtor> debtors;
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;
        static BankCodes spanishBankCodes;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            debtors = new Dictionary<string, Debtor>()
            {
                {"00001", new Debtor("00001", "Francisco", "Gómez-Caldito", "Viseas")},
                {"00002", new Debtor("00002", "Pedro", "Pérez", "Gómez")}
            };

            creditor = new Creditor("G12345678", "NOMBRE ACREEDOR PRUEBAS");
            creditorAgent = new CreditorAgent(new BankCode("2100", "CaixaBank","CAIXESBBXXX"));
            directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")),
                creditor.NIF,
                "777",
                creditorAgent);

            var directDebitmandateslist = new[]
            {
                new {debtorID = "00001", internalReference = 1234, ccc = "21002222002222222222" },
                new {debtorID = "00002", internalReference = 1235, ccc = "21003333802222222222" }
            };

            foreach (var ddM in directDebitmandateslist)
            {
                DirectDebitMandate directDebitMandate = new DirectDebitMandate(
                    ddM.internalReference,
                    new DateTime(2013,11,11),
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

            spanishBankCodes = new BankCodes(@"XMLFiles\SpanishBankCodes.xml", BankCodes.BankCodesFileFormat.XML);
        }

        [TestMethod]
        public void ADirectDebittRemmitanceInstanceIsCorrectlyCreated()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            DirectDebitRemittance directDebitRemmitance = new DirectDebitRemittance(creationDate, requestedCollectionDate, directDebitInitiationContract);
            string expectedMessageId = "ES26777G123456782013113007:15:00";
            Assert.AreEqual(expectedMessageId, directDebitRemmitance.MessageID);
            Assert.AreEqual(creationDate, directDebitRemmitance.CreationDate);
            Assert.AreEqual(requestedCollectionDate, directDebitRemmitance.RequestedCollectionDate);
        }

        [TestMethod]
        public void TheDirectDebitRemmitanceMessageIDCanBeDirectlyAsignedAtAnyMoment()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            DirectDebitRemittance directDebitRemmitance = new DirectDebitRemittance(creationDate, requestedCollectionDate, directDebitInitiationContract);
            directDebitRemmitance.MessageID = "TestingID";
            Assert.AreEqual("TestingID", directDebitRemmitance.MessageID);
            Assert.AreEqual(creationDate, directDebitRemmitance.CreationDate);
            Assert.AreEqual(requestedCollectionDate, directDebitRemmitance.RequestedCollectionDate);
        }

        [TestMethod]
        public void AnEmptyDirectDebitTransactionIsCorrectlyCreated()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            Assert.AreEqual(internalDirectDebitReferenceNumber, directDebitTransaction.MandateInternalReferenceNumber);
            Assert.AreEqual(debtorAccount, directDebitTransaction.DebtorAccount);
            Assert.AreEqual(0, directDebitTransaction.NumberOfBills);
        }

        [TestMethod]
        public void ADirectDebitTransactionIsCorrectlyCreated()
        {
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(bills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            Assert.AreEqual(bills, directDebitTransaction.BillsInTransaction);
            Assert.AreEqual(internalDirectDebitReferenceNumber, directDebitTransaction.MandateInternalReferenceNumber);
            Assert.AreEqual(debtorAccount, directDebitTransaction.DebtorAccount);
            Assert.AreEqual((decimal)158, directDebitTransaction.Amount);
            Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
        }

        [TestMethod]
        public void WhenAddingAnotherBillToADirectDebitTransactionTheAmmountAndNumberOfBillsAreCorrectlyUpdated()
        {
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = new List<SimplifiedBill> { debtor.SimplifiedBills.ElementAt(0).Value };
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(bills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            Assert.AreEqual((decimal)79, directDebitTransaction.Amount);
            Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
            SimplifiedBill bill = debtor.SimplifiedBills.ElementAt(1).Value;
            directDebitTransaction.AddBill(bill);
            Assert.AreEqual((decimal)158, directDebitTransaction.Amount);
            Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
        }

        [TestMethod]
        public void ADirecDebitTransactionGroupPaymnetIsCorrectlyCreated()
        {
            string localInstrument = "COR1";
            DirectDebitTransactionsGroupPayment dDTxGrpPaymentInfo = new DirectDebitTransactionsGroupPayment(localInstrument);
            Assert.AreEqual("COR1", dDTxGrpPaymentInfo.LocalInstrument);
        }

        [TestMethod]
        public void ADirectDebitTransactionIsCorrectlyAddedToGroupPayment()
        {
            string localInstrument = "COR1";
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment(localInstrument);

            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = new List<SimplifiedBill> { debtor.SimplifiedBills.ElementAt(0).Value };
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(bills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            directDebitTransactionsGroupPayment.AddDirectDebitTransaction(directDebitTransaction);
            Assert.AreEqual(1, directDebitTransactionsGroupPayment.NumberOfDirectDebitTransactions);
            Assert.AreEqual((decimal)79, directDebitTransactionsGroupPayment.TotalAmount);
        }

        [TestMethod]
        public void APaymentGroupIsCorrectlyAddedToADirectDebitRemmitance()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            DateTime requestedCollectionDate = new DateTime(2013, 12,1);
            DirectDebitRemittance directDebitRemmitance = new DirectDebitRemittance(creationDate, requestedCollectionDate, directDebitInitiationContract);
            string localInstrument = "COR1";
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment(localInstrument);
            directDebitRemmitance.AddDirectDebitTransactionsGroupPayment(directDebitTransactionsGroupPayment);
            Assert.AreEqual(1, directDebitRemmitance.DirectDebitTransactionGroupPaymentCollection.Count);
        }

        [TestMethod]
        public void ThePaymentInformationIDOfADirectDebitTransactionGroupPaymentCanBeDirectlyAsignedAtAnyMoment() //Actualizar los ID al generar el mensaje 
        {
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment("COR1");
            directDebitTransactionsGroupPayment.PaymentInformationID = "testingID";
            Assert.AreEqual("testingID", directDebitTransactionsGroupPayment.PaymentInformationID);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantAssignAPaymentInformationIDLongerThan35Characters()
        {
            try
            {
                DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment("COR1");
                directDebitTransactionsGroupPayment.PaymentInformationID = "0123456789012345678901234567890123456789";
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("PaymentInformationID", e.ParamName);
                Assert.AreEqual("PaymentInformationID lenght can't exceed 35 characters", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantAssignAnEmptyPaymentInformationID()
        {
            try
            {
                DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment("COR1");
                directDebitTransactionsGroupPayment.PaymentInformationID = "";
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("PaymentInformationID", e.ParamName);
                Assert.AreEqual("PaymentInformationID lenght can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void CantAssignANullPaymentInformationID()
        {
            try
            {
                DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment("COR1");
                directDebitTransactionsGroupPayment.PaymentInformationID = null;
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("PaymentInformationID", e.ParamName);
                Assert.AreEqual("PaymentInformationID can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        public void TheInstructionIDOfADirectDebitTransactionCanBeDirectlyAsignedAtAnyMoment()
        {
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = new List<SimplifiedBill> { debtor.SimplifiedBills.ElementAt(0).Value };
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(bills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            directDebitTransaction.InternalUniqueInstructionID = "testingID";
            Assert.AreEqual("testingID", directDebitTransaction.InternalUniqueInstructionID);
        }

        [TestMethod]
        public void TheMandateIDOfADirectDebitIsWellGenerated() 
        {
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = new List<SimplifiedBill> { debtor.SimplifiedBills.ElementAt(0).Value };
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(bills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            Assert.AreEqual(1235, directDebitTransaction.MandateInternalReferenceNumber);
            directDebitTransaction.GenerateAT01MandateID("777");
            Assert.AreEqual("000077701235                       ", directDebitTransaction.MandateID);
        }
    }
}
