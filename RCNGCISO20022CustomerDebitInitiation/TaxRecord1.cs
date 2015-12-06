namespace RCNGCISO20022CustomerDebitInitiation
{
/// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class TaxRecord1
    {
        private string tpField;
        private string ctgyField;
        private string ctgyDtlsField;
        private string dbtrStsField;
        private string certIdField;
        private string frmsCdField;
        private TaxPeriod1 prdField;
        private TaxAmount1 taxAmtField;
        private string addtlInfField;

        //Parameterless constructor for Serialization purpose
        private TaxRecord1() { }

        public TaxRecord1(
            string type,
            string category,
            string categoryDetails,
            string debtorStatus,
            string certificateIdentification,
            string formsCode,
            TaxPeriod1 period,
            TaxAmount1 taxAmount,
            string additionalInformation)
        {
            this.tpField = type;
            this.ctgyField = category;
            this.ctgyDtlsField = categoryDetails;
            this.dbtrStsField = debtorStatus;
            this.certIdField = certificateIdentification;
            this.frmsCdField = formsCode;
            this.prdField = period;
            this.taxAmtField = taxAmount;
            this.addtlInfField = additionalInformation;
        }

        /// <comentarios/>
        public string Tp
        {
            get
            {
                return this.tpField;
            }
            set
            {
                this.tpField = value;
            }
        }

        /// <comentarios/>
        public string Ctgy
        {
            get
            {
                return this.ctgyField;
            }
            set
            {
                this.ctgyField = value;
            }
        }

        /// <comentarios/>
        public string CtgyDtls
        {
            get
            {
                return this.ctgyDtlsField;
            }
            set
            {
                this.ctgyDtlsField = value;
            }
        }

        /// <comentarios/>
        public string DbtrSts
        {
            get
            {
                return this.dbtrStsField;
            }
            set
            {
                this.dbtrStsField = value;
            }
        }

        /// <comentarios/>
        public string CertId
        {
            get
            {
                return this.certIdField;
            }
            set
            {
                this.certIdField = value;
            }
        }

        /// <comentarios/>
        public string FrmsCd
        {
            get
            {
                return this.frmsCdField;
            }
            set
            {
                this.frmsCdField = value;
            }
        }

        /// <comentarios/>
        public TaxPeriod1 Prd
        {
            get
            {
                return this.prdField;
            }
            set
            {
                this.prdField = value;
            }
        }

        /// <comentarios/>
        public TaxAmount1 TaxAmt
        {
            get
            {
                return this.taxAmtField;
            }
            set
            {
                this.taxAmtField = value;
            }
        }

        /// <comentarios/>
        public string AddtlInf
        {
            get
            {
                return this.addtlInfField;
            }
            set
            {
                this.addtlInfField = value;
            }
        }
    }
}
