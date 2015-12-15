using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISO20022PaymentInitiations.CPStatusReportSerializableClasses
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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

    /// Account type, in a coded form.
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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

    /// Credit or Debit code.
    /// Specifies whether the adjustment must be substracted or added to the total amount
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public enum CreditDebitCode
    {

        /// Credit
        CRDT,

        /// Debit
        DBIT,
    }

    /// Document Type
    /// Type of creditor reference, in a coded form.
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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

    /// Document Type.
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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

    /// Regularity with which direct debit instructions are to be created and processed.
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// Coded
        Cd,

        /// Propietary
        Prtry,
    }

    /// Name Prefix (address)
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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

    /// Specifies the means of payment that will be used to move the amount of money.
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public enum Priority2Code
    {

        /// High
        HIGH,

        /// Normal
        NORM,
    }

    /// Identifies the direct debit sequence, such as first, recurrent, final or one-off.
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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

    /// Specifies the status of the payment information group.
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
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
