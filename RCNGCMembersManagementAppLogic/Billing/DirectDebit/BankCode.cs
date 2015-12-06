using System.Xml.Serialization;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    [System.Serializable()]
    public class BankCode
    {
        string localBankCode;
        string name;
        string bIC;

        //Paramenterless constructor for serialization purposes
        private BankCode() { }

        public BankCode(string localBankCode, string name, string bIC)
        {
            this.localBankCode = localBankCode;
            this.name = name;
            this.bIC = bIC;
        }

        [XmlElement("LocalBankCode")]
        public string LocalBankCode
        {
            get { return localBankCode; }
            set { localBankCode = value; }
        }

        [XmlElement("BankName")]
        public string BankName
        {
            get { return name; }
            set { name = value; }
        }
        [XmlElement("BIC")]
        public string BankBIC
        {
            get { return bIC; }
            set { bIC = value; }
        }
    }
}
