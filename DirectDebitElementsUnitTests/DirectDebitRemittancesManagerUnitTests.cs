using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;
using DirectDebitElements;
using ReferencesAndTools;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class DirectDebitRemittancesManagerUnitTests
    {
        static Dictionary<string, Debtor> debtors;
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;
        static DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator;
        static DirectDebitTransaction directDebitTransaction1;
        static DirectDebitTransaction directDebitTransaction2;
        static DirectDebitTransaction directDebitTransaction3;
        static DirectDebitAmendmentInformation amendmentInformation1;
        static string paymentInformationID1;
        static string paymentInformationID2;


        //static BankCodes spanishBankCodes;

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

            amendmentInformation1 = new DirectDebitAmendmentInformation(
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(1000),
                new BankAccount(new InternationalAccountBankNumberIBAN("ES7621000000650000000001")));

            paymentInformationID1 = "PRE201512010001";
            paymentInformationID2 = "PRE201511150001";
        }

        [TestMethod]
        public void AnEmptyDirectDebitTransactionInCorrectlyGenerated()
        {
            List<SimplifiedBill> emptyBillsList = new List<SimplifiedBill>();
            string internalUniqueInstructionID = "PaymentInstruction1-00001";
            DirectDebitMandate directDebitMandate = debtors["00001"].DirectDebitmandates[1234];
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);

            DirectDebitRemittancesManager directDebitRemittancesManager = new DirectDebitRemittancesManager();
            DirectDebitTransaction emptyDirectDebitTransaction = directDebitRemittancesManager.CreateAnEmptyDirectDebitTransaction(
                internalUniqueInstructionID,
                mandateID,
                directDebitMandate,
                amendmentInformation1);

            Assert.AreEqual(internalUniqueInstructionID, emptyDirectDebitTransaction.InternalUniqueInstructionID);
            Assert.AreEqual(mandateID, emptyDirectDebitTransaction.MandateID);
            Assert.AreEqual(directDebitMandate.DirectDebitMandateCreationDate, emptyDirectDebitTransaction.MandateSigatureDate);
            Assert.AreEqual(directDebitMandate.BankAccount, emptyDirectDebitTransaction.DebtorAccount);
            Assert.AreEqual(directDebitMandate.AccountHolderName, emptyDirectDebitTransaction.AccountHolderName);
            Assert.AreEqual(0, emptyDirectDebitTransaction.NumberOfBills);
            Assert.AreEqual(0, emptyDirectDebitTransaction.Amount);
            CollectionAssert.AreEqual(emptyBillsList, emptyDirectDebitTransaction.BillsInTransaction);
            Assert.AreEqual(amendmentInformation1, emptyDirectDebitTransaction.AmendmentInformation);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void InternalUniqueInstructionIDCantBeNullWhenCreatingAnEmptyDirectDebitTransaction()
        {
            List<SimplifiedBill> emptyBillsList = new List<SimplifiedBill>();
            string internalUniqueInstructionID = null;
            DirectDebitMandate directDebitMandate = debtors["00001"].DirectDebitmandates[1234];
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);

            DirectDebitRemittancesManager directDebitRemittancesManager = new DirectDebitRemittancesManager();
            try
            {
                DirectDebitTransaction emptyDirectDebitTransaction = directDebitRemittancesManager.CreateAnEmptyDirectDebitTransaction(
                internalUniqueInstructionID,
                mandateID,
                directDebitMandate,
                null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("InternalUniqueInstructionID", e.ParamName);
                Assert.AreEqual("InternalUniqueInstructionID can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void InternalUniqueInstructionIDCantBeEmptyWhenCreatingAnEmptyDirectDebitTransaction()
        {
            List<SimplifiedBill> emptyBillsList = new List<SimplifiedBill>();
            string internalUniqueInstructionID = "";
            DirectDebitMandate directDebitMandate = debtors["00001"].DirectDebitmandates[1234];
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);

            DirectDebitRemittancesManager directDebitRemittancesManager = new DirectDebitRemittancesManager();
            try
            {
                DirectDebitTransaction emptyDirectDebitTransaction = directDebitRemittancesManager.CreateAnEmptyDirectDebitTransaction(
                internalUniqueInstructionID,
                mandateID,
                directDebitMandate,
                null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("InternalUniqueInstructionID", e.ParamName);
                Assert.AreEqual("InternalUniqueInstructionID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void MandateIDCantBeNullWhenCreatingAnEmptyDirectDebitTransaction()
        {
            List<SimplifiedBill> emptyBillsList = new List<SimplifiedBill>();
            string internalUniqueInstructionID = "PaymentInstruction1-00001";
            DirectDebitMandate directDebitMandate = debtors["00001"].DirectDebitmandates[1234];
            string mandateID = null;

            DirectDebitRemittancesManager directDebitRemittancesManager = new DirectDebitRemittancesManager();
            try
            {
                DirectDebitTransaction emptyDirectDebitTransaction = directDebitRemittancesManager.CreateAnEmptyDirectDebitTransaction(
                internalUniqueInstructionID,
                mandateID,
                directDebitMandate,
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
        public void MandateIDCantBeEmptyWhenCreatingAnEmptyDirectDebitTransaction()
        {
            List<SimplifiedBill> emptyBillsList = new List<SimplifiedBill>();
            string internalUniqueInstructionID = "PaymentInstruction1-00001";
            DirectDebitMandate directDebitMandate = debtors["00001"].DirectDebitmandates[1234];
            string mandateID = " ";

            DirectDebitRemittancesManager directDebitRemittancesManager = new DirectDebitRemittancesManager();
            try
            {
                DirectDebitTransaction emptyDirectDebitTransaction = directDebitRemittancesManager.CreateAnEmptyDirectDebitTransaction(
                internalUniqueInstructionID,
                mandateID,
                directDebitMandate,
                null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("MandateID", e.ParamName);
                Assert.AreEqual("MandateID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void DirectDebitMandateCantBeNullWhenCreatingAnEmptyDirectDebitTransaction()
        {
            List<SimplifiedBill> emptyBillsList = new List<SimplifiedBill>();
            string internalUniqueInstructionID = "PaymentInstruction1-00001";
            DirectDebitMandate directDebitMandate = null;
            string mandateID = "000007701234";

            DirectDebitRemittancesManager directDebitRemittancesManager = new DirectDebitRemittancesManager();
            try
            {
                DirectDebitTransaction emptyDirectDebitTransaction = directDebitRemittancesManager.CreateAnEmptyDirectDebitTransaction(
                internalUniqueInstructionID,
                mandateID,
                directDebitMandate,
                null);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("directDebitMandate", e.ParamName);
                Assert.AreEqual("DirectDebitMandate can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        //[TestMethod]
        //public void ADirectDebitTransactionInCorrectlyGeneratedFromAGroupOfBills()
        //{
        //    List<SimplifiedBill> emptyBillsList = new List<SimplifiedBill>();
        //    string internalUniqueInstructionID = "PaymentInstruction1-00001";
        //    DirectDebitMandate directDebitMandate = debtors["00001"].DirectDebitmandates[1234];
        //    string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);

        //    DirectDebitRemittancesManager directDebitRemittancesManager = new DirectDebitRemittancesManager();
        //    DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateADirectDebitTransactionFromAGroupOfBills(
        //        internalUniqueInstructionID,
        //        mandateID,
        //        directDebitmandate,
        //        billsList,
        //        amendmentInformation1);

        //    Assert.AreEqual(internalUniqueInstructionID, directDebitTransaction.InternalUniqueInstructionID);
        //    Assert.AreEqual(mandateID, directDebitTransaction.MandateID);
        //    Assert.AreEqual(directDebitMandate.DirectDebitMandateCreationDate, directDebitTransaction.MandateSigatureDate);
        //    Assert.AreEqual(directDebitMandate.BankAccount, directDebitTransaction.DebtorAccount);
        //    Assert.AreEqual(directDebitMandate.AccountHolderName, directDebitTransaction.AccountHolderName);
        //    Assert.AreEqual(0, directDebitTransaction.NumberOfBills);
        //    Assert.AreEqual(0, directDebitTransaction.Amount);
        //    CollectionAssert.AreEqual(emptyBillsList, directDebitTransaction.BillsInTransaction);
        //}
    }
}
