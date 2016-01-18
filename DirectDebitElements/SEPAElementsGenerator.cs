using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISO20022PaymentInitiations.SchemaSerializableClasses;
using ISO20022PaymentInitiations.SchemaSerializableClasses.DDInitiation;
using ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport;
using ExtensionMethods;



namespace DirectDebitElements
{
    public static class SEPAElementsGenerator
    {

        public static PartyIdentification32 GenerateInitiationParty_InitPty(
            Creditor creditor,
            DirectDebitInitiationContract directDebitInitiationContract)
        {
            OrganisationIdentificationSchemeName1Choice orgIDSchemeNameChoice_schmeNm = new OrganisationIdentificationSchemeName1Choice(
                "SEPA", ItemChoiceType.Prtry);

            GenericOrganisationIdentification1 genericOrganisationIdentification_othr = new GenericOrganisationIdentification1(
                directDebitInitiationContract.CreditorID,           //<Id>
                orgIDSchemeNameChoice_schmeNm,                      //<SchemeNm>
                null);                                              //<Issr> - No issuer

            OrganisationIdentification4 organisationIdentification_orgiD = new OrganisationIdentification4(
                null,
                new GenericOrganisationIdentification1[] { genericOrganisationIdentification_othr });

            Party6Choice organisationOrPrivateIdentification_id = new Party6Choice(organisationIdentification_orgiD);

            PartyIdentification32 initiationParty_initgPty = new PartyIdentification32(
                creditor.Name,                              //<Nm>
                null,                                       //<PstlAdr> - Not used in SEPA
                organisationOrPrivateIdentification_id,     //<OrgID> or <PrvtId>
                null,                                       //<CtryOfRes> - Not used in SEPA
                null);                                      //<CtctDtls> - Not used in SEPA

            return initiationParty_initgPty;
        }

        public static GroupHeader39 GenerateGroupHeader_GrpHdr(
            string messageID,
            DateTime generationDateTime,
            int numberOfTransactions,
            decimal controlSum,
            PartyIdentification32 initiationParty_InitgPty)
        {

            Authorisation1Choice[] authorisation_authstn = new Authorisation1Choice[] { null };

            DateTime creatingdDateTime =
                DateTime.SpecifyKind(generationDateTime, DateTimeKind.Unspecified).Truncate(TimeSpan.FromSeconds(1));

            GroupHeader39 groupHeader_grpHdr = new GroupHeader39(
                messageID,                                              //<MsgID>
                creatingdDateTime,                                      //<CreDtTm>
                authorisation_authstn,                                  //<Authstn> - Not used in SEPA. Array of null instead of null to avoid null reference exception
                numberOfTransactions.ToString(),                        //<NbOfTxs>
                controlSum,                                             //<CtrlSum>
                true,                                                   //Control sum is specified
                initiationParty_InitgPty,                               //<InitgPty>
                null);                                                  //<FwdgAgt> - Not used by creditor in SEPA COR

            return groupHeader_grpHdr;
        }

        public static DirectDebitTransactionInformation9 GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
            CreditorAgent creditorAgent,
            DirectDebitTransaction directDebitTransaction,
            bool singleUnstructuredConcept)
        {
            PaymentIdentification1 paymentIdentification_PmtID = new PaymentIdentification1(
                directDebitTransaction.InternalUniqueInstructionID,    //<InstrID>
                directDebitTransaction.InternalUniqueInstructionID);   //<EndToEndID>

            ActiveOrHistoricCurrencyAndAmount instructedAmount_InstdAmt = new ActiveOrHistoricCurrencyAndAmount(
                "EUR",                                      //<InstdAmt> ""CCY" atribute value
                directDebitTransaction.Amount);             //<InstdAmt>

            MandateRelatedInformation6 mandateRelatedInformation_MndtRltdInf = new MandateRelatedInformation6(
                directDebitTransaction.MandateID,               //<MndtID>
                directDebitTransaction.MandateSigatureDate,     //<DtOfSgntr>
                true,                                           //<DtOfSgntr> will be serialized
                false,                                          //<AmdmntInd> - There is no amendment
                false,                                          //<AmdmntInd> will not be serialize
                null,                                           //<AmdmntInfDtls> - No amendment details
                null,                                           //<ElctrncSgntr> - No electronic signature 
                DateTime.MinValue,                              //<FrstColltnDt> - Not used by creditor in SEPA COR, but can't be null
                false,                                          //<FrstColltnDt> will not be serialized
                DateTime.MaxValue,                              //<FnlColltnDt> - Not used by creditor in SEPA COR, but can't be null
                false,                                          //<FnlColltnDt> will not be serialized
                Frequency1Code.MNTH,                            //<Frqcy> - Not used by creditor in SEPA COR, but can't be null
                false);                                         //<Frqcy> will not be serialized

            DirectDebitTransaction6 directDebitTransaction_DrctDbtTx = new DirectDebitTransaction6(
                mandateRelatedInformation_MndtRltdInf,  //<MndtRltdInf>
                null,                                   //<CdtrSchmeId> - No. Only one creditor scheme per payment information <PmtInf> group  
                null,                                   //<PreNtfctnId> - Not used by creditor in SEPA COR
                DateTime.MinValue,                      //<PreNtfctnDt> - Not used by creditor in SEPA COR, but can't be null
                false);                                 //<PreNtfctnDt> will not be serialized 

            FinancialInstitutionIdentification7 financialInstitutuinIdentification_FinInstnID = new FinancialInstitutionIdentification7(
                creditorAgent.BankBIC,  //<BIC>
                null,                   //<ClrYsMmbId> - Not used by creditor in SEPA COR
                null,                   //<Nm> Not used by creditor in SEPA COR
                null,                   //<PstlAdr> - Not used by creditor in SEPA COR
                null);                  //<Othr> - Not used by creditor in SEPA COR

            BranchAndFinancialInstitutionIdentification4 debtorAgent_DbtrAgt = new BranchAndFinancialInstitutionIdentification4(
                financialInstitutuinIdentification_FinInstnID,  //<FinInstnId>
                null);                                          //<BrcnhID> - Not used by creditor in SEPA COR

            PartyIdentification32 debtor_Dbtr = new PartyIdentification32(
                directDebitTransaction.AccountHolderName,   //<Nm>
                null,                                       //<PstlAdr> - No postal address needed
                null,                                       //<Id> - No extra ID needed
                null,                                       //<CtryOfRes> - Not used by creditor in SEPA COR
                null);                                      //<CtctDtls> - Not used by creditor in SEPA COR

            AccountIdentification4Choice accountID_Id = new AccountIdentification4Choice(
                directDebitTransaction.DebtorAccount.IBAN.IBAN);

            CashAccount16 debtorAccount_DbtrAcct = new CashAccount16(
                accountID_Id,   //<Id>
                null,           //<Tp> - Not used by creditor in SEPA COR
                null,           //<Ccy> - Not used by creditor in SEPA COR
                null);          //<Nm> - Not used by creditor in SEPA COR

            string[] remittanceConcepts = BuildUnstructuredRemmitanceInformation(directDebitTransaction, singleUnstructuredConcept);

            RemittanceInformation5 remitanceInformation_RmtInf = new RemittanceInformation5(
                remittanceConcepts,                                     //<Ustrd>
                new StructuredRemittanceInformation7[] { null });       //<Strd> - Only <Ustrd> or <Strd>

            DirectDebitTransactionInformation9 directDebitTransactionInfo_DrctDbtTxInf = new DirectDebitTransactionInformation9(
                paymentIdentification_PmtID,        //<PmtID>
                null,                               //<PmtTpInf> - Not used by creditor in SEPA COR 
                instructedAmount_InstdAmt,          //<InstdAmt>
                ChargeBearerType1Code.SLEV,         //<ChrgBr> - No. Only one Charge Bearer per payment information <PmtInf> group
                false,                              //<ChrgBr> will not be serialized    
                directDebitTransaction_DrctDbtTx,   //<DrctDbtTx>
                null,                               //<UltmtCdtr> - Not necessary. If son, only one Ultimate Creditor per payment information <PmtInf> group
                debtorAgent_DbtrAgt,                //<DbtrAgt>
                null,                               //<DbtrAgtAcct> - Not used by creditor in SEPA COR
                debtor_Dbtr,                        //<Dbtr>
                debtorAccount_DbtrAcct,             //<DbtrAcct>
                null,                               //<UltmtDbtr> - Only if Ultimate Debtor is different from debtor.
                null,                               //<InstrForCdtrAgt> - Not used by creditor in SEPA COR
                null,                               //<Purp> - Not mandatory. Only use to inform debtor. Is meaningless for agents.
                new RegulatoryReporting3[] { null },//<RgltryRptg> - Only needed for big payments from non residents
                null,                               //<Tax> - Not used by creditor in SEPA COR
                new RemittanceLocation2[] { null }, //<RltdRmtInf> - Not used by creditor in SEPA COR
                remitanceInformation_RmtInf);       //<RmtInf>

            return directDebitTransactionInfo_DrctDbtTxInf;
        }

        public static  PaymentInstructionInformation4 GeneratePaymentInformation_PmtInf(
            Creditor creditor,
            CreditorAgent creditorAgent,
            DirectDebitInitiationContract directDebitInitiationContract,
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment,
            bool singleUnstructuredConcept)
        {
            string paymentInformationIdentificaction_PmtInfId = directDebitTransactionsGroupPayment.PaymentInformationID;  //Private unique ID for payment group
            DateTime reqCollectionDate_ReqdColltnDt = new DateTime(2014, 2, 01);
            List<DirectDebitTransactionInformation9> directDebitTransactionInfo_DrctDbtTxInfList = new List<DirectDebitTransactionInformation9>();
            foreach (DirectDebitTransaction directDebitTransaction in directDebitTransactionsGroupPayment.DirectDebitTransactionsCollection)
            {
                DirectDebitTransactionInformation9 directDebitTransactionInfo_DrctDbtTxInf = GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                    creditorAgent,
                    directDebitTransaction,
                    singleUnstructuredConcept);
                directDebitTransactionInfo_DrctDbtTxInfList.Add(directDebitTransactionInfo_DrctDbtTxInf);
            }

            ServiceLevel8Choice serviceLevel_SvcLvl = new ServiceLevel8Choice("SEPA", ItemChoiceType.Cd);

            LocalInstrument2Choice localInstrument_LclInstrm = new LocalInstrument2Choice(directDebitTransactionsGroupPayment.LocalInstrument, ItemChoiceType.Cd);

            CategoryPurpose1Choice categoryOfPurpose_CtgyPurp = new CategoryPurpose1Choice("TRAD", ItemChoiceType.Cd);

            PaymentTypeInformation20 paymentTypeInformation_PmtTpInf = new PaymentTypeInformation20(
                Priority2Code.NORM,                 //<InstrPrty> Not used in SEPA COR1, but can't be null
                false,                              //<InstrPrty> will not be serialized
                serviceLevel_SvcLvl,                //<SvcLvl>
                localInstrument_LclInstrm,          //<LclInstrm>
                SequenceType1Code.RCUR,             //<SeqTp>
                true,                               //<SeqTP> wll be serialized
                categoryOfPurpose_CtgyPurp);        //<CtgyPurp>

            PartyIdentification32 creditor_Cdtr = new PartyIdentification32(
                creditor.Name, null, null, null, null);

            AccountIdentification4Choice creditorAccount_Id = new AccountIdentification4Choice(
                directDebitInitiationContract.CreditorAcount.IBAN.IBAN);

            CashAccount16 creditorAccount_CdtrAcct = new CashAccount16(
                creditorAccount_Id, null, null, null);

            FinancialInstitutionIdentification7 financialInstitutuinIdentification_FinInstnID = new FinancialInstitutionIdentification7(
                creditorAgent.BankBIC, null, null, null, null);

            BranchAndFinancialInstitutionIdentification4 creditorAgent_CdtrAgt = new BranchAndFinancialInstitutionIdentification4(
                financialInstitutuinIdentification_FinInstnID, null);


            OrganisationIdentificationSchemeName1Choice orgIDSchemeNameChoice_schmeNm = new OrganisationIdentificationSchemeName1Choice(
                "SEPA", ItemChoiceType.Prtry);

            GenericOrganisationIdentification1 genericOrganisationIdentification_othr = new GenericOrganisationIdentification1(
                directDebitInitiationContract.CreditorID, orgIDSchemeNameChoice_schmeNm, null);

            OrganisationIdentification4 organisationIdentification_orgiD = new OrganisationIdentification4(
                null,
                new GenericOrganisationIdentification1[] { genericOrganisationIdentification_othr });

            Party6Choice organisationOrPrivateIdentification_id = new Party6Choice(organisationIdentification_orgiD);

            PartyIdentification32 creditorSchemeIdentification_CdtrSchemeId = new PartyIdentification32(
                null, null, organisationOrPrivateIdentification_id, null, null);

            DirectDebitTransactionInformation9[] directDebitTransactionInfoCollection = directDebitTransactionInfo_DrctDbtTxInfList.ToArray();

            PaymentInstructionInformation4 paymentInformation_PmtInf = new PaymentInstructionInformation4(
                paymentInformationIdentificaction_PmtInfId, //<PmtInfId>
                PaymentMethod2Code.DD,                       //<PmtMtd>
                true,                                       //<BtchBookg> Only one account entry for all payments
                true,                                       //<BtchBookg> Will be serialized
                "2",                                        //<NbOfTxs>
                (decimal)237,                               //<CtrlSum>
                true,                                       //<CtrlSum> will be specified
                paymentTypeInformation_PmtTpInf,            //<PmtTpInf>
                reqCollectionDate_ReqdColltnDt,             //<ReqdColltnDt>
                creditor_Cdtr,                              //<Cdtr>
                creditorAccount_CdtrAcct,                   //<CdtrAcc>
                creditorAgent_CdtrAgt,                      //<CdtrAgt>
                null,                                       //<CdtrAgtAcct>
                null,                                       //<UltmtCdtr> Not neccesary. Same than creditor
                ChargeBearerType1Code.SLEV,                 //<ChrgBr>
                true,                                       //<ChrgBr> will be serialized
                null,                                       //<ChrgsAcct> Not used in SEPA COR1
                null,                                       //<ChrgsAcctAgt> Not used in SEPA COR1
                creditorSchemeIdentification_CdtrSchemeId,  //<CdtrSchemeId>
                directDebitTransactionInfoCollection);      //<DrctDbtTxInf>

            return paymentInformation_PmtInf;
        }

        private static string[] BuildUnstructuredRemmitanceInformation(DirectDebitTransaction directDebitTransaction, bool singleUnstructuredConcept)
        {
            string[] remittanceConcepts = directDebitTransaction.BillsInTransaction.Select
                (bill => bill.Description + " --- " + bill.Amount.ToString("0.00")).ToArray();
            if (singleUnstructuredConcept) remittanceConcepts = new string[] { String.Join("; ", remittanceConcepts).Left(140) };
            return remittanceConcepts;
         }
    }
}
