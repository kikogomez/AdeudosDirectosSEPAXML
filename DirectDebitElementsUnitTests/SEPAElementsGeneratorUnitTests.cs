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
                null);
            bool singleUnstructuredConcept = false;

            DirectDebitTransactionInformation9 directDebitTransactionInformation = SEPAElementsGenerator.GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                creditorAgent,
                directDebitTransaction,
                singleUnstructuredConcept);

            Assert.AreEqual(directDebitTransaction.TransactionID, directDebitTransactionInformation.PmtId.InstrId);
            Assert.AreEqual(directDebitTransaction.TransactionID, directDebitTransactionInformation.PmtId.EndToEndId);
            Assert.AreEqual(directDebitTransaction.Amount, directDebitTransactionInformation.InstdAmt.Value);
            Assert.AreEqual("EUR", directDebitTransactionInformation.InstdAmt.Ccy);
            Assert.AreEqual(directDebitTransaction.MandateID, directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.MndtId);
            Assert.AreEqual(directDebitTransaction.MandateSigatureDate, directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.DtOfSgntr);
            Assert.IsTrue(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.DtOfSgntrSpecified);
            Assert.AreEqual(creditorAgent.BankBIC, directDebitTransactionInformation.DbtrAgt.FinInstnId.BIC);
            Assert.AreEqual(directDebitTransaction.AccountHolderName, directDebitTransactionInformation.Dbtr.Nm);
            Assert.AreEqual(directDebitTransaction.DebtorAccount.IBAN.IBAN, (string)directDebitTransactionInformation.DbtrAcct.Id.Item);
            string[] expectedConcepts = new string[] { "Cuota Social Octubre 2013 --- 79,00", "Cuota Social Noviembre 2013 --- 79,00" };
            CollectionAssert.AreEqual(expectedConcepts, directDebitTransactionInformation.RmtInf.Ustrd);
            Assert.AreEqual(ChargeBearerType1Code.SLEV, directDebitTransactionInformation.ChrgBr);
            Assert.IsFalse(directDebitTransactionInformation.ChrgBrSpecified);                              //ChargeBearer se especifica una sola vez por remesa, en el PaymentInformation

            Assert.IsFalse(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInd);              //AmmendmentInformationInd es 'false'
            Assert.IsFalse(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntIndSpecified);     //Si AmmendmentInformationInd es 'false', no hace falta ni siquiera incluirlo
            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls);

            AssertUnusedDirectDebitTransactionInformation9(directDebitTransactionInformation);
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
                directDebitAmendmentInformation);
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

            AssertUnusedDirectDebitAmendmentInformation(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.AmdmntInfDtls);
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
                directDebitAmendmentInformation);
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
                directDebitAmendmentInformation);
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
                null);

            DirectDebitTransactionInformation9 directDebitTransactionInformation = SEPAElementsGenerator.GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                creditorAgent,
                directDebitTransaction,
                singleUnstructuredConcept);

            string[] expectedStringArrayWithOnlyOneString = new string[] { "Cuota Social Octubre 2013 --- 79,00; Cuota Social Noviembre 2013 --- 79,00" };
            CollectionAssert.AreEqual(expectedStringArrayWithOnlyOneString, directDebitTransactionInformation.RmtInf.Ustrd);
        }

        private void AssertUnusedDirectDebitTransactionInformation9(DirectDebitTransactionInformation9 directDebitTransactionInformation)
        {
            Assert.IsNull(directDebitTransactionInformation.Dbtr.CtctDtls);
            Assert.IsNull(directDebitTransactionInformation.Dbtr.CtryOfRes);
            Assert.IsNull(directDebitTransactionInformation.Dbtr.Id);
            Assert.IsNull(directDebitTransactionInformation.Dbtr.PstlAdr);
            Assert.IsNull(directDebitTransactionInformation.DbtrAcct.Ccy);
            Assert.IsNull(directDebitTransactionInformation.DbtrAcct.Nm);
            Assert.IsNull(directDebitTransactionInformation.DbtrAcct.Tp);
            Assert.IsNull(directDebitTransactionInformation.DbtrAgt.BrnchId);
            Assert.IsNull(directDebitTransactionInformation.DbtrAgt.FinInstnId.ClrSysMmbId);
            Assert.IsNull(directDebitTransactionInformation.DbtrAgt.FinInstnId.Nm);
            Assert.IsNull(directDebitTransactionInformation.DbtrAgt.FinInstnId.Othr);
            Assert.IsNull(directDebitTransactionInformation.DbtrAgt.FinInstnId.PstlAdr);
            Assert.IsNull(directDebitTransactionInformation.DbtrAgtAcct);
            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.CdtrSchmeId);
            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.ElctrncSgntr);
            Assert.AreEqual(DateTime.MaxValue, directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.FnlColltnDt);    //No se admiten nulos, asi que se pone un valor maximo
            Assert.IsFalse(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.FnlColltnDtSpecified);               //No se serializa
            Assert.AreEqual(Frequency1Code.MNTH, directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.Frqcy);
            Assert.IsFalse(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.FrqcySpecified);                     //Aunque se especifica frecuencia mensual, no se serializa
            Assert.AreEqual(DateTime.MinValue, directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.FrstColltnDt);   //No se admiten nulos, asi que se pone un valor minimo
            Assert.IsFalse(directDebitTransactionInformation.DrctDbtTx.MndtRltdInf.FrstColltnDtSpecified);              //No se serializa
            Assert.AreEqual(DateTime.MinValue, directDebitTransactionInformation.DrctDbtTx.PreNtfctnDt);                //No se admiten nulos, asi que se pone un valor minimo
            Assert.IsFalse(directDebitTransactionInformation.DrctDbtTx.PreNtfctnDtSpecified);                           //No se serializa
            Assert.IsNull(directDebitTransactionInformation.DrctDbtTx.PreNtfctnId);
            Assert.IsNull(directDebitTransactionInformation.InstrForCdtrAgt);
            Assert.IsNull(directDebitTransactionInformation.PmtTpInf);
            Assert.IsNull(directDebitTransactionInformation.Purp);
            CollectionAssert.AreEqual(new RegulatoryReporting3[] { null }, directDebitTransactionInformation.RgltryRptg);
            CollectionAssert.AreEqual(new RemittanceLocation2[] { null }, directDebitTransactionInformation.RltdRmtInf);
            Assert.IsNull(directDebitTransactionInformation.Tax);
            Assert.IsNull(directDebitTransactionInformation.UltmtCdtr);
            Assert.IsNull(directDebitTransactionInformation.UltmtDbtr);
        }

        private void AssertUnusedDirectDebitAmendmentInformation(AmendmentInformationDetails6 amendmentInformationDetails)
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
    }
}
