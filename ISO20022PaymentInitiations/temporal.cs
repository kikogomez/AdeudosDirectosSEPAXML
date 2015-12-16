namespace ISO20022PaymentInitiations
{
    [System.SerializableAttribute()]
    public class Foo
    {
        private string myField;
        private bool myFieldSerialzes;

        public Foo() { }

        public Foo(string myField, bool myFieldDeserializes)
        {
            this.myField = myField;
            this.myFieldSerialzes = myFieldDeserializes;
        }

        public string MyField
        {
            get {return this.myField;}
            set {this.myField = value;}
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MyFieldDeserializes
        {
            get {return this.myFieldSerialzes;}
            set {this.myFieldSerialzes = value;}
        }
    }
}
