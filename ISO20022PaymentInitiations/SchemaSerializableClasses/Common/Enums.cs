namespace   ISO20022PaymentInitiations.SchemaSerializableClasses
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum AddressType2Code
    {

        /// Postal
        ADDR,

        /// POBox
        PBOX,

        /// Residential
        HOME,

        /// Business
        BIZZ,

        /// MailTo
        MLTO,

        /// DeliveryTo
        DLVY,
    }

    /// Authorisation Code
    /// Specifies the authorisation, in a coded form
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum Authorisation1Code
    {

        /// PreAuthorisedFile
        AUTH,

        /// FileLevelAuthorisationDetails
        FDET,

        /// FileLevelAuthorisationSummary
        FSUM,

        /// InstructionLevelAuthorisation
        ILEV,
    }

    /// Account type, in a coded form
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum CashAccountType4Code
    {

        /// CashPayment
        CASH,

        /// Charges
        CHAR,

        /// Commission
        COMM,

        /// Tax
        TAXE,

        /// CashIncome
        CISH,

        /// CashTrading
        TRAS,

        /// Settlement
        SACC,

        /// Current
        CACC,

        /// Savings
        SVGS,

        /// OverNightDeposit
        ONDP,

        /// MarginalLending
        MGLD,

        /// NonResidentExternal
        NREX,

        /// MoneyMarket
        MOMA,

        /// Loan
        LOAN,

        /// Salary
        SLRY,

        /// Overdraft
        ODFT,
    }

    /// Specifies the clearing channel to be used to process the payment instruction.
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public enum ClearingChannel2Code
    {

        /// RealTimeGrossSettlementSystem
        RTGS,

        /// RealTimeNetSettlementSystem
        RTNS,

        /// MassPaymentNetSystem
        MPNS,

        /// BookTransfer
        BOOK,
    }

    /// Credit or Debit code
    /// Specifies whether the adjustment must be substracted or added to the total amount
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum CreditDebitCode
    {

        /// Credit
        CRDT,

        /// Debit
        DBIT,
    }

    /// Specifies which party/parties will bear the charges associated with the processing of the payment transaction
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum ChargeBearerType1Code
    {

        /// BorneByDebtor
        DEBT,

        /// BorneByCreditor
        CRED,

        /// Shared
        SHAR,

        /// FollowingServiceLevel
        SLEV,
    }

    /// Document Type
    /// Type of creditor reference, in a coded form.
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum DocumentType3Code
    {

        /// RemittanceAdviceMessage
        RADM,

        /// RelatedPaymentInstruction
        RPIN,

        /// ForeignExchangeDealReference
        FXDR,

        /// DispatchAdvice
        DISP,

        /// PurchaseOrder
        PUOR,

        /// StructuredCommunicationReference
        SCOR,
    }

    /// Document Type
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum DocumentType5Code
    {

        /// MeteredServiceInvoice
        MSIN,

        /// CreditNoteRelatedToFinancialAdjustment
        CNFA,

        /// DebitNoteRelatedToFinancialAdjustment
        DNFA,

        /// CommercialInvoice
        CINV,

        /// CreditNote
        CREN,

        /// DebitNote>
        DEBN,

        /// HireInvoice
        HIRI,

        /// SelfBilledInvoice
        SBIN,

        /// CommercialContract
        CMCN,

        /// StatementOfAccount
        SOAC,

        /// DispatchAdvice
        DISP,

        /// BillOfLading
        BOLD,

        /// Voucher
        VCHR,

        /// AccountReceivableOpenItem
        AROI,

        /// TradeServicesUtilityTransaction
        TSUT,
    }

    /// Regularity with which direct debit instructions are to be created and processed
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum Frequency1Code
    {

        /// Yearly
        YEAR,

        /// Monthly
        MNTH,

        /// Quarterly
        QURT,

        /// SemiAnnual
        MIAN,

        /// Weekly
        WEEK,

        /// Daily
        DAIL,

        /// Adhoc
        ADHO,

        /// IntraDay
        INDA,
    }

    /// Choice option: Coded ("Cd") or Propietary("Prtry")
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// Coded
        Cd,

        /// Propietary
        Prtry,
    }

    /// Name Prefix (address)
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum NamePrefix1Code
    {

        /// Doctor
        DOCT,

        /// Mister
        MIST,

        /// Miss
        MISS,

        /// Madam
        MADM,
    }

    /// Specifies the means of payment that will be used to move the amount of money
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum PaymentMethod2Code
    {

        /// DirectDebit
        DD,
    }

    /// Specifies the means of payment that will be used to move the amount of money.
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public enum PaymentMethod4Code
    {

        /// Cheque
        CHK,

        /// CreditTransfer
        TRF,

        /// DirectDebit
        DD,

        /// TransferAdvice
        TRA,
    }

    /// Indicator of the urgency or order of importance that the instructing party would like the instructed party to apply to the processing of the instruction.
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum Priority2Code
    {

        /// High
        HIGH,

        /// Normal
        NORM,
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum RegulatoryReportingType1Code
    {

        /// Credit
        CRED,

        /// Debit
        DEBT,

        /// Both
        BOTH,
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum RemittanceLocationMethod2Code
    {

        /// Fax
        FAXI,

        /// ElectronicDataInterchange
        EDIC,

        /// UniformResourceIdentifier
        URID,

        /// EMail
        EMAL,

        /// Post
        POST,

        /// SMS
        SMSM,
    }

    /// Identifies the direct debit sequence, such as first, recurrent, final or one-off.
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum SequenceType1Code
    {
        /// First
        FRST,

        /// Recurring
        RCUR,

        /// Final
        FNAL,

        /// OneOff
        OOFF,
    }

    /// Method used to settle the (batch of) payment instructions.
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public enum SettlementMethod1Code
    {

        /// InstructedAgent
        INDA,

        /// InstructingAgent
        INGA,

        /// CoverMethod
        COVE,

        /// ClearingSystem
        CLRG,
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public enum TaxRecordPeriod1Code
    {

        /// FirstMonth
        MM01,

        /// SecondMonth
        MM02,

        /// ThirdMonth
        MM03,

        /// FourthMont
        MM04,

        /// FifthMonth
        MM05,

        /// SixthMonth
        MM06,

        /// SeventhMonth
        MM07,

        /// EigthMonth
        MM08,

        /// NinethMonth
        MM09,

        /// TenthMonth
        MM10,

        /// EleventhMonth
        MM11,

        /// TwelvethMonth
        MM12,

        /// FirsQuarter
        QTR1,

        /// SecondQuarter
        QTR2,

        /// ThirdQuarter
        QTR3,

        /// FourthQuarter
        QTR4,

        /// FirstHalf
        HLF1,

        /// SecondHalf
        HLF2,
    }

    /// Specifies the status of the payment information group.
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public enum TransactionGroupStatus3Code
    {

        /// AcceptedTechnicalValidation
        ACTC,

        /// Received
        RCVD,

        /// PartiallyAccepted
        PART,

        /// Rejected
        RJCT,

        /// Pending
        PDNG,

        /// AcceptedCustomerProfile
        ACCP,

        /// AcceptedSettlementInProcess
        ACSP,

        /// AcceptedSettlementCompleted
        ACSC,

        /// AcceptedWithChange
        ACWC,
    }

    /// Specifies the status of a transaction, in a coded form
    [System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public enum TransactionIndividualStatus3Code
    {

        /// AcceptedTechnicalValidation
        ACTC,

        /// Rejected
        RJCT,

        /// Pending
        PDNG,

        /// AcceptedCustomerProfile
        ACCP,

        /// AcceptedSettlementInProcess
        ACSP,

        /// AcceptedSettlementCompleted
        ACSC,

        /// AcceptedWithChange
        ACWC,
    }


}
