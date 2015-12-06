using System;
using System.Linq;
using RCNGCMembersManagementAppLogic.Billing;

namespace RCNGCMembersManagementAppLogic
{
    public class BillsManager
    {
        public void PayBill(Invoice invoiceContainingTheBill, Bill billToPay, Payment payment)
        {
            billToPay.PayBill(payment);
            CheckIfAgreementIsAccomplished(invoiceContainingTheBill, billToPay);
            invoiceContainingTheBill.CheckIfInvoiceIsFullPaid();
        }

        public void CheckDueDate(Invoice invoiceContainingTheBill, Bill billToCheck, DateTime todayDate)
        {
            bool itWasToCollect = billToCheck.PaymentResult == Bill.BillPaymentResult.ToCollect;
            billToCheck.CheckDueDate(todayDate);
            bool itIsUnpaid = billToCheck.PaymentResult == Bill.BillPaymentResult.Unpaid;
            if (itWasToCollect && itIsUnpaid) invoiceContainingTheBill.SetInvoiceAsUnpaid();
        }

        public void RenewBillDueDate(Invoice invoiceContainingTheBill, Bill billToRenew, DateTime newDueDate, DateTime todayDate)
        {
            billToRenew.RenewDueDate(newDueDate, todayDate);
            invoiceContainingTheBill.SetInvoiceToBePaidIfHasNoUnpaidBills();
        }

        private void CheckIfAgreementIsAccomplished(Invoice invoiceContainingTheBill, Bill paidBill)
        {
            PaymentAgreement activeAgreement = GetActiveAgreementFromBill(paidBill);
            if (activeAgreement!=null)
            {
                DateTime agreementDate = activeAgreement.AgreementDate;
                var billsIncludedInTheAgreement = invoiceContainingTheBill.Bills.Values.Where(bill => bill.PaymentAgreements.ContainsKey(agreementDate));
                var unpaidBillsFromAgreement = billsIncludedInTheAgreement.Where(bill => bill.PaymentResult == Bill.BillPaymentResult.ToCollect);
                if (unpaidBillsFromAgreement.Count() == 0)
                {
                    foreach (Bill bill in billsIncludedInTheAgreement)
                        bill.PaymentAgreements[agreementDate].PaymentAgreementActualStatus = PaymentAgreement.PaymentAgreementStatus.Accomplished;
                    invoiceContainingTheBill.PaymentAgreements[agreementDate].PaymentAgreementActualStatus = PaymentAgreement.PaymentAgreementStatus.Accomplished;
                }
            }
        }

        private PaymentAgreement GetActiveAgreementFromBill(Bill bill)
        {
            if (bill.PaymentAgreements.Count == 0) return null;
            PaymentAgreement activePaymentAgreement = 
                bill.PaymentAgreements.First(keyValuePair => keyValuePair.Value.PaymentAgreementActualStatus == PaymentAgreement.PaymentAgreementStatus.Active).Value;
            return activePaymentAgreement;
        }
    }
}
