using System;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class PaymentAgreement
    {
        string authorizingPerson;
        string agreementTerms;
        DateTime agreementDate;
        PaymentAgreementStatus paymentAgreementActualStatus;

        public PaymentAgreement(string authorizingPerson, string agreementTerms, DateTime agreementDate)
        {
            this.authorizingPerson = authorizingPerson;
            this.agreementTerms = agreementTerms;
            this.agreementDate = agreementDate;
            paymentAgreementActualStatus = PaymentAgreementStatus.Active;
        }

        public enum PaymentAgreementStatus { Active, Accomplished, NotAcomplished, Renegotiated };

        public string AuthorizingPerson
        {
            get { return authorizingPerson; }
        }

        public string AgreementTerms
        {
            get { return agreementTerms; }
        }

        public DateTime AgreementDate
        {
            get { return agreementDate; }
        }
        public PaymentAgreementStatus PaymentAgreementActualStatus
        {
            get { return paymentAgreementActualStatus; }
            set { paymentAgreementActualStatus = value; }
        }
    }
}
