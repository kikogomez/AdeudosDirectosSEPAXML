using System;
using System.Collections.Generic;
using DirectDebitElements;

namespace Billing
{
    public class Debtor
    {
        string debtorID;
        string name;
        string firstSurname;
        string secondSurname;
        PaymentMethod defaultPaymentMethod;
        Dictionary<int, DirectDebitMandate> directDebitmandates;
        Dictionary<string, SimplifiedBill> simplifiedBills;

        public Debtor(string memberID, string givenName, string firstSurname, string secondSurname)
        {
            InitializeDebtorInstance(givenName, firstSurname, secondSurname);
            this.debtorID = memberID;
        }

        public string DebtorID
        {
            get { return debtorID; }
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
            if (DirectDebitmandates.ContainsKey(directDebitMandate.InternalReferenceNumber))
            {
                directDebitmandates.Remove(directDebitMandate.InternalReferenceNumber);
            }
            directDebitmandates.Add(directDebitMandate.InternalReferenceNumber, directDebitMandate);
        }

        public void AddSimplifiedBill(SimplifiedBill simplifiedBill)
        {
            if(SimplifiedBills.ContainsKey(simplifiedBill.BillID))
            {
                throw new ArgumentException("The billID already exists", "billID");
            }
            simplifiedBills.Add(simplifiedBill.BillID, simplifiedBill);
        }

        public void SetDefaultPaymentMethod(PaymentMethod paymentMethod)
        {
            this.defaultPaymentMethod = paymentMethod;
            if (paymentMethod.GetType() == typeof(DirectDebitPaymentMethod))
            {
                AddDirectDebitMandate(((DirectDebitPaymentMethod)paymentMethod).DirectDebitMandate);
            }
        }

        private string GetFullname()
        {
            return (name + " " + firstSurname + " " + (secondSurname ?? "")).Trim();
        }

        private void InitializeDebtorInstance(string givenName, string firstSurname, string secondSurname)
        {
            InitializePersonalData(givenName, firstSurname, secondSurname);
            InitializeBillingData();
        }

        private void InitializePersonalData(string givenName, string firstSurname, string secondSurname)
        {
            if ((givenName ?? "").Trim() == "") throw new ArgumentException("Debtor name cannot be empty", "name");
            if ((firstSurname ?? "").Trim() == "") throw new ArgumentException("Debtor first surname cannot be empty", "firstSurname");
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
