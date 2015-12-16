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
    public partial class EquivalentAmount2
    {

        private ActiveOrHistoricCurrencyAndAmount amtField;

        private string ccyOfTrfField;

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
        public string CcyOfTrf
        {
            get
            {
                return this.ccyOfTrfField;
            }
            set
            {
                this.ccyOfTrfField = value;
            }
        }
    }
}
