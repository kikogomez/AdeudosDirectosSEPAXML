using RCNGCMembersManagementAppLogic.Billing.DirectDebit;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class BankTransferPaymentMethod : PaymentMethod
    {
        BankAccount transferorAccount;
        BankAccount transfereeAccount;

        public BankTransferPaymentMethod(BankAccount transferorAccount, BankAccount transfereeAccount)
            : base()
        {
            this.transferorAccount = transferorAccount;
            this.transfereeAccount = transfereeAccount;
        }

        public BankAccount TransferorAccount
        {
            get { return transferorAccount; }
        }

        public BankAccount TransfereeAccount
        {
            get { return transfereeAccount; }
        }
    }
}
