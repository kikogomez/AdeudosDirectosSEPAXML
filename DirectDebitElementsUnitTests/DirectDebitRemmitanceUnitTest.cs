using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;
using Billing;
using ReferencesAndTools;
//using RCNGCMembersManagementMocks;
//using RCNGCMembersManagementAppLogic;
//using RCNGCMembersManagementAppLogic.Billing;
//using RCNGCMembersManagementAppLogic.Billing.DirectDebit;
//using RCNGCMembersManagementAppLogic.MembersManaging;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class DirectDebitRemmitanceUnitTest
    {
        static Dictionary<string, ClubMember_POCOTestClass> clubMembers;
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;
        static BankCodes spanishBankCodes;
        //static InvoicesManager invoicesManager;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            //BillingSequenceNumbersMock invoiceDataManagerMock = new BillingSequenceNumbersMock();
            //BillingDataManager.Instance.SetBillingSequenceNumberCollaborator(invoiceDataManagerMock);
            //BillingDataManager.Instance.InvoiceSequenceNumber=5000;
            //invoicesManager = new InvoicesManager();

            clubMembers = new Dictionary<string, ClubMember_POCOTestClass>()
            {
                {"00001", new ClubMember_POCOTestClass("00001", "Francisco", "Gómez-Caldito", "Viseas")},
                {"00002", new ClubMember_POCOTestClass("00002", "Pedro", "Pérez", "Gómez")}
            };

            creditor = new Creditor("G35008770", "Real Club Náutico de Gran Canaria");
            creditorAgent = new CreditorAgent(new BankCode("2100", "CaixaBank","CAIXESBBXXX"));
            directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")),
                creditor.NIF,
                "777",
                creditorAgent);

            var directDebitmandateslist = new[]
            {
                new {clubMemberID = "00001", internalReference = 1234, ccc = "21002222002222222222" },
                new {clubMemberID = "00002", internalReference = 1235, ccc = "21003333802222222222" }
            };

            foreach (var ddM in directDebitmandateslist)
            {
                DirectDebitMandate directDebitMandate = new DirectDebitMandate(
                    ddM.internalReference,
                    new DateTime(2013,11,11),
                    new BankAccount(new ClientAccountCodeCCC(ddM.ccc)),
                    clubMembers[ddM.clubMemberID].FullName);
                clubMembers[ddM.clubMemberID].AddDirectDebitMandate(directDebitMandate);
            }

            var billsList = new[]
            {
                new {clubMemberID = "00001", billID= "00001/01", Amount = 79, transactionDescription = "Cuota Social Octubre 2013" },
                new {clubMemberID = "00002", billID= "00002/01",Amount = 79, transactionDescription="Cuota Social Octubre 2013" },
                new {clubMemberID = "00002", billID= "00002/02",Amount = 79, transactionDescription="Cuota Social Noviembre 2013"}
            };

            foreach (var bill in billsList)
            {
                //List<Transaction> transaction = new List<Transaction>()
                //{
                //    new Transaction(bill.transactionDescription,1,bill.Amount,new Tax("NoTAX",0),0)
                //};
                ClubMember_POCOTestClass clubMember = clubMembers[bill.clubMemberID];
                SimplifiedBill simplifiedBill = new SimplifiedBill(
                    bill.transactionDescription,
                    bill.Amount,
                    DateTime.Today,
                    DateTime.Today.AddMonths(1));
                clubMember.AddSimplifiedBill(simplifiedBill);
                //InvoiceCustomerData invoiceCustomerData = new InvoiceCustomerData(clubMember);
                //Invoice invoice = new Invoice(invoiceCustomerData, transaction, new DateTime(2013, 11, 11));
                //invoicesManager.AddInvoiceToClubMember(invoice, clubMember);
            }

            spanishBankCodes = new BankCodes(@"XMLFiles\SpanishBankCodes.xml", BankCodes.BankCodesFileFormat.XML);
        }

        [TestMethod]
        public void ADirectDebittRemmitanceInstanceIsCorrectlyCreated()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            DirectDebitRemittance directDebitRemmitance = new DirectDebitRemittance(creationDate, requestedCollectionDate, directDebitInitiationContract);
            string expectedMessageId = "ES90777G350087702013113007:15:00";
            Assert.AreEqual(expectedMessageId, directDebitRemmitance.MessageID);
            Assert.AreEqual(creationDate, directDebitRemmitance.CreationDate);
            Assert.AreEqual(requestedCollectionDate, directDebitRemmitance.RequestedCollectionDate);
        }

        [TestMethod]
        public void AnEmptyDirectDebitTransactionIsCorrectlyCreated()
        {
            ClubMember_POCOTestClass clubMember = clubMembers["00002"];
            DirectDebitMandate directDebitMandate = clubMembers["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            Assert.AreEqual(internalDirectDebitReferenceNumber, directDebitTransaction.InternalDirectDebitReferenceNumber);
            Assert.AreEqual(debtorAccount, directDebitTransaction.DebtorAccount);
            Assert.AreEqual(0, directDebitTransaction.NumberOfBills);
        }

        [TestMethod]
        public void ADirectDebitTransactionIsCorrectlyCreated()
        {
            ClubMember_POCOTestClass clubMember = clubMembers["00002"];
            List<SimplifiedBill> simplifiedBills = clubMember.SimplifiedBills.Values.ToList();
            DirectDebitMandate directDebitMandate = clubMembers["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(simplifiedBills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            Assert.AreEqual(simplifiedBills, directDebitTransaction.BillsInTransaction);
            Assert.AreEqual(internalDirectDebitReferenceNumber, directDebitTransaction.InternalDirectDebitReferenceNumber);
            Assert.AreEqual(debtorAccount, directDebitTransaction.DebtorAccount);
            Assert.AreEqual((decimal)158, directDebitTransaction.Amount);
            Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
        }

        [TestMethod]
        public void WhenAddingAnotherBillToADirectDebitTransactionTheAmmountAndNumberOfBillsAreCorrectlyUpdated()
        {
            ClubMember_POCOTestClass clubMember = clubMembers["00002"];
            //Invoice firstInvoice = clubMember.InvoicesList.Values.ElementAt(0);
            List<Bill> bills = new List<Bill>() { firstInvoice.Bills.Values.ElementAt(0) };
            DirectDebitMandate directDebitMandate = clubMembers["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(bills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            Assert.AreEqual((decimal)79, directDebitTransaction.Amount);
            Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
            Invoice secondInvoice = clubMember.InvoicesList.Values.ElementAt(1);
            Bill bill = secondInvoice.Bills.ElementAt(0).Value;
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

            ClubMember clubMember = clubMembers["00002"];
            Invoice firstInvoice = clubMember.InvoicesList.Values.ElementAt(0);
            List<Bill> bills = new List<Bill>() { firstInvoice.Bills.Values.ElementAt(0) };
            DirectDebitMandate directDebitMandate = clubMembers["00002"].DirectDebitmandates.ElementAt(0).Value;
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
        public void TheDirectDebitTransactionGruopPaymentIdIsWellGenerated() //Actualizar los ID al generar el mensaje 
        {
            int sequenceNumber = 1;
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment("COR1");
            directDebitTransactionsGroupPayment.GeneratePaymentInformationID(sequenceNumber);
            Assert.AreEqual("001", directDebitTransactionsGroupPayment.PaymentInformationID);
        }

        [TestMethod]
        public void TheInstructionIDOfADirectDebitTransactionIsWellGenerated() 
        {
            int sequenceNumber = 1;
            ClubMember clubMember = clubMembers["00002"];
            Invoice firstInvoice = clubMember.InvoicesList.Values.ElementAt(0);
            List<Bill> bills = new List<Bill>() { firstInvoice.Bills.Values.ElementAt(0) };
            DirectDebitMandate directDebitMandate = clubMembers["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(bills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            directDebitTransaction.GenerateDirectDebitTransactionInternalReference(sequenceNumber);
            Assert.AreEqual("000001", directDebitTransaction.DirectDebitTransactionInternalReference);
        }

        [TestMethod]
        public void TheMandateIDOfADirectDebitIsWellGenerated() 
        {
            ClubMember clubMember = clubMembers["00002"];
            Invoice firstInvoice = clubMember.InvoicesList.Values.ElementAt(0);
            List<Bill> bills = new List<Bill>() { firstInvoice.Bills.Values.ElementAt(0) };
            DirectDebitMandate directDebitMandate = clubMembers["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName; 
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(bills, internalDirectDebitReferenceNumber, debtorAccount, accountHolderName, mandateSignatureDate);
            Assert.AreEqual(1235, directDebitTransaction.InternalDirectDebitReferenceNumber);
            directDebitTransaction.GenerateMandateID("777");
            Assert.AreEqual("000077701235                       ", directDebitTransaction.MandateID);
        }
    }
}
