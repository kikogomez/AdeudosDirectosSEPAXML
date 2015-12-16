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
    public partial class ReferredDocumentType2
    {

        private ReferredDocumentType1Choice cdOrPrtryField;

        private string issrField;

        /// <comentarios/>
        public ReferredDocumentType1Choice CdOrPrtry
        {
            get
            {
                return this.cdOrPrtryField;
            }
            set
            {
                this.cdOrPrtryField = value;
            }
        }

        /// <comentarios/>
        public string Issr
        {
            get
            {
                return this.issrField;
            }
            set
            {
                this.issrField = value;
            }
        }
    }
}
