using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public partial class NumberOfTransactionsPerStatus3
    {

        private string dtldNbOfTxsField;

        private TransactionIndividualStatus3Code dtldStsField;

        private decimal dtldCtrlSumField;

        private bool dtldCtrlSumFieldSpecified;

        /// <comentarios/>
        public string DtldNbOfTxs
        {
            get
            {
                return this.dtldNbOfTxsField;
            }
            set
            {
                this.dtldNbOfTxsField = value;
            }
        }

        /// <comentarios/>
        public TransactionIndividualStatus3Code DtldSts
        {
            get
            {
                return this.dtldStsField;
            }
            set
            {
                this.dtldStsField = value;
            }
        }

        /// <comentarios/>
        public decimal DtldCtrlSum
        {
            get
            {
                return this.dtldCtrlSumField;
            }
            set
            {
                this.dtldCtrlSumField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DtldCtrlSumSpecified
        {
            get
            {
                return this.dtldCtrlSumFieldSpecified;
            }
            set
            {
                this.dtldCtrlSumFieldSpecified = value;
            }
        }
    }
}
