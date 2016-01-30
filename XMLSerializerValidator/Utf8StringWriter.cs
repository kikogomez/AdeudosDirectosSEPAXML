using System.Text;
using System.IO;

namespace XMLSerializerValidator
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}
