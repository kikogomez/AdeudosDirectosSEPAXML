using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEPAXMLPaymentStatusReportReader;
using DirectDebitElements;
using System.IO;
using ExtensionMethods;

namespace SEPAXMLPaymentStatusReportReaderTests
{
    [TestClass]
    public class PaymentStatusReportReadingTests
    {
        [TestMethod]
        public void ValidPathsCausesNoError()
        {
            string pathToXMLFile = @"TestFiles\XML Test Files\pain.002.001.03_2.xml";
            string pathToFileDataBase = @"TestFiles\TestMDB.mdb";
            string pathErrors = "";
            int exitCode = 0;
            bool pathsHaveErrros;

            MainInstance mainInstance = new MainInstance();
            pathsHaveErrros = mainInstance.PathsToFilesAreCorrect(pathToXMLFile, pathToFileDataBase, out pathErrors, out exitCode);

            Assert.AreEqual(true, pathsHaveErrros);
            Assert.AreEqual("", pathErrors);
            Assert.AreEqual(0, exitCode);
        }

        [TestMethod]
        public void InvalidXMLFilePathCausesError()
        {
            string pathToXMLFile = @"TestFiles\XML Test Files\pain.xml";
            string pathToFileDataBase = @"TestFiles\TestMDB.mdb";
            string pathError = "";
            int exitCode = 0;
            bool pathToFilesAreCorrect;

            MainInstance mainInstance = new MainInstance();
            pathToFilesAreCorrect = mainInstance.PathsToFilesAreCorrect(pathToXMLFile, pathToFileDataBase, out pathError, out exitCode);

            string expectedError = @"TestFiles\XML Test Files\pain.xml" + Environment.NewLine + "File not found!";
            Assert.AreEqual(false, pathToFilesAreCorrect);
            Assert.AreEqual(expectedError, pathError);
            Assert.AreEqual((int)ExitCodes.InvalidPaymentStatusFilePath, exitCode);
        }

        [TestMethod]
        public void InvalidDataBasePathCausesError()
        {
            string pathToXMLFile = @"TestFiles\XML Test Files\pain.002.001.03_2.xml";
            string pathToFileDataBase = @"TestFiles\Test.mdb";
            string pathError = "";
            int exitCode = 0;
            bool pathsHaveErrros;

            MainInstance mainInstance = new MainInstance();
            pathsHaveErrros = mainInstance.PathsToFilesAreCorrect(pathToXMLFile, pathToFileDataBase, out pathError, out exitCode);

            string expectedError = @"TestFiles\Test.mdb" + Environment.NewLine + "File not found!";
            Assert.AreEqual(false, pathsHaveErrros);
            Assert.AreEqual(expectedError, pathError);
            Assert.AreEqual((int)ExitCodes.InvalidDataBasePath, exitCode);
        }

        [TestMethod]
        public void InvalidCharactersInPathCausesError()
        {
            string pathToXMLFile = @"TestFiles\XML Test Files\pa<in.002.001.03_2.xml";
            string pathToFileDataBase = @"TestFiles\TestMDB.mdb";
            string pathError = "";
            int exitCode = 0;
            bool pathToFilesAreCorrect;

            MainInstance mainInstance = new MainInstance();
            pathToFilesAreCorrect = mainInstance.PathsToFilesAreCorrect(pathToXMLFile, pathToFileDataBase, out pathError, out exitCode);

            string expectedError = @"TestFiles\XML Test Files\pa<in.002.001.03_2.xml" + Environment.NewLine + "Path to file contains invalid characters";
            Assert.AreEqual(false, pathToFilesAreCorrect);
            Assert.AreEqual(expectedError, pathError);
            Assert.AreEqual((int)ExitCodes.InvalidPaymentStatusFilePath, exitCode);
        }

        [TestMethod]
        public void NoFileSpecifiedCausesError()
        {
            string pathToXMLFile = @"TestFiles\XML Test Files\";
            string pathToFileDataBase = @"TestFiles\TestMDB.mdb";
            string pathError = "";
            int exitCode = 0;
            bool pathToFilesAreCorrect;

            MainInstance mainInstance = new MainInstance();
            pathToFilesAreCorrect = mainInstance.PathsToFilesAreCorrect(pathToXMLFile, pathToFileDataBase, out pathError, out exitCode);

            string expectedError = @"TestFiles\XML Test Files\" + Environment.NewLine + "No file specified in path";
            Assert.AreEqual(false, pathToFilesAreCorrect);
            Assert.AreEqual(expectedError, pathError);
            Assert.AreEqual((int)ExitCodes.InvalidPaymentStatusFilePath, exitCode);
        }

        [TestMethod]
        public void ACorrectXMLPaymentStatusReportStringParsesOK()
        {
            string xMLFilePath = @"TestFiles\XML Test Files\LaCaixa_pain00200103_Example2.xml";
            string xmlString = File.ReadAllText(xMLFilePath);

            MainInstance mainInstance = new MainInstance();
            PaymentStatusReport paymentStatusReport = mainInstance.ParsePaymentStatusReportString(xmlString);
            
            //General info from file
            DateTime expectedMessageCreationDate = DateTime.Parse("2015-12-08T06:00:01");
            DateTime expectedRejectAccountChargeDateTime = DateTime.Parse("2015-12-08");
            Assert.AreEqual("DAG35008770033201512080511490000677", paymentStatusReport.MessageID);
            Assert.AreEqual(expectedMessageCreationDate, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(expectedRejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(4, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual((decimal)1006.65, paymentStatusReport.ControlSum);
            Assert.AreEqual(3, paymentStatusReport.DirectDebitPaymentInstructionRejects.Count);

            //Info from first DirectDectDebitRemittance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList1 = new List<string>()
            {"15M/025450120151203", "15M/025720120151203"};
            Assert.AreEqual("2015-12-0112205515Rem.150 Ord.1", paymentStatusReport.DirectDebitPaymentInstructionRejects[0].OriginalPaymentInformationID);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].NumberOfTransactions);
            Assert.AreEqual((decimal)657.73, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList1, paymentStatusReport.DirectDebitPaymentInstructionRejects[0].OriginalEndtoEndTransactiontransactionIDList);

            //Info from second DirectDectDebitRemittance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList2 = new List<string>()
            {"15M/022581120151204"};
            Assert.AreEqual("2015-12-0113442815Rem.151 Ord.1", paymentStatusReport.DirectDebitPaymentInstructionRejects[1].OriginalPaymentInformationID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)277.45, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].OriginalEndtoEndTransactiontransactionIDList);

            //Info from third DirectDectDebitRemittance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList3 = new List<string>()
            {"15M/026530120151204"};
            Assert.AreEqual("2015-12-0115153115Rem.152 Ord.1", paymentStatusReport.DirectDebitPaymentInstructionRejects[2].OriginalPaymentInformationID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)71.47, paymentStatusReport.DirectDebitPaymentInstructionRejects[2].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitPaymentInstructionRejects[1].OriginalEndtoEndTransactiontransactionIDList);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Xml.XmlException))]
        public void AnInvalidXMLStringThrowsAnXmlExceptionWhenIsParsed()
        {
            string xMLFilePath = @"TestFiles\XML Test Files\pain.002.001.03_2(ErroneousFormat).xml";
            string xmlString = File.ReadAllText(xMLFilePath);
            MainInstance mainInstance = new MainInstance();
            try
            {
                PaymentStatusReport paymentStatusReport = mainInstance.ParsePaymentStatusReportString(xmlString);
            }
            catch (System.Xml.XmlException)
            {
                Assert.IsTrue(true);    //Just assert an exception is trown
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.Xml.Schema.XmlSchemaValidationException))]
        public void ANonCompilantXMLStringThrowsAnXmlSchemaValidationExceptionWhenIsParsed()
        {
            string xMLFilePath = @"TestFiles\XML Test Files\pain.002.001.03_2(NotCompilant).xml";
            string xmlString = File.ReadAllText(xMLFilePath);
            MainInstance mainInstance = new MainInstance();
            try
            {
                PaymentStatusReport paymentStatusReport = mainInstance.ParsePaymentStatusReportString(xmlString);
            }
            catch (System.Xml.Schema.XmlSchemaValidationException invalidXMLFormatException)
            {
                Assert.AreEqual("ERROR:", invalidXMLFormatException.Message.Left(6));
                throw;
            }
        }
    }
}
