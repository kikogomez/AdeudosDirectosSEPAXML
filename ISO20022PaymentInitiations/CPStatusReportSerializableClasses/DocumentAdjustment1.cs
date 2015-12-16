using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISO20022PaymentInitiations.CPStatusReportSerializableClasses
{
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public partial class DocumentAdjustment1
    {

        private ActiveOrHistoricCurrencyAndAmount amtField;

        private CreditDebitCode cdtDbtIndField;

        private bool cdtDbtIndFieldSpecified;

        private string rsnField;

        private string addtlInfField;

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount Amt
        {
            get
            {
                return this.amtField;
            }
            set
            {
                this.amtField = value;
            }
        }

        /// <comentarios/>
        public CreditDebitCode CdtDbtInd
        {
            get
            {
                return this.cdtDbtIndField;
            }
            set
            {
                this.cdtDbtIndField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CdtDbtIndSpecified
        {
            get
            {
                return this.cdtDbtIndFieldSpecified;
            }
            set
            {
                this.cdtDbtIndFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public string Rsn
        {
            get
            {
                return this.rsnField;
            }
            set
            {
                this.rsnField = value;
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
