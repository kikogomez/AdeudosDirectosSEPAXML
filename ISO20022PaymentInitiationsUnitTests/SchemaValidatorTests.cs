using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISO20022PaymentInitiations;
using XMLSerializerValidator;
using System.IO;

namespace ISO20022PaymentInitiationsUnitTests
{
    [TestClass]
    public class SchemaValidatorTests
    {
        [TestMethod]
        public void ISO20020XMLExampleFileIsWellValidatedThroughXSD()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"XML Test Files\pain.002.001.03\pain.002.001.03_2.xml";

            string validatingErrors = SchemaValidators.ValidatePaymentStatusReportFile(xMLFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void LaCaixaXMLExampleFileIsWellValidatedThroughXSD()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example1.xml";

            string validatingErrors = SchemaValidators.ValidatePaymentStatusReportFile(xMLFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void XmlFileWithNonUnicodeCharactersIsWellValidated()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"F:\Gestion\devoluciones\2016\Febrero\Bankia\descarga_fichero_2016_02_18_1010.xml";

            string validatingErrors = SchemaValidators.ValidatePaymentStatusReportFile(xMLFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void NonCompilantISO20020XMLExample2FileHasValidationErrors()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"XML Test Files\pain.002.001.03\pain.002.001.03_2(NotCompilant).xml";

            string validatingErrors = SchemaValidators.ValidatePaymentStatusReportFile(xMLFilePath);
            Assert.IsFalse(validatingErrors=="");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Xml.XmlException))]
        public void ErroneousFormatXMLExample3FileRaisesAnExceptioWhenValidated()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"XML Test Files\pain.002.001.03\pain.002.001.03_2(ErroneousFormat).xml";

            try
            {
                string validatingErrors = SchemaValidators.ValidatePaymentStatusReportFile(xMLFilePath);
            }
            catch(System.Xml.XmlException notValidXMLFileException)
            {
                Assert.IsNotNull(notValidXMLFileException.Message);
                throw;
            }            
        }

        [TestMethod]
        public void ISO20020XMLExampleStringIsWellValidatedThroughXSD()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"XML Test Files\pain.002.001.03\pain.002.001.03_2.xml";
            string xMLString = File.ReadAllText(xMLFilePath);

            string validatingErrors = SchemaValidators.ValidatePaymentStatusReportString(xMLString);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void NonCompilantISO20020XMLExample2StringHasValidationErrors()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"XML Test Files\pain.002.001.03\pain.002.001.03_2(NotCompilant).xml";
            string xMLString = File.ReadAllText(xMLFilePath);

            string validatingErrors = SchemaValidators.ValidatePaymentStatusReportString(xMLString);
            Assert.IsFalse(validatingErrors == "");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Xml.XmlException))]
        public void ErroneousFormatXMLExample3StringRaisesAnExceptioWhenValidated()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"XML Test Files\pain.002.001.03\pain.002.001.03_2(ErroneousFormat).xml";
            string xMLString = File.ReadAllText(xMLFilePath);

            try
            {
                string validatingErrors = SchemaValidators.ValidatePaymentStatusReportString(xMLString);
            }
            catch (System.Xml.XmlException notValidXMLFileException)
            {
                Assert.IsNotNull(notValidXMLFileException.Message);
                throw;
            }
        }
    }
}
