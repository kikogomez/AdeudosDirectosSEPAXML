using System;
using System.Collections.Generic;
using System.Linq;
using DirectDebitElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class PaymentStatusReportUnitTests
    {
        DirectDebitTransactionReject directDebitTransactionReject1;
        DirectDebitTransactionReject directDebitTransactionReject2;
        DirectDebitTransactionReject directDebitTransactionReject3;
        List<DirectDebitTransactionReject> directDebitTransactionRejectsList1;
        List<DirectDebitTransactionReject> directDebitTransactionRejectsList2;

        static string originalPaymentInformationID1;
        static string originalPaymentInformationID2;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            originalPaymentInformationID1 = "PRE201512010001";
            originalPaymentInformationID2 = "PRE201511150001";
        }

        [TestInitialize]
        public void InitializeTransacions()
        {
            // La inicializacion de las transacciones no se hace con variables estáticas
            // pues la suscripcion a eventos interacciona entre los tests

            directDebitTransactionReject1 = new DirectDebitTransactionReject(
                "0123456788",
                "2015120100124",
                DateTime.Parse("2015-12-01"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            directDebitTransactionReject2 = new DirectDebitTransactionReject(
                "0123456789",
                "2015120100312",
                DateTime.Parse("2015-12-01"),
                70,
                "00000110421",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES3011112222003333333333")),
                "MS01");

            directDebitTransactionReject3 = new DirectDebitTransactionReject(
                "0123456790",
                "2015150100124",
                DateTime.Parse("2015-11-15"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            directDebitTransactionRejectsList1 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject1, directDebitTransactionReject2 };

            directDebitTransactionRejectsList2 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject3};
        }

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyCreated()
        {
            string originalTransactionIdentification = "0123456789";
            string originalEndtoEndTransactionIdentification = "2015120100124";
            DateTime requestedCollectionDate = DateTime.Parse("2015-12-01");
            decimal amount = 10;
            string mandateID = "000001102564";
            BankAccount debtorAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890"));
            string rejectReason = "MS02";

            DirectDebitTransactionReject directDebitTransactionReject = new DirectDebitTransactionReject(
                originalTransactionIdentification,
                originalEndtoEndTransactionIdentification,
                requestedCollectionDate,
                amount,
                mandateID,
                debtorAccount,
                rejectReason);

            Assert.AreEqual(originalTransactionIdentification, directDebitTransactionReject.OriginalTransactionIdentification);
            Assert.AreEqual(originalEndtoEndTransactionIdentification, directDebitTransactionReject.OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual(requestedCollectionDate, directDebitTransactionReject.RequestedCollectionDate);
            Assert.AreEqual(amount, directDebitTransactionReject.Amount);
            Assert.AreEqual(mandateID, directDebitTransactionReject.MandateID);
            Assert.AreEqual(debtorAccount, directDebitTransactionReject.DebtorAccount);
            Assert.AreEqual(rejectReason, directDebitTransactionReject.RejectReason);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void OriginalEndToEndTransactionIdentificationOfADirectDebitTransactionRejectCantBeNull()
        {
            try
            {
                DirectDebitTransactionReject DirectDebitTransactionReject = new DirectDebitTransactionReject(
                    null,
                    null,
                    DateTime.Now.AddDays(3),
                    100,
                    "000001112345",
                    new BankAccount(new InternationalAccountBankNumberIBAN("ES7621000000650000000001")),
                    "MS01");
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("originalEndtoEndTransactionIdentification", e.ParamName);
                Assert.AreEqual("OriginalEndtoEndTransactionIdentification can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void OriginalEndToEndTransactionIdentificationOfADirectDebitTransactionRejectCantBeEmpty()
        {
            try
            {
                DirectDebitTransactionReject DirectDebitTransactionReject = new DirectDebitTransactionReject(
                    null,
                    "",
                    DateTime.Now.AddDays(3),
                    100,
                    "000001112345",
                    new BankAccount(new InternationalAccountBankNumberIBAN("ES7621000000650000000001")),
                    "MS01");
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("originalEndtoEndTransactionIdentification", e.ParamName);
                Assert.AreEqual("OriginalEndtoEndTransactionIdentification can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void OriginalEndToEndTransactionIdentificationOfADirectDebitTransactionRejectCantBeOnlySpaces()
        {
            try
            {
                DirectDebitTransactionReject DirectDebitTransactionReject = new DirectDebitTransactionReject(
                    null,
                    "  ",
                    DateTime.Now.AddDays(3),
                    100,
                    "000001112345",
                    new BankAccount(new InternationalAccountBankNumberIBAN("ES7621000000650000000001")),
                    "MS01");
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("originalEndtoEndTransactionIdentification", e.ParamName);
                Assert.AreEqual("OriginalEndtoEndTransactionIdentification can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void OriginalEndToEndTransactionIdentificationOfADirectDebitTransactionRejectCantBeLongerThan35characters()
        {
            try
            {
                DirectDebitTransactionReject DirectDebitTransactionReject = new DirectDebitTransactionReject(
                    null,
                    "1234567890123456789012345678901234567890",
                    DateTime.Now.AddDays(3),
                    100,
                    "000001112345",
                    new BankAccount(new InternationalAccountBankNumberIBAN("ES7621000000650000000001")),
                    "MS01");
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("originalEndtoEndTransactionIdentification", e.ParamName);
                Assert.AreEqual("OriginalEndtoEndTransactionIdentification can't be longer than 35 characters", e.GetMessageWithoutParamName());
                throw;
            }
        }

        //
        //
        // DEBERIAMOS COMPROBAR LA VALIDEZ DE TODOS LOS PARAMETROS DE ENTRADA,
        // PERO NO ES URGENTE YA QUE SE SUPONE QUE LEEMOS UN FICHERO BANCARIO QUE DEBE INCLUIR TODO
        // SOLO REVISAMOS ORIGINALENDTOENDTRANSACTIONIDENTIFICATION, QUE ES LO QUE USAREMOS PRINCIPALMENTE
        //

        [TestMethod]
        public void AnEmptyDirectDebitPaymentInstructionRejectIsCorrectlyCreated()
        {
            string originalPaymentInformationID = "PRE201207010001";

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID);

            Assert.AreEqual(originalPaymentInformationID, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(0, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(0, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual(0, directDebitPaymentInstructionReject.DirectDebitTransactionsRejects.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void CantAssignANullOriginalPaymentInformationIDWhenCreatingAnEmptyDirectDebitPaymentIOnstructionReject()
        {
            string originalPaymentInformationID = null;
            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("originalPaymentInformationID", e.ParamName);
                Assert.AreEqual("OriginalPaymentInformationID can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CantAssignAnEmptyOriginalPaymentInformationIDWhenCreatingAnEmptyDirectDebitPaymentIOnstructionReject()
        {
            string originalPaymentInformationID = " ";
            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("originalPaymentInformationID", e.ParamName);
                Assert.AreEqual("OriginalPaymentInformationID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantAssignAnOriginalPaymentInformationIDLongerThan35CharactersWhenCreatingAnEmptyDirectDebitPaymentIOnstructionReject()
        {
            string originalPaymentInformationID = "0123456789012345678901234567890123456789";
            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID);
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("originalPaymentInformationID", e.ParamName);
                Assert.AreEqual("OriginalPaymentInformationID lenght can't exceed 35 characters", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        public void ADirectDebitPaymentInstructionRejectIsCorrectlyCreatedWithoutProvidingNumberOfTransactionsNorControlSum()
        {

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            int numberOfTransactions = directDebitTransactionRejectsList1.Count;
            decimal controlSum = directDebitTransactionRejectsList1.Select(ddRemittanceReject => ddRemittanceReject.Amount).Sum();
            Assert.AreEqual(originalPaymentInformationID1, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(numberOfTransactions, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void CantAssignANullOriginalPaymentInformationIDWhenCreatingADirectDebitPaymentInstructionReject()
        {
            string originalPaymentInformationID = null;
            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID,
                    directDebitTransactionRejectsList1);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("originalPaymentInformationID", e.ParamName);
                Assert.AreEqual("OriginalPaymentInformationID can't be null", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CantAssignAnEmptyOriginalPaymentInformationIDWhenCreatingADirectDebitPaymentInstructionReject()
        {
            string originalPaymentInformationID = " ";
            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID,
                    directDebitTransactionRejectsList1);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("originalPaymentInformationID", e.ParamName);
                Assert.AreEqual("OriginalPaymentInformationID can't be empty", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantAssignAnOriginalPaymentInformationIDLongerThan35CharactersWhenCreatingADirectDebitPaymentInstructionReject()
        {
            string originalPaymentInformationID = "0123456789012345678901234567890123456789";
            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID,
                    directDebitTransactionRejectsList1);
            }

            catch (System.ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("originalPaymentInformationID", e.ParamName);
                Assert.AreEqual("OriginalPaymentInformationID lenght can't exceed 35 characters", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        public void ADirectDebitPaymentInstructionRejectIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            int numberOfTransactions = directDebitTransactionRejectsList1.Count;
            decimal controlSum = directDebitTransactionRejectsList1.Select(ddRemittanceReject => ddRemittanceReject.Amount).Sum();
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejectsList1);

            Assert.AreEqual(originalPaymentInformationID1, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(numberOfTransactions, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheDirectDebitPaymentInstructionRejectThrowsATypeInitializationErrorException()
        {
            int numberofTransactions = 1;
            decimal controlSum = 150;

            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID1,
                    numberofTransactions,
                    controlSum,
                    directDebitTransactionRejectsList1);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                Assert.AreEqual("DirectDebitPaymentInstructionReject", typeInitializationException.TypeName);

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
        public void IfGivenIncorrectControlSumTheDirectDirectDebitPaymentInstructionRejectThrowsATypeInitializationErrorException()
        {
            int numberofTransactions = 2;
            decimal controlSum = 100;

            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID1,
                    numberofTransactions,
                    controlSum,
                    directDebitTransactionRejectsList1);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                Assert.AreEqual("DirectDebitPaymentInstructionReject", typeInitializationException.TypeName);

                string expectedErrorMessage = "The Control Sum is wrong. It should be 150, but 100 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("controlSum", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyAddedToADirectDebitPaymentInstructionReject()
        {
            string originalPaymentInformationID = "PRE201207010001";
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                directDebitTransactionRejects);

            directDebitPaymentInstructionReject.AddDirectDebitTransactionReject(directDebitTransactionReject1);

            Assert.AreEqual(1, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(80, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual(1, directDebitPaymentInstructionReject.DirectDebitTransactionsRejects.Count);
            Assert.AreEqual("2015120100124", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ICanGetaListofAllTheOriginalEndtoEndTransactionIdentificationInADirectDebitPaymentInstructionReject()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            List<string> expectedOriginalEndtoEndTransactionIdentificationList = new List<string>()
            { "2015120100124", "2015120100312"};

            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList, directDebitPaymentInstructionReject.OriginalEndtoEndTransactiontransactionIDList);
        }

        [TestMethod]
        public void AnEmptyPaymentStatusReportIsCorrectlyCreated()
        {
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime);

            List<DirectDebitPaymentInstruction> emptyDirectDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>();
            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(0, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(0, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(emptyDirectDebitPaymentInstructions, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        //
        // DEBERIAMOS COMPROBAR LA VALIDEZ DE TODOS LOS PARAMETROS DE ENTRADA,
        // PERO NO ES URGENTE YA QUE SE SUPONE QUE LEEMOS UN FICHERO BANCARIO QUE DEBE INCLUIR TODO
        //

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreated()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitPaymentInstructionReject1.NumberOfTransactions + directDebitPaymentInstructionReject2.NumberOfTransactions;
            decimal controlSum = directDebitPaymentInstructionReject1.ControlSum + directDebitPaymentInstructionReject2.ControlSum;

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(numberOfTransactions, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(controlSum, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitPaymentInstructionRejects, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitPaymentInstructionReject1.NumberOfTransactions + directDebitPaymentInstructionReject2.NumberOfTransactions;
            decimal controlSum = directDebitPaymentInstructionReject1.ControlSum + directDebitPaymentInstructionReject2.ControlSum;

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitPaymentInstructionRejects);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(numberOfTransactions, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(controlSum, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitPaymentInstructionRejects, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsThePaymentStatusReportThrowsATypeInitializationException()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 2;
            decimal controlSum = 230;

            try
            {
                PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                    messageID,
                    messageCreationDateTime,
                    rejectAccountChargeDateTime,
                    numberOfTransactions,
                    controlSum,
                    directDebitPaymentInstructionRejects);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                string expectedErrorMessage = "The Number of Transactions is wrong. It should be 3, but 2 is provided";
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
        public void IfGivenIncorrectControlSumThePaymentStatusReportThrowsATypeInitializationException()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 3;
            decimal controlSum = 330;

            try
            {
                PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                    messageID,
                    messageCreationDateTime,
                    rejectAccountChargeDateTime,
                    numberOfTransactions,
                    controlSum,
                    directDebitPaymentInstructionRejects);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                string expectedErrorMessage = "The Control Sum is wrong. It should be 230, but 330 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("controlSum", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }

        [TestMethod]
        public void ICanAddAPaymentInstructionRejectsToAnExistingPaymentStatusReport()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);



            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            paymentStatusReport.AddDirectDebitPaymentInstructionReject(directDebitPaymentInstructionReject);

            List<DirectDebitPaymentInstructionReject> expectedDirectDebitPaymentInstructionRejects =
                new List<DirectDebitPaymentInstructionReject>() { directDebitPaymentInstructionReject };
            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(2, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(150, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(expectedDirectDebitPaymentInstructionRejects, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        public void APaymentStatusReportCanHaveMoreThanOnePaymentInstructionReject()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            paymentStatusReport.AddDirectDebitPaymentInstructionReject(directDebitPaymentInstructionReject2);

            List<DirectDebitPaymentInstructionReject> expectedDirectDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            {directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(expectedDirectDebitPaymentInstructionRejects, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        public void WhenAddingAnotherTransactionRejectToADirectDebitPaymentInstructionRejectInsideAPaymentStatusReportTheAmmountAndNumberOfBillsOfThePaymentStatusReportAreCorrectlyUpdated()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects =
                new List<DirectDebitPaymentInstructionReject>() {directDebitPaymentInstructionReject1};
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);

            directDebitPaymentInstructionReject1.AddDirectDebitTransactionReject(directDebitTransactionReject3);

            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
        }

        [TestMethod]
        public void WhenAddingAnotherTransactionRejectToADirectDebitPaymentInstructionRejectRecentlyAddedToAPaymentStatusReportTheAmmountAndNumberOfBillsOfThePaymentStatusReportAreCorrectlyUpdated()
        {
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime);
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);
            paymentStatusReport.AddDirectDebitPaymentInstructionReject(directDebitPaymentInstructionReject1);

            directDebitPaymentInstructionReject1.AddDirectDebitTransactionReject(directDebitTransactionReject3);

            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
        }
    }
}
