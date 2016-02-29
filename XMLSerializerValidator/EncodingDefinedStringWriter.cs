using System.Text;
using System.IO;

namespace XMLSerializerValidator
{
    public class EncodingDefinedStringWriter: StringWriter
    {
        Encoding encoding;

        public EncodingDefinedStringWriter(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding { get { return encoding; } }
    }
}
