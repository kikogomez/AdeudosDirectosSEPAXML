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
    public partial class ContactDetails2
    {

        private NamePrefix1Code nmPrfxField;

        private bool nmPrfxFieldSpecified;

        private string nmField;

        private string phneNbField;

        private string mobNbField;

        private string faxNbField;

        private string emailAdrField;

        private string othrField;

        /// <comentarios/>
        public NamePrefix1Code NmPrfx
        {
            get
            {
                return this.nmPrfxField;
            }
            set
            {
                this.nmPrfxField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NmPrfxSpecified
        {
            get
            {
                return this.nmPrfxFieldSpecified;
            }
            set
            {
                this.nmPrfxFieldSpecified = value;
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
        public string PhneNb
        {
            get
            {
                return this.phneNbField;
            }
            set
            {
                this.phneNbField = value;
            }
        }

        /// <comentarios/>
        public string MobNb
        {
            get
            {
                return this.mobNbField;
            }
            set
            {
                this.mobNbField = value;
            }
        }

        /// <comentarios/>
        public string FaxNb
        {
            get
            {
                return this.faxNbField;
            }
            set
            {
                this.faxNbField = value;
            }
        }

        /// <comentarios/>
        public string EmailAdr
        {
            get
            {
                return this.emailAdrField;
            }
            set
            {
                this.emailAdrField = value;
            }
        }

        /// <comentarios/>
        public string Othr
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
