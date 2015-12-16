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
    public partial class RemittanceAmount1
    {

        private ActiveOrHistoricCurrencyAndAmount duePyblAmtField;

        private ActiveOrHistoricCurrencyAndAmount dscntApldAmtField;

        private ActiveOrHistoricCurrencyAndAmount cdtNoteAmtField;

        private ActiveOrHistoricCurrencyAndAmount taxAmtField;

        private DocumentAdjustment1[] adjstmntAmtAndRsnField;

        private ActiveOrHistoricCurrencyAndAmount rmtdAmtField;

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount DuePyblAmt
        {
            get
            {
                return this.duePyblAmtField;
            }
            set
            {
                this.duePyblAmtField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount DscntApldAmt
        {
            get
            {
                return this.dscntApldAmtField;
            }
            set
            {
                this.dscntApldAmtField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount CdtNoteAmt
        {
            get
            {
                return this.cdtNoteAmtField;
            }
            set
            {
                this.cdtNoteAmtField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount TaxAmt
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
        [System.Xml.Serialization.XmlElementAttribute("AdjstmntAmtAndRsn")]
        public DocumentAdjustment1[] AdjstmntAmtAndRsn
        {
            get
            {
                return this.adjstmntAmtAndRsnField;
            }
            set
            {
                this.adjstmntAmtAndRsnField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount RmtdAmt
        {
            get
            {
                return this.rmtdAmtField;
            }
            set
            {
                this.rmtdAmtField = value;
            }
        }
    }
}
