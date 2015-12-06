using System;
using System.Collections.Generic;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;

namespace RCNGCMembersManagementAppLogic.MembersManaging
{
    public class ClubMember
    {
        ClubMemberDataManager clubMemberDataManager;

        string memberID;
        string name;
        string firstSurname;
        string secondSurname;
        PaymentMethod defaultPaymentMethod;
        Dictionary<int, DirectDebitMandate> directDebitmandates;
        Dictionary<string, Invoice> invoicesList;
        Dictionary<string, ProFormaInvoice> proFormaInvoicesList;
        Dictionary<string, AmendingInvoice> amendingInvoicesList;

        public ClubMember(string givenName, string firstSurname, string secondSurname)
        {
            InitializeMemberInstance(givenName, firstSurname, secondSurname);
            this.memberID = GetMemberID();
        }

        public ClubMember(string memberID, string givenName, string firstSurname, string secondSurname)
        {
            InitializeMemberInstance(givenName, firstSurname, secondSurname);
            this.memberID = memberID;
        }

        public string MemberID
        {
            get { return memberID; }
        }

        public string Name
        {
            get { return name; }
        }

        public string FirstSurname
        {
            get { return firstSurname; }
        }

        public string SecondSurname
        {
            get { return secondSurname; }
        }

        public string FullName
        {
            get { return GetFullname(); }
        }

        public PaymentMethod DefaultPaymentMethod
        {
            get { return defaultPaymentMethod; }
        }

        public Dictionary<int, DirectDebitMandate> DirectDebitmandates
        {
            get { return directDebitmandates; }
        }

        public Dictionary<string, Invoice> InvoicesList
        {
            get { return invoicesList; }
        }

        public Dictionary<string, AmendingInvoice> AmendingInvoicesList
        {
            get { return amendingInvoicesList; }
        }

        public void SetDefaultPaymentMethod(PaymentMethod paymentMethod)
        {
            this.defaultPaymentMethod = paymentMethod;
        }

        public void AddInvoice(Invoice invoice)
        {
            invoicesList.Add(invoice.InvoiceID, invoice);
        }

        public void AddProFormainvoice(ProFormaInvoice proFormaInvoice)
        {
            proFormaInvoicesList.Add(proFormaInvoice.InvoiceID, proFormaInvoice);
        }

        public void AddAmendingInvoice(AmendingInvoice amendinginvoice)
        {
            amendingInvoicesList.Add(amendinginvoice.InvoiceID, amendinginvoice);
        }

        public void AddDirectDebitMandate(DirectDebitMandate directDebitMandate)
        {
            directDebitmandates.Add(directDebitMandate.InternalReferenceNumber, directDebitMandate);
        }

        private string GetFullname()
        {
            return (name + " " + firstSurname + " " + (secondSurname ?? "")).Trim();
        }

        private void InitializeMemberInstance(string givenName, string firstSurname, string secondSurname)
        {
            InitializePersonalData(givenName, firstSurname, secondSurname);
            InitializeBillingData();
        }

        private void InitializePersonalData(string givenName, string firstSurname, string secondSurname)
        {
            if ((givenName ?? "").Trim() == "") throw new ArgumentException("Club Member name cannot be empty", "name");
            if ((firstSurname ?? "").Trim() == "") throw new ArgumentException("Club Member first surname cannot be empty", "firstSurname");
            this.name = givenName.Trim();
            this.firstSurname = firstSurname.Trim();
            this.secondSurname = (secondSurname ?? "").Trim();
        }

        private void InitializeBillingData()
        {
            this.defaultPaymentMethod = new CashPaymentMethod();
            directDebitmandates = new Dictionary<int, DirectDebitMandate>();
            invoicesList = new Dictionary<string, Invoice>();
            proFormaInvoicesList = new Dictionary<string, ProFormaInvoice>();
            amendingInvoicesList = new Dictionary<string, AmendingInvoice>();
        }

        private string GetMemberID()
        {
            clubMemberDataManager = ClubMemberDataManager.Instance;
            string formatedMemeberID = clubMemberDataManager.AssingnMemberIDSequenceNumber().ToString("00000");
            return formatedMemeberID;
        }
    }
}
