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
    public partial class OrganisationIdentification4
    {

        private string bICOrBEIField;

        private GenericOrganisationIdentification1[] othrField;

        /// <comentarios/>
        public string BICOrBEI
        {
            get
            {
                return this.bICOrBEIField;
            }
            set
            {
                this.bICOrBEIField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Othr")]
        public GenericOrganisationIdentification1[] Othr
        {
            get
            {
                return this.othrField;
            }
            set
            {
                this.othrField = value;
            }
        }
    }

}
