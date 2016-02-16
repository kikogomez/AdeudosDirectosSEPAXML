using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEPAXMLPaymentStatusReportReader;

namespace SEPAXMLPaymentStatusReportReaderTests
{
    [TestClass]
    public class PaymentStatusReportReadingTests
    {
        [TestMethod]
        public void ValidPathsCausesNoError()
        {
            string pathToXMLFile = @"TestFiles\pain.002.001.03_2.xml";
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
            string pathToXMLFile = @"TestFiles\pain.xml";
            string pathToFileDataBase = @"TestFiles\TestMDB.mdb";
            string pathError = "";
            int exitCode = 0;
            bool pathToFilesAreCorrect;

            MainInstance mainInstance = new MainInstance();
            pathToFilesAreCorrect = mainInstance.PathsToFilesAreCorrect(pathToXMLFile, pathToFileDataBase, out pathError, out exitCode);

            string expectedError = @"TestFiles\pain.xml" + Environment.NewLine + "File not found!";
            Assert.AreEqual(false, pathToFilesAreCorrect);
            Assert.AreEqual(expectedError, pathError);
            Assert.AreEqual((int)ExitCodes.InvalidPaymentStatusFilePath, exitCode);
        }

        [TestMethod]
        public void InvalidDataBasePathCausesError()
        {
            string pathToXMLFile = @"TestFiles\pain.002.001.03_2.xml";
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


    }
}
