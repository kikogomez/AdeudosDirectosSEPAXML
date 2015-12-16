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
    public partial class DateAndPlaceOfBirth
    {

        private System.DateTime birthDtField;

        private string prvcOfBirthField;

        private string cityOfBirthField;

        private string ctryOfBirthField;

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime BirthDt
        {
            get
            {
                return this.birthDtField;
            }
            set
            {
                this.birthDtField = value;
            }
        }

        /// <comentarios/>
        public string PrvcOfBirth
        {
            get
            {
                return this.prvcOfBirthField;
            }
            set
            {
                this.prvcOfBirthField = value;
            }
        }

        /// <comentarios/>
        public string CityOfBirth
        {
            get
            {
                return this.cityOfBirthField;
            }
            set
            {
                this.cityOfBirthField = value;
            }
        }

        /// <comentarios/>
        public string CtryOfBirth
        {
            get
            {
                return this.ctryOfBirthField;
            }
            set
            {
                this.ctryOfBirthField = value;
            }
        }
    }
}
