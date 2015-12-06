using System;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class Payment
    {
        decimal paymentAmount;
        DateTime paymentDate;
        PaymentMethod paymentMethod;

        public Payment(decimal paymentAmount, DateTime paymentDate, PaymentMethod paymentMethod)
        {
            this.paymentAmount = paymentAmount;
            this.paymentDate = paymentDate;
            this.paymentMethod = paymentMethod;
        }

        public decimal PaymentAmount
        {
            get { return paymentAmount; }
        }

        public DateTime PaymentDate
        {
            get { return paymentDate; }
        }

        public PaymentMethod PaymentMethod
        {
            get { return paymentMethod; }
        }
    }
}
