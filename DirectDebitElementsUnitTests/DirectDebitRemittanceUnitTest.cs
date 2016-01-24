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
    public class DirectDebitRemittanceUnitTest
    {
        static Dictionary<string, Debtor> debtors;
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;
        static DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator;
        static DirectDebitTransaction directDebitTransaction1;
        static DirectDebitTransaction directDebitTransaction2;
        static DirectDebitTransaction directDebitTransaction3;
        static string paymentInformationID1;
        static string paymentInformationID2;

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

            directDebitTransaction1 = new DirectDebitTransaction(
                debtors["00001"].SimplifiedBills.Values.ToList(),
                "PaymentInstruction1-00001",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00001"].DirectDebitmandates[1234].InternalReferenceNumber),
                debtors["00001"].DirectDebitmandates[1234].DirectDebitMandateCreationDate,
                debtors["00001"].DirectDebitmandates[1234].BankAccount,
                debtors["00001"].FullName,
                null);

            directDebitTransaction2 = new DirectDebitTransaction(
                debtors["00002"].SimplifiedBills.Values.ToList(),
                "PaymentInstruction1-00002",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00002"].DirectDebitmandates[1235].InternalReferenceNumber),
                debtors["00002"].DirectDebitmandates[1235].DirectDebitMandateCreationDate,
                debtors["00002"].DirectDebitmandates[1235].BankAccount,
                debtors["00002"].FullName,
                null);

            directDebitTransaction3 = new DirectDebitTransaction(
                debtors["00001"].SimplifiedBills.Values.ToList(),
                "PaymentInstruction2-00001",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00001"].DirectDebitmandates[1234].InternalReferenceNumber),
                debtors["00001"].DirectDebitmandates[1234].DirectDebitMandateCreationDate,
                debtors["00001"].DirectDebitmandates[1234].BankAccount,
                debtors["00001"].FullName,
                null);

            paymentInformationID1 = "PRE201512010001";
            paymentInformationID2 = "PRE201511150001";

            //spanishBankCodes = new BankCodes(@"XMLFiles\SpanishBankCodes.xml", BankCodes.BankCodesFileFormat.XML);
        }

        [TestMethod]
        public void ADirectDebitTransactionIsCorrectlyCreated()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();
            DirectDebitAmendmentInformation amendmentinformation = new DirectDebitAmendmentInformation(
                "02121", new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")));

            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                bills,
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                amendmentinformation);

            Assert.AreEqual(transactionID, directDebitTransaction.TransactionID);
            Assert.AreEqual("000077701235", directDebitTransaction.MandateID);
            Assert.AreEqual(mandateSignatureDate, directDebitTransaction.MandateSigatureDate);
            Assert.AreEqual(debtorAccount, directDebitTransaction.DebtorAccount);
            Assert.AreEqual(accountHolderName, directDebitTransaction.AccountHolderName);
            Assert.AreEqual(bills, directDebitTransaction.BillsInTransaction);
            Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(158, directDebitTransaction.Amount);
        }

        [TestMethod]
        public void AmendmentInformationCanBeNullInADirectdebitTransaction()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                bills,
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                null);

            Assert.AreEqual(transactionID, directDebitTransaction.TransactionID);
            Assert.AreEqual("000077701235", directDebitTransaction.MandateID);
            Assert.AreEqual(mandateSignatureDate, directDebitTransaction.MandateSigatureDate);
            Assert.AreEqual(debtorAccount, directDebitTransaction.DebtorAccount);
            Assert.AreEqual(accountHolderName, directDebitTransaction.AccountHolderName);
            Assert.AreEqual(bills, directDebitTransaction.BillsInTransaction);
            Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(158, directDebitTransaction.Amount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void transactionIDOfADirectDebitTransactionCantBeNull()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = null;
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("transactionID", e.ParamName);
                Assert.AreEqual("TransactionID can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void transactionIDOfADirectDebitTransactionCantBeEmpty()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("transactionID", e.ParamName);
                Assert.AreEqual("TransactionID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void transactionIDOfADirectDebitTransactionCantBeOnlySpaces()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "   ";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("transactionID", e.ParamName);
                Assert.AreEqual("TransactionID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void transactionIDOfADirectDebitTransactionCantBeLongerThan35Characters()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "1234567890123456789012345678901234567890";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("transactionID", e.ParamName);
                Assert.AreEqual("transactionID can't be longer than 35 characters", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void MandateIDOfADirectDebitTransactionCantBeNull()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = null;
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("MandateID", e.ParamName);
                Assert.AreEqual("MandateID can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void MandateIDOfADirectDebitTransactionCantBeEmpty()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = "";
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("MandateID", e.ParamName);
                Assert.AreEqual("MandateID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void MandateIDOfADirectDebitTransactionCantBeOnlySpaces()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = " ";
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("MandateID", e.ParamName);
                Assert.AreEqual("MandateID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void MandateIDOfADirectDebitTransactionCantBeLongerThan35Characters()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = "1234567890123456789012345678901234567890";
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("MandateID", e.ParamName);
                Assert.AreEqual("MandateID can't be longer than 35 characters", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void DebtorAccountOfADirectDebitTransactionCantBeNull()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = null;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("DebtorAccount", e.ParamName);
                Assert.AreEqual("DebtorAccount can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void DebtorAccountIBANOfADirectDebitTransactionCantBeinvalid()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = new BankAccount(new BankAccountFields("1111", "1111", "11", "1111111111"));
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();

            try
            {
                DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                    bills,
                    transactionID,
                    mandateID,
                    mandateSignatureDate,
                    debtorAccount,
                    accountHolderName,
                    null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("DebtorAccount", e.ParamName);
                Assert.AreEqual("DebtorAccount must be a valid IBAN", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        public void AnEmptyDirectDebitTransactionIsCorrectlyCreated()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string transactionID = "00001";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;

            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                new List<SimplifiedBill>(),
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                null);

            List<SimplifiedBill> expectedEmptyList = new List<SimplifiedBill>();
            Assert.AreEqual(transactionID, directDebitTransaction.TransactionID);
            Assert.AreEqual("000077701235", directDebitTransaction.MandateID);
            Assert.AreEqual(mandateSignatureDate, directDebitTransaction.MandateSigatureDate);
            Assert.AreEqual(debtorAccount, directDebitTransaction.DebtorAccount);
            Assert.AreEqual(accountHolderName, directDebitTransaction.AccountHolderName);
            CollectionAssert.AreEqual(expectedEmptyList, directDebitTransaction.BillsInTransaction);
            Assert.AreEqual(0, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(0, directDebitTransaction.Amount);
        }

        [TestMethod]
        public void WhenAddingAnotherBillToADirectDebitTransactionTheAmmountAndNumberOfBillsAreCorrectlyUpdated()
        {
            Debtor debtor = debtors["00002"];
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
            string transactionID = "00001";
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            List<SimplifiedBill> bills = new List<SimplifiedBill> { debtor.SimplifiedBills.ElementAt(0).Value };
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                bills,
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                null);
            Assert.AreEqual((decimal)79, directDebitTransaction.Amount);
            Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
            SimplifiedBill bill = debtor.SimplifiedBills.ElementAt(1).Value;

            directDebitTransaction.AddBill(bill);

            Assert.AreEqual((decimal)158, directDebitTransaction.Amount);
            Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
        }

        [TestMethod]
        public void AnEmptyDirectDebitPaymentInstructionIsCorrectlyCreated()
        {
            string localInstrument = "COR1";
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(paymentInformationID1, localInstrument);
            Assert.AreEqual("PRE201512010001", directDebitPaymentInstruction.PaymentInformationID);
            Assert.AreEqual("COR1", directDebitPaymentInstruction.LocalInstrument);
            Assert.AreEqual(0, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(0, directDebitPaymentInstruction.TotalAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantAssignAPaymentInformationIDLongerThan35Characters()
        {
            string paymentInformationID = "0123456789012345678901234567890123456789";
            try
            {
                DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(paymentInformationID, "COR1");
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("PaymentInformationID", e.ParamName);
                Assert.AreEqual("PaymentInformationID lenght can't exceed 35 characters", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CantAssignAnEmptyPaymentInformationID()
        {
            try
            {
                DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction("", "COR1");
            }

            catch (System.ArgumentException e)
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
                DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(null, "COR1");
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("PaymentInformationID", e.ParamName);
                Assert.AreEqual("PaymentInformationID can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        public void ADirectDebitTransactionIsCorrectlyAddedToADirectDebitPaymentInstruction()
        {
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1 };

            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, directDebitTransactions);

            directDebitPaymentInstruction.AddDirectDebitTransaction(directDebitTransaction2);

            Assert.AreEqual(2, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(237, directDebitPaymentInstruction.TotalAmount);
        }

        [TestMethod]
        public void ADirectDebitPaymentInstructionIsCorrectlyCreatedWithoutProvidingNumberOfTransactionsNorControlSum()
        {
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };

            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, directDebitTransactions);

            Assert.AreEqual("PRE201512010001", directDebitPaymentInstruction.PaymentInformationID);
            Assert.AreEqual("COR1", directDebitPaymentInstruction.LocalInstrument);
            Assert.AreEqual(2, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(237, directDebitPaymentInstruction.TotalAmount);
        }

        [TestMethod]
        public void IfGivenCorrectNumberOfTransactionsAndControlSumTheDirectDebitPaymentInstructionIsCorrectlyCreated()
        {
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            int numberOfTransactions = directDebitTransactions.Count;
            decimal controlSum = directDebitTransactions.Select(directDebitTransaction => directDebitTransaction.Amount).Sum();

            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument,directDebitTransactions, numberOfTransactions, controlSum);

            Assert.AreEqual("PRE201512010001", directDebitPaymentInstruction.PaymentInformationID);
            Assert.AreEqual("COR1", directDebitPaymentInstruction.LocalInstrument);
            Assert.AreEqual(2, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(237, directDebitPaymentInstruction.TotalAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheDirectDebitPaymentInstructionThrowsATypeInitializationErrorException()
        {
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            int numberOfTransactions = 1;
            decimal controlSum = directDebitTransactions.Select(directDebitTransaction => directDebitTransaction.Amount).Sum();

            try
            {
                DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                    paymentInformationID1, localInstrument, directDebitTransactions, numberOfTransactions, controlSum);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                Assert.AreEqual("DirectDebitPaymentInstruction", typeInitializationException.TypeName);

                string expectedErrorMessage = "The Number of Transactions is wrong. It should be 2, but 1 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("numberOfTransactions", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectControlSumTheDirectDebitPaymentInstructionThrowsATypeInitializationErrorException()
        {
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            int numberOfTransactions = directDebitTransactions.Count;
            decimal controlSum = 0;

            try
            {
                DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                    paymentInformationID1, localInstrument, directDebitTransactions, numberOfTransactions, controlSum);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                Assert.AreEqual("DirectDebitPaymentInstruction", typeInitializationException.TypeName);

                string expectedErrorMessage = "The Control Sum is wrong. It should be 237, but 0 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("controlSum", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }

        [TestMethod]
        public void AnEmptyDirectDebitRemittanceInstanceIsCorrectlyCreated()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);

            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);

            string expectedMessageId = "ES26777G123456782013113007:15:00";
            Assert.AreEqual(expectedMessageId, directDebitRemittance.MessageID);
            Assert.AreEqual(creationDate, directDebitRemittance.CreationDate);
            Assert.AreEqual(requestedCollectionDate, directDebitRemittance.RequestedCollectionDate);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void MessageIDCantBeNullInADirectDebitRemittance()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = null;
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);

            try
            {
                DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                    messageID,
                    creationDate,
                    requestedCollectionDate,
                    directDebitInitiationContract);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("MessageID", e.ParamName);
                Assert.AreEqual("MessageID can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void MessageIDCantBeEmptyInADirectDebitRemittance()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "";
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);

            try
            {
                DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                    messageID,
                    creationDate,
                    requestedCollectionDate,
                    directDebitInitiationContract);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("MessageID", e.ParamName);
                Assert.AreEqual("MessageID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void MessageIDCantBeLongerThan35CharactersInADirectDebitRemittance()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "1234567890123456789012345678901234567890";
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);

            try
            {
                DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                    messageID,
                    creationDate,
                    requestedCollectionDate,
                    directDebitInitiationContract);
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("MessageID", e.ParamName);
                Assert.AreEqual("MessageID can't be longer than 35 characters", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void DirectDebitInitiationContractCantBeNullInADirectDebitRemittance()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "ES26777G123456782013113007:15:00";
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);

            try
            {
                DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                    messageID,
                    creationDate,
                    requestedCollectionDate,
                    null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("DirectDebitInitiationContract", e.ParamName);
                Assert.AreEqual("DirectDebitInitiationContract can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        public void AnDirectDebitRemittanceIsCorrectlyCreatedWithoutProvidingNumberOfTransactionsNorControlSum()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, directDebitTransactions);
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>() { directDebitPaymentInstruction };

            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                messageID,
                creationDate,
                requestedCollectionDate,
                directDebitInitiationContract,
                directDebitPaymentInstructions);

            string expectedMessageId = "ES26777G123456782013113007:15:00";
            Assert.AreEqual(expectedMessageId, directDebitRemittance.MessageID);
            Assert.AreEqual(creationDate, directDebitRemittance.CreationDate);
            Assert.AreEqual(requestedCollectionDate, directDebitRemittance.RequestedCollectionDate);
            Assert.AreEqual(1, directDebitRemittance.DirectDebitPaymentInstructions.Count);
            Assert.AreEqual(2, directDebitRemittance.DirectDebitPaymentInstructions[0].NumberOfDirectDebitTransactions);
            Assert.AreEqual(237, directDebitRemittance.DirectDebitPaymentInstructions[0].TotalAmount);
        }

        [TestMethod]
        public void IfGivenCorrectNumberOfTransactionsAndControlSumTheDirectDebitRemittanceIsCorrectlyCreated()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, directDebitTransactions);
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>() { directDebitPaymentInstruction };
            int numberOfTransactions = directDebitPaymentInstructions.Select(paymentInstruction => paymentInstruction.NumberOfDirectDebitTransactions).Sum();
            decimal controlSum = directDebitPaymentInstructions.Select(paymentInstruction => paymentInstruction.TotalAmount).Sum();

            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                messageID,
                creationDate,
                requestedCollectionDate,
                directDebitInitiationContract,
                numberOfTransactions,
                controlSum,
                directDebitPaymentInstructions);

            Assert.AreEqual("PRE201512010001", directDebitPaymentInstruction.PaymentInformationID);
            Assert.AreEqual("COR1", directDebitPaymentInstruction.LocalInstrument);
            Assert.AreEqual(2, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(237, directDebitPaymentInstruction.TotalAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheDirectDebitRemittanceThrowsATypeInitializationErrorException()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, directDebitTransactions);
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>() { directDebitPaymentInstruction };
            int numberOfTransactions = 1;
            decimal controlSum = directDebitPaymentInstructions.Select(paymentInstruction => paymentInstruction.TotalAmount).Sum();

            try
            {
                DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                    messageID,
                    creationDate,
                    requestedCollectionDate,
                    directDebitInitiationContract,
                    numberOfTransactions,
                    controlSum,
                    directDebitPaymentInstructions);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                Assert.AreEqual("DirectDebitRemittance", typeInitializationException.TypeName);

                string expectedErrorMessage = "The Number of Transactions is wrong. It should be 2, but 1 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("numberOfTransactions", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectControlSumTheDirectDebitRemittanceThrowsATypeInitializationErrorException()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, directDebitTransactions);
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>() { directDebitPaymentInstruction };
            int numberOfTransactions = directDebitPaymentInstructions.Select(paymentInstruction => paymentInstruction.NumberOfDirectDebitTransactions).Sum();
            decimal controlSum = 100;

            try
            {
                DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                    messageID,
                    creationDate,
                    requestedCollectionDate,
                    directDebitInitiationContract,
                    numberOfTransactions,
                    controlSum,
                    directDebitPaymentInstructions);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                Assert.AreEqual("DirectDebitRemittance", typeInitializationException.TypeName);

                string expectedErrorMessage = "The Control Sum is wrong. It should be 237, but 100 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("controlSum", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }
        [TestMethod]
        public void ADirectDebitPaymentInstructionIsCorrectlyAddedToADirectDebitRemittance()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);
            string localInstrument = "COR1";
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, directDebitTransactions);

            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction);

            Assert.AreEqual(1, directDebitRemittance.DirectDebitPaymentInstructions.Count);
            Assert.AreEqual(2, directDebitRemittance.DirectDebitPaymentInstructions[0].NumberOfDirectDebitTransactions);
            Assert.AreEqual(237, directDebitRemittance.DirectDebitPaymentInstructions[0].TotalAmount);
        }

        [TestMethod]
        public void ADirectDebitRemittanceCanHaveMoreThanOnePaymentInstruction()
        {
            DateTime creationDate = new DateTime(2013, 11, 30, 7, 15, 0);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 12, 1);
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);

            string localInstrument1 = "COR1";
            List<DirectDebitTransaction> directDebitTransactions1 = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };
            DirectDebitPaymentInstruction directDebitPaymentInstruction1 = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument1, directDebitTransactions1);
            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction1);
            string localInstrument2 = "COR1";
            List<DirectDebitTransaction> directDebitTransactions2 = new List<DirectDebitTransaction>() { directDebitTransaction3 };
            DirectDebitPaymentInstruction directDebitPaymentInstruction2 = new DirectDebitPaymentInstruction(
                paymentInformationID2, localInstrument2, directDebitTransactions2);

            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction2);

            Assert.AreEqual(2, directDebitRemittance.DirectDebitPaymentInstructions.Count);
            Assert.AreEqual(3, directDebitRemittance.NumberOfTransactions);
            Assert.AreEqual(316, directDebitRemittance.ControlSum);
            Assert.AreEqual(2, directDebitRemittance.DirectDebitPaymentInstructions[0].NumberOfDirectDebitTransactions);
            Assert.AreEqual(237, directDebitRemittance.DirectDebitPaymentInstructions[0].TotalAmount);
            Assert.AreEqual(1, directDebitRemittance.DirectDebitPaymentInstructions[1].NumberOfDirectDebitTransactions);
            Assert.AreEqual(79, directDebitRemittance.DirectDebitPaymentInstructions[1].TotalAmount);
        }


        //[TestMethod]
        //public void WhenAddingAnotherBillToADirectDebitTransactionInsideAPaymentInstructionTheAmmountAndNumberOfBillsOfThePaymentInstructionAreCorrectlyUpdated()
        //{
        //    Debtor debtor = debtors["00002"];
        //    DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
        //    int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
        //    string transactionID = "00001";
        //    string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
        //    DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
        //    BankAccount debtorAccount = directDebitMandate.BankAccount;
        //    string accountHolderName = directDebitMandate.AccountHolderName;
        //    List<SimplifiedBill> bills = new List<SimplifiedBill> { debtor.SimplifiedBills.ElementAt(0).Value };
        //    DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
        //        bills,
        //        transactionID,
        //        mandateID,
        //        mandateSignatureDate,
        //        debtorAccount,
        //        accountHolderName,
        //        null);
        //    Assert.AreEqual((decimal)79, directDebitTransaction.Amount);
        //    Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
        //    SimplifiedBill bill = debtor.SimplifiedBills.ElementAt(1).Value;

        //    directDebitTransaction.AddBill(bill);

        //    Assert.AreEqual((decimal)158, directDebitTransaction.Amount);
        //    Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
        //}

        //[TestMethod]
        //public void WhenAddingAnotherBillToADirectDebitTransactionInsideARemmitanceTheAmmountAndNumberOfBillsOfTheRemmitanceAreCorrectlyUpdated()
        //{
        //    Debtor debtor = debtors["00002"];
        //    DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
        //    int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
        //    string transactionID = "00001";
        //    string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
        //    DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
        //    BankAccount debtorAccount = directDebitMandate.BankAccount;
        //    string accountHolderName = directDebitMandate.AccountHolderName;
        //    List<SimplifiedBill> bills = new List<SimplifiedBill> { debtor.SimplifiedBills.ElementAt(0).Value };
        //    DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
        //        bills,
        //        transactionID,
        //        mandateID,
        //        mandateSignatureDate,
        //        debtorAccount,
        //        accountHolderName,
        //        null);
        //    Assert.AreEqual((decimal)79, directDebitTransaction.Amount);
        //    Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
        //    SimplifiedBill bill = debtor.SimplifiedBills.ElementAt(1).Value;

        //    directDebitTransaction.AddBill(bill);

        //    Assert.AreEqual((decimal)158, directDebitTransaction.Amount);
        //    Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
        //}

        //[TestMethod]
        //public void WhenAddingAnotherTransactionToADirectDebitPaymentInstructionInsideARemmitanceTheAmmountAndNumberOfBillsOfTheRemmitanceAreCorrectlyUpdated()
        //{
        //    Debtor debtor = debtors["00002"];
        //    DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
        //    int internalDirectDebitReferenceNumber = directDebitMandate.InternalReferenceNumber;
        //    string transactionID = "00001";
        //    string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
        //    DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
        //    BankAccount debtorAccount = directDebitMandate.BankAccount;
        //    string accountHolderName = directDebitMandate.AccountHolderName;
        //    List<SimplifiedBill> bills = new List<SimplifiedBill> { debtor.SimplifiedBills.ElementAt(0).Value };
        //    DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
        //        bills,
        //        transactionID,
        //        mandateID,
        //        mandateSignatureDate,
        //        debtorAccount,
        //        accountHolderName,
        //        null);
        //    Assert.AreEqual((decimal)79, directDebitTransaction.Amount);
        //    Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
        //    SimplifiedBill bill = debtor.SimplifiedBills.ElementAt(1).Value;

        //    directDebitTransaction.AddBill(bill);

        //    Assert.AreEqual((decimal)158, directDebitTransaction.Amount);
        //    Assert.AreEqual(2, directDebitTransaction.NumberOfBills);
        //}
    }
}
