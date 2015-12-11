using System;
using System.Collections.Generic;
using DirectDebitElements;
using Billing;

namespace DirectDebitElementsUnitTests
{
    public class ClubMember_POCOTestClass
    {
        string memberID;
        string name;
        string firstSurname;
        string secondSurname;
        PaymentMethod defaultPaymentMethod;
        Dictionary<int, DirectDebitMandate> directDebitmandates;
        Dictionary<string, SimplifiedBill> simplifiedBills;

        public ClubMember_POCOTestClass(string memberID, string givenName, string firstSurname, string secondSurname)
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

        public Dictionary<string, SimplifiedBill> SimplifiedBills
        {
            get { return simplifiedBills; }
        }

        public void AddDirectDebitMandate(DirectDebitMandate directDebitMandate)
        {
            directDebitmandates.Add(directDebitMandate.InternalReferenceNumber, directDebitMandate);
        }

        public void AddSimplifiedBill(SimplifiedBill simplifiedBill)
        {
            simplifiedBills.Add(simplifiedBill.BillID, simplifiedBill);
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
            simplifiedBills = new Dictionary<string, SimplifiedBill>();

        }
    }
}
