using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;
using DirectDebitElements;
using ReferencesAndTools;
using ISO20022PaymentInitiations.SchemaSerializableClasses;
using ISO20022PaymentInitiations.SchemaSerializableClasses.DDInitiation;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class SEPAElementsGeneratorUnitTests
    {

        static Dictionary<string, Debtor> debtors;
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;
        static DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator;
        DirectDebitTransaction directDebitTransaction1;
        DirectDebitTransaction directDebitTransaction2;
        DirectDebitTransaction directDebitTransaction3;
        DirectDebitTransaction directDebitTransaction4;
        static DirectDebitAmendmentInformation amendmentInformation1;
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

            paymentInformationID1 = "PRE201512010001";
            paymentInformationID2 = "PRE201511150001";

            amendmentInformation1 = new DirectDebitAmendmentInformation(
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(1000),
                new BankAccount(new InternationalAccountBankNumberIBAN("ES7621000000650000000001")));
        }

        [TestInitialize]
        public void InitializeTransacions()
        {
            // La inicializacion de las transacciones no se hace con variables estáticas
            // pues la suscripcion a eventos interacciona entre los tests

            directDebitTransaction1 = new DirectDebitTransaction(
                debtors["00001"].SimplifiedBills.Values.ToList(),
                "PaymentInstruction1-00001",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00001"].DirectDebitmandates[1234].InternalReferenceNumber),
                debtors["00001"].DirectDebitmandates[1234].DirectDebitMandateCreationDate,
                debtors["00001"].DirectDebitmandates[1234].BankAccount,
                debtors["00001"].FullName,
                null,
                false);

            directDebitTransaction2 = new DirectDebitTransaction(
                debtors["00002"].SimplifiedBills.Values.ToList(),
                "PaymentInstruction1-00002",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00002"].DirectDebitmandates[1235].InternalReferenceNumber),
                debtors["00002"].DirectDebitmandates[1235].DirectDebitMandateCreationDate,
                debtors["00002"].DirectDebitmandates[1235].BankAccount,
                debtors["00002"].FullName,
                null,
                false);

            directDebitTransaction3 = new DirectDebitTransaction(
                debtors["00001"].SimplifiedBills.Values.ToList(),
                "PaymentInstruction2-00001",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00001"].DirectDebitmandates[1234].InternalReferenceNumber),
                debtors["00001"].DirectDebitmandates[1234].DirectDebitMandateCreationDate,
                debtors["00001"].DirectDebitmandates[1234].BankAccount,
                debtors["00001"].FullName,
                null,
                false);

            directDebitTransaction4 = new DirectDebitTransaction(
                debtors["00001"].SimplifiedBills.Values.ToList(),
                "PaymentInstruction3-00001",
                directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(debtors["00001"].DirectDebitmandates[1234].InternalReferenceNumber),
                debtors["00001"].DirectDebitmandates[1234].DirectDebitMandateCreationDate,
                debtors["00001"].DirectDebitmandates[1234].BankAccount,
                debtors["00001"].FullName,
                null,
                true);
        }

        [TestMethod]
        public void APartyIdentification32IsCorrectlyGenerated()
        {
            PartyIdentification32 initiationParty_InitPty = SEPAElementsGenerator.GenerateInitiationParty_InitPty(
                creditor,
                directDebitInitiationContract);

            Assert.AreEqual("NOMBRE ACREEDOR PRUEBAS",initiationParty_InitPty.Nm);
            Assert.AreEqual("ES26777G12345678", ((OrganisationIdentification4)initiationParty_InitPty.Id.Item).Othr[0].Id);

            Assert.IsNull(initiationParty_InitPty.CtctDtls);
            Assert.IsNull(initiationParty_InitPty.CtryOfRes);
            Assert.IsNull(initiationParty_InitPty.PstlAdr);
        }

        [TestMethod]
        public void AGroupHeader39IsCorrectlyGenerated()
        {
            string messageID = "ES26011G123456782015011007:15:00";
            DateTime generationDateTime = DateTime.Now;
            int numberOfTransactions = 2;
            decimal controlSum = 158;

            PartyIdentification32 initiationParty_InitPty = SEPAElementsGenerator.GenerateInitiationParty_InitPty(
                creditor,
                directDebitInitiationContract);

            GroupHeader39 groupHeader_GrpHdr = SEPAElementsGenerator.GenerateGroupHeader_GrpHdr(
                messageID,
                generationDateTime,
                numberOfTransactions,
                controlSum,
                initiationParty_InitPty);

            DateTime truncatedToSecondsGenerationDateTime = DateTime.SpecifyKind(generationDateTime, DateTimeKind.Unspecified).Truncate(TimeSpan.FromSeconds(1));
            Assert.AreEqual("ES26011G123456782015011007:15:00", groupHeader_GrpHdr.MsgId);
            Assert.AreEqual(truncatedToSecondsGenerationDateTime, groupHeader_GrpHdr.CreDtTm);
            Assert.IsTrue(groupHeader_GrpHdr.CtrlSumSpecified);
            Assert.AreEqual(numberOfTransactions.ToString(),groupHeader_GrpHdr.NbOfTxs);
            Assert.AreEqual(controlSum, groupHeader_GrpHdr.CtrlSum);
            Assert.AreEqual(initiationParty_InitPty, groupHeader_GrpHdr.InitgPty);

            CollectionAssert.AreEqual(new Authorisation1Choice[] { null }, groupHeader_GrpHdr.Authstn);
            Assert.IsNull(groupHeader_GrpHdr.FwdgAgt);
        }

        [TestMethod]
        public void ADirectDebitTransactionInformation9WithoutAmmendmentInformationIsCorrectlyGenerated()
        {
            string transactionID = "00001";
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                bills,
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                null,
                false);
            bool singleUnstructuredConcept = false;

            DirectDebitTransactionInformation9 directDebitTransactionInformation_DrctDbtTxInf = SEPAElementsGenerator.GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                creditorAgent,
                directDebitTransaction,
                singleUnstructuredConcept);

            Assert.AreEqual(directDebitTransaction.TransactionID, directDebitTransactionInformation_DrctDbtTxInf.PmtId.InstrId);
            Assert.AreEqual(directDebitTransaction.TransactionID, directDebitTransactionInformation_DrctDbtTxInf.PmtId.EndToEndId);
            Assert.AreEqual(directDebitTransaction.Amount, directDebitTransactionInformation_DrctDbtTxInf.InstdAmt.Value);
            Assert.AreEqual("EUR", directDebitTransactionInformation_DrctDbtTxInf.InstdAmt.Ccy);
            Assert.AreEqual(directDebitTransaction.MandateID, directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.MndtId);
            Assert.AreEqual(directDebitTransaction.MandateSigatureDate, directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.DtOfSgntr);
            Assert.IsTrue(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.DtOfSgntrSpecified);
            Assert.AreEqual(creditorAgent.BankBIC, directDebitTransactionInformation_DrctDbtTxInf.DbtrAgt.FinInstnId.BIC);
            Assert.AreEqual(directDebitTransaction.AccountHolderName, directDebitTransactionInformation_DrctDbtTxInf.Dbtr.Nm);
            Assert.AreEqual(directDebitTransaction.DebtorAccount.IBAN.IBAN, (string)directDebitTransactionInformation_DrctDbtTxInf.DbtrAcct.Id.Item);
            string[] expectedConcepts = new string[] { "Cuota Social Octubre 2013 --- 79,00", "Cuota Social Noviembre 2013 --- 79,00" };
            CollectionAssert.AreEqual(expectedConcepts, directDebitTransactionInformation_DrctDbtTxInf.RmtInf.Ustrd);
            Assert.AreEqual(ChargeBearerType1Code.SLEV, directDebitTransactionInformation_DrctDbtTxInf.ChrgBr);
            Assert.IsFalse(directDebitTransactionInformation_DrctDbtTxInf.ChrgBrSpecified);                              //ChargeBearer se especifica una sola vez por remesa, en el PaymentInformation

            Assert.IsFalse(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.AmdmntInd);              //AmmendmentInformationInd es 'false'
            Assert.IsFalse(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.AmdmntIndSpecified);     //Si AmmendmentInformationInd es 'false', no hace falta ni siquiera incluirlo
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.AmdmntInfDtls);

            AssertUnusedDirectDebitTransactionInformation9_DrctDbtTxInf_Fields(directDebitTransactionInformation_DrctDbtTxInf);
        }

                [TestMethod]
        public void ADirectDebitTransactionInformation9WithAmmendmentInformation_ChangeMandateID_IsCorrectlyGenerated()
        {
            string transactionID = "00001";
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            string oldMandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(1000);
            DirectDebitAmendmentInformation directDebitAmendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, null);
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                bills,
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                directDebitAmendmentInformation,
                false);
            bool singleUnstructuredConcept = true;

            DirectDebitTransactionInformation9 directDebitTransactionInformation = SEPAElementsGenerator.GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                creditorAgent,
                directDebitTransaction,
                singleUnstructuredConcept);

            Assert.IsTrue(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInd);              //AmmendmentInformationInd es 'false'
            Assert.IsTrue(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntIndSpecified);     //Si AmmendmentInformationInd es 'false', no hace falta ni siquiera incluirlo

            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAcct);
            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAgt);
            Assert.AreEqual(oldMandateID, directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlMndtId);

            AssertUnusedAmendmentInformationDetails6_AmdmntInfDtls_Fields(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls);
        }

        [TestMethod]
        public void ADirectDebitTransactionInformation9WithAmmendmentInformation_ChangeDebtorAccountSameDebtorAgent_IsCorrectlyGenerated()
        {
            string transactionID = "00001";
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount oldBankAcount = new BankAccount(new InternationalAccountBankNumberIBAN(new ClientAccountCodeCCC("21002222002222222222")));

            DirectDebitAmendmentInformation directDebitAmendmentInformation = new DirectDebitAmendmentInformation(null, oldBankAcount);
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                bills,
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                directDebitAmendmentInformation,
                false);
            bool singleUnstructuredConcept = true;

            DirectDebitTransactionInformation9 directDebitTransactionInformation = SEPAElementsGenerator.GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                creditorAgent,
                directDebitTransaction,
                singleUnstructuredConcept);

            Assert.IsTrue(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInd);              //AmmendmentInformationInd es 'false'
            Assert.IsTrue(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntIndSpecified);     //Si AmmendmentInformationInd es 'false', no hace falta ni siquiera incluirlo

            Assert.AreEqual(oldBankAcount.IBAN.IBAN, (string)directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAcct.Id.Item);
            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAgt);
            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlMndtId);
        }

        [TestMethod]
        public void ADirectDebitTransactionInformation9WithAmmendmentInformation_ChangeDebtorAccountDifferentDebtorAgent_IsCorrectlyGenerated()
        {
            string transactionID = "00001";
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            BankAccount oldBankAcount = new BankAccount(new InternationalAccountBankNumberIBAN(new ClientAccountCodeCCC("12345678061234567890")));

            DirectDebitAmendmentInformation directDebitAmendmentInformation = new DirectDebitAmendmentInformation(null, oldBankAcount);
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                bills,
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                directDebitAmendmentInformation,
                true);
            bool singleUnstructuredConcept = true;

            DirectDebitTransactionInformation9 directDebitTransactionInformation = SEPAElementsGenerator.GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                creditorAgent,
                directDebitTransaction,
                singleUnstructuredConcept);

            Assert.IsTrue(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInd);
            Assert.IsTrue(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntIndSpecified);

            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAcct);
            Assert.AreEqual("SMNDA", directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAgt.FinInstnId.Othr.Id);
            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlMndtId);
        }

        [TestMethod]
        public void ADirectDebitTransactionInformation9WithAllConceptsJoinedIsCorrectlyGenrated()
        {
            string transactionID = "00001";
            Debtor debtor = debtors["00002"];
            List<SimplifiedBill> bills = debtor.SimplifiedBills.Values.ToList();
            DirectDebitMandate directDebitMandate = debtors["00002"].DirectDebitmandates.ElementAt(0).Value;
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitMandate.InternalReferenceNumber);
            BankAccount debtorAccount = directDebitMandate.BankAccount;
            string accountHolderName = directDebitMandate.AccountHolderName;
            DateTime mandateSignatureDate = directDebitMandate.DirectDebitMandateCreationDate;
            bool singleUnstructuredConcept = true;
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                bills,
                transactionID,
                mandateID,
                mandateSignatureDate,
                debtorAccount,
                accountHolderName,
                null,
                false);

            DirectDebitTransactionInformation9 directDebitTransactionInformation = SEPAElementsGenerator.GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                creditorAgent,
                directDebitTransaction,
                singleUnstructuredConcept);

            string[] expectedStringArrayWithOnlyOneString = new string[] { "Cuota Social Octubre 2013 --- 79,00; Cuota Social Noviembre 2013 --- 79,00" };
            CollectionAssert.AreEqual(expectedStringArrayWithOnlyOneString, directDebitTransactionInformation.RmtInf.Ustrd);
        }

        [TestMethod]
        public void ARCURPaymentInstructionInformation4IsCorrectlyGenerated()
        {
            string localInstrument = "COR1";
            bool firstDebits = false;
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction1, directDebitTransaction2 };

            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, firstDebits, directDebitTransactions);

            DateTime requestedCollectionDate = DateTime.Now.AddDays(3);
            bool singleUnstructuredConcept = true;

            PaymentInstructionInformation4 paymentInstructionInformation_PmtIf = SEPAElementsGenerator.GeneratePaymentInformation_PmtInf(
                creditor,
                creditorAgent,
                directDebitInitiationContract,
                directDebitPaymentInstruction,
                requestedCollectionDate,
                singleUnstructuredConcept);

            Assert.IsTrue(paymentInstructionInformation_PmtIf.BtchBookg);
            Assert.IsFalse(paymentInstructionInformation_PmtIf.BtchBookgSpecified);
            Assert.AreEqual(creditor.Name, paymentInstructionInformation_PmtIf.Cdtr.Nm);
            Assert.AreEqual(ChargeBearerType1Code.SLEV, paymentInstructionInformation_PmtIf.ChrgBr);
            Assert.IsTrue(paymentInstructionInformation_PmtIf.ChrgBrSpecified);
            Assert.AreEqual(directDebitInitiationContract.CreditorAcount.IBAN.IBAN, (string)paymentInstructionInformation_PmtIf.CdtrAcct.Id.Item);
            Assert.AreEqual(directDebitInitiationContract.CreditorAgent.BankBIC, paymentInstructionInformation_PmtIf.CdtrAgt.FinInstnId.BIC);
            Assert.AreEqual(directDebitInitiationContract.CreditorID, ((PersonIdentification5)paymentInstructionInformation_PmtIf.CdtrSchmeId.Id.Item).Othr[0].Id);
            Assert.AreEqual("SEPA", ((PersonIdentification5)paymentInstructionInformation_PmtIf.CdtrSchmeId.Id.Item).Othr[0].SchmeNm.Item);
            Assert.AreEqual(ItemChoiceType.Prtry, ((PersonIdentification5)paymentInstructionInformation_PmtIf.CdtrSchmeId.Id.Item).Othr[0].SchmeNm.ItemElementName);
            Assert.AreEqual(directDebitPaymentInstruction.TotalAmount, paymentInstructionInformation_PmtIf.CtrlSum);
            Assert.IsTrue(paymentInstructionInformation_PmtIf.CtrlSumSpecified);
            Assert.AreEqual(directDebitPaymentInstruction.NumberOfDirectDebitTransactions, paymentInstructionInformation_PmtIf.DrctDbtTxInf.Count());
            decimal paymentInstructionInformationDrctDbtTxInfInstructedAmountSum =
                paymentInstructionInformation_PmtIf.DrctDbtTxInf.ToList().Select(drctDbtTxInf => drctDbtTxInf.InstdAmt.Value).Sum();
            Assert.AreEqual(directDebitPaymentInstruction.TotalAmount, paymentInstructionInformationDrctDbtTxInfInstructedAmountSum);
            Assert.AreEqual(directDebitPaymentInstruction.NumberOfDirectDebitTransactions.ToString(), paymentInstructionInformation_PmtIf.NbOfTxs);
            Assert.AreEqual(directDebitPaymentInstruction.PaymentInformationID, paymentInstructionInformation_PmtIf.PmtInfId);
            Assert.AreEqual(PaymentMethod2Code.DD, paymentInstructionInformation_PmtIf.PmtMtd);
            Assert.AreEqual("TRAD", paymentInstructionInformation_PmtIf.PmtTpInf.CtgyPurp.Item);
            Assert.AreEqual(ItemChoiceType.Cd, paymentInstructionInformation_PmtIf.PmtTpInf.CtgyPurp.ItemElementName);
            Assert.AreEqual(directDebitPaymentInstruction.LocalInstrument, paymentInstructionInformation_PmtIf.PmtTpInf.LclInstrm.Item);
            Assert.AreEqual(ItemChoiceType.Cd, paymentInstructionInformation_PmtIf.PmtTpInf.LclInstrm.ItemElementName);
            Assert.AreEqual(SequenceType1Code.RCUR, paymentInstructionInformation_PmtIf.PmtTpInf.SeqTp);
            Assert.IsTrue(paymentInstructionInformation_PmtIf.PmtTpInf.SeqTpSpecified);
            Assert.AreEqual(ItemChoiceType.Cd, paymentInstructionInformation_PmtIf.PmtTpInf.SvcLvl.ItemElementName);
            Assert.AreEqual("SEPA", paymentInstructionInformation_PmtIf.PmtTpInf.SvcLvl.Item);
            Assert.AreEqual(requestedCollectionDate, paymentInstructionInformation_PmtIf.ReqdColltnDt);

            AssertUnusedPaymentInstructionInformation4_PmtIf_Fields(paymentInstructionInformation_PmtIf);
        }

        [TestMethod]
        public void AFRSTPaymentInstructionInformation4IsCorrectlyGenerated()
        {
            string localInstrument = "COR1";
            bool firstDebits = true;
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>() { directDebitTransaction4 };

            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID1, localInstrument, firstDebits, directDebitTransactions);

            DateTime requestedCollectionDate = DateTime.Now.AddDays(3);
            bool singleUnstructuredConcept = true;

            PaymentInstructionInformation4 paymentInstructionInformation_PmtIf = SEPAElementsGenerator.GeneratePaymentInformation_PmtInf(
                creditor,
                creditorAgent,
                directDebitInitiationContract,
                directDebitPaymentInstruction,
                requestedCollectionDate,
                singleUnstructuredConcept);

            Assert.IsTrue(paymentInstructionInformation_PmtIf.BtchBookg);
            Assert.IsFalse(paymentInstructionInformation_PmtIf.BtchBookgSpecified);
            Assert.AreEqual(creditor.Name, paymentInstructionInformation_PmtIf.Cdtr.Nm);
            Assert.AreEqual(ChargeBearerType1Code.SLEV, paymentInstructionInformation_PmtIf.ChrgBr);
            Assert.IsTrue(paymentInstructionInformation_PmtIf.ChrgBrSpecified);
            Assert.AreEqual(directDebitInitiationContract.CreditorAcount.IBAN.IBAN, (string)paymentInstructionInformation_PmtIf.CdtrAcct.Id.Item);
            Assert.AreEqual(directDebitInitiationContract.CreditorAgent.BankBIC, paymentInstructionInformation_PmtIf.CdtrAgt.FinInstnId.BIC);
            Assert.AreEqual(directDebitInitiationContract.CreditorID, ((PersonIdentification5)paymentInstructionInformation_PmtIf.CdtrSchmeId.Id.Item).Othr[0].Id);
            Assert.AreEqual("SEPA", ((PersonIdentification5)paymentInstructionInformation_PmtIf.CdtrSchmeId.Id.Item).Othr[0].SchmeNm.Item);
            Assert.AreEqual(ItemChoiceType.Prtry, ((PersonIdentification5)paymentInstructionInformation_PmtIf.CdtrSchmeId.Id.Item).Othr[0].SchmeNm.ItemElementName);
            Assert.AreEqual(directDebitPaymentInstruction.TotalAmount, paymentInstructionInformation_PmtIf.CtrlSum);
            Assert.IsTrue(paymentInstructionInformation_PmtIf.CtrlSumSpecified);
            Assert.AreEqual(directDebitPaymentInstruction.NumberOfDirectDebitTransactions, paymentInstructionInformation_PmtIf.DrctDbtTxInf.Count());
            decimal paymentInstructionInformationDrctDbtTxInfInstructedAmountSum =
                paymentInstructionInformation_PmtIf.DrctDbtTxInf.ToList().Select(drctDbtTxInf => drctDbtTxInf.InstdAmt.Value).Sum();
            Assert.AreEqual(directDebitPaymentInstruction.TotalAmount, paymentInstructionInformationDrctDbtTxInfInstructedAmountSum);
            Assert.AreEqual(directDebitPaymentInstruction.NumberOfDirectDebitTransactions.ToString(), paymentInstructionInformation_PmtIf.NbOfTxs);
            Assert.AreEqual(directDebitPaymentInstruction.PaymentInformationID, paymentInstructionInformation_PmtIf.PmtInfId);
            Assert.AreEqual(PaymentMethod2Code.DD, paymentInstructionInformation_PmtIf.PmtMtd);
            Assert.AreEqual("TRAD", paymentInstructionInformation_PmtIf.PmtTpInf.CtgyPurp.Item);
            Assert.AreEqual(ItemChoiceType.Cd, paymentInstructionInformation_PmtIf.PmtTpInf.CtgyPurp.ItemElementName);
            Assert.AreEqual(directDebitPaymentInstruction.LocalInstrument, paymentInstructionInformation_PmtIf.PmtTpInf.LclInstrm.Item);
            Assert.AreEqual(ItemChoiceType.Cd, paymentInstructionInformation_PmtIf.PmtTpInf.LclInstrm.ItemElementName);
            Assert.AreEqual(SequenceType1Code.FRST, paymentInstructionInformation_PmtIf.PmtTpInf.SeqTp);
            Assert.IsTrue(paymentInstructionInformation_PmtIf.PmtTpInf.SeqTpSpecified);
            Assert.AreEqual(ItemChoiceType.Cd, paymentInstructionInformation_PmtIf.PmtTpInf.SvcLvl.ItemElementName);
            Assert.AreEqual("SEPA", paymentInstructionInformation_PmtIf.PmtTpInf.SvcLvl.Item);
            Assert.AreEqual(requestedCollectionDate, paymentInstructionInformation_PmtIf.ReqdColltnDt);
        }

        private void AssertUnusedDirectDebitTransactionInformation9_DrctDbtTxInf_Fields(DirectDebitTransactionInformation9 directDebitTransactionInformation_DrctDbtTxInf)
        {
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.Dbtr.CtctDtls);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.Dbtr.CtryOfRes);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.Dbtr.Id);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.Dbtr.PstlAdr);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAcct.Ccy);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAcct.Nm);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAcct.Tp);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAgt.BrnchId);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAgt.FinInstnId.ClrSysMmbId);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAgt.FinInstnId.Nm);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAgt.FinInstnId.Othr);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAgt.FinInstnId.PstlAdr);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DbtrAgtAcct);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.CdtrSchmeId);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.ElctrncSgntr);
            Assert.AreEqual(DateTime.MaxValue, directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.FnlColltnDt);    //No se admiten nulos, asi que se pone un valor maximo
            Assert.IsFalse(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.FnlColltnDtSpecified);               //No se serializa
            Assert.AreEqual(Frequency1Code.MNTH, directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.Frqcy);
            Assert.IsFalse(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.FrqcySpecified);                     //Aunque se especifica frecuencia mensual, no se serializa
            Assert.AreEqual(DateTime.MinValue, directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.FrstColltnDt);   //No se admiten nulos, asi que se pone un valor minimo
            Assert.IsFalse(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.MndtRltdInf.FrstColltnDtSpecified);              //No se serializa
            Assert.AreEqual(DateTime.MinValue, directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.PreNtfctnDt);                //No se admiten nulos, asi que se pone un valor minimo
            Assert.IsFalse(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.PreNtfctnDtSpecified);                           //No se serializa
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.DrctDbtTx.PreNtfctnId);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.InstrForCdtrAgt);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.PmtTpInf);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.Purp);
            CollectionAssert.AreEqual(new RegulatoryReporting3[] { null }, directDebitTransactionInformation_DrctDbtTxInf.RgltryRptg);
            CollectionAssert.AreEqual(new RemittanceLocation2[] { null }, directDebitTransactionInformation_DrctDbtTxInf.RltdRmtInf);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.Tax);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.UltmtCdtr);
            Assert.IsNull(directDebitTransactionInformation_DrctDbtTxInf.UltmtDbtr);
        }

        private void AssertUnusedAmendmentInformationDetails6_AmdmntInfDtls_Fields(AmendmentInformationDetails6 amendmentInformationDetails)
        {
            Assert.IsNull(amendmentInformationDetails.OrgnlCdtrAgt);
            Assert.IsNull(amendmentInformationDetails.OrgnlCdtrAgtAcct);
            Assert.IsNull(amendmentInformationDetails.OrgnlCdtrSchmeId);
            Assert.IsNull(amendmentInformationDetails.OrgnlDbtr);
            Assert.IsNull(amendmentInformationDetails.OrgnlDbtrAgtAcct);
            Assert.AreEqual(DateTime.MinValue, amendmentInformationDetails.OrgnlFnlColltnDt);   //dateTime can't be null
            Assert.IsFalse(amendmentInformationDetails.OrgnlFnlColltnDtSpecified);
            Assert.AreEqual(Frequency1Code.MNTH, amendmentInformationDetails.OrgnlFrqcy);       //enum can't be null
            Assert.IsFalse(amendmentInformationDetails.OrgnlFrqcySpecified);
        }

        private void AssertUnusedPaymentInstructionInformation4_PmtIf_Fields(PaymentInstructionInformation4 paymentInstructionInformation_PmtIf)
        {
            Assert.IsNull(paymentInstructionInformation_PmtIf.Cdtr.CtctDtls);
            Assert.IsNull(paymentInstructionInformation_PmtIf.Cdtr.CtryOfRes);
            Assert.IsNull(paymentInstructionInformation_PmtIf.Cdtr.Id);
            Assert.IsNull(paymentInstructionInformation_PmtIf.Cdtr.PstlAdr);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAcct.Ccy);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAcct.Nm);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAcct.Tp);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAgt.BrnchId);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAgt.FinInstnId.ClrSysMmbId);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAgt.FinInstnId.Nm);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAgt.FinInstnId.Othr);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAgt.FinInstnId.PstlAdr);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrAgtAcct);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrSchmeId.CtctDtls);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrSchmeId.CtryOfRes);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrSchmeId.Nm);
            Assert.IsNull(paymentInstructionInformation_PmtIf.CdtrSchmeId.PstlAdr);
            Assert.IsNull(paymentInstructionInformation_PmtIf.ChrgsAcct);
            Assert.IsNull(paymentInstructionInformation_PmtIf.ChrgsAcctAgt);
            Assert.AreEqual(Priority2Code.NORM, paymentInstructionInformation_PmtIf.PmtTpInf.InstrPrty);
            Assert.IsFalse(paymentInstructionInformation_PmtIf.PmtTpInf.InstrPrtySpecified);
            Assert.IsNull(paymentInstructionInformation_PmtIf.UltmtCdtr);
        }
    }
}
