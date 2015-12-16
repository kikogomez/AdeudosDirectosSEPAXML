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
    public partial class FinancialInstitutionIdentification7
    {

        private string bICField;

        private ClearingSystemMemberIdentification2 clrSysMmbIdField;

        private string nmField;

        private PostalAddress6 pstlAdrField;

        private GenericFinancialIdentification1 othrField;

        /// <comentarios/>
        public string BIC
        {
            get
            {
                return this.bICField;
            }
            set
            {
                this.bICField = value;
            }
        }

        /// <comentarios/>
        public ClearingSystemMemberIdentification2 ClrSysMmbId
        {
            get
            {
                return this.clrSysMmbIdField;
            }
            set
            {
                this.clrSysMmbIdField = value;
            }
        }

        /// <comentarios/>
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        /// <comentarios/>
        public PostalAddress6 PstlAdr
        {
            get
            {
                return this.pstlAdrField;
            }
            set
            {
                this.pstlAdrField = value;
            }
        }

        /// <comentarios/>
        public GenericFinancialIdentification1 Othr
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
