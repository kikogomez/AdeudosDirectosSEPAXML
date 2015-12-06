using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class BankCodesUnitTests
    {
        [TestMethod]
        public void BankCodesCSVFileIsWellReaded()
        {
            BankCodes bankCodes = new BankCodes();
            BankCode[] spanisBankCodes = bankCodes.ReadBankBicCodesTabCSVFile(@"CSVFiles\SpanishBankCodes.csv");
            Assert.AreEqual(spanisBankCodes[0].BankBIC, "ABNAESMMXXX");
            Assert.AreEqual(spanisBankCodes[0].BankName, "THE ROYAL BANK OF SCOTLAND PLC, SUCURSAL EN ESPAÑA.");
            Assert.AreEqual(spanisBankCodes[0].LocalBankCode, "0156");
            Assert.AreEqual(spanisBankCodes[1].BankBIC, "AHCFESMMXXX");
        }

        [TestMethod]
        public void BankCodesXMLFileIsWellReaded()
        {
            BankCodes bankCodes = new BankCodes();
            BankCode[] spanisBankCodes = bankCodes.ReadBankBicCodesXMLFile(@"XMLFiles\SpanishBankCodes.xml");
            Assert.AreEqual(spanisBankCodes[0].BankBIC, "ABNAESMMXXX");
            Assert.AreEqual(spanisBankCodes[0].BankName, "THE ROYAL BANK OF SCOTLAND PLC, SUCURSAL EN ESPAÑA.");
            Assert.AreEqual(spanisBankCodes[0].LocalBankCode, "0156");
            Assert.AreEqual(spanisBankCodes[1].BankBIC, "AHCFESMMXXX");
        }

        [TestMethod]
        public void EmptyBankDictionaryByLocalBankCodeIsCreatedWhenInstantiatingSEPAIdentificatorCodes()
        {
            BankCodes bankCodes = new BankCodes();
            Assert.AreEqual(0, bankCodes.BankDictionaryByLocalBankCode.Count);
        }

        [TestMethod]
        public void EmptyBankDictionaryByBICIsCreatedWhenInstantiatingSEPAIdentificatorCodes()
        {
            BankCodes bankCodes = new BankCodes();
            Assert.AreEqual(0, bankCodes.BankDictionaryByBIC.Count);
        }

        [TestMethod]
        public void BankDictionaryByLocalBankCodeIsCorrectlyFilledWhenInstantiatingSEPAIdentificatorCodesWithCSVFile()
        {
            BankCodes bankCodesCSVInitialized =
                new BankCodes(@"CSVFiles\SpanishBankCodes.csv", BankCodes.BankCodesFileFormat.CSV);
            Assert.AreEqual("BANKINTER, S.A.", bankCodesCSVInitialized.BankDictionaryByLocalBankCode["0128"].BankName);
            Assert.AreEqual("CAIXESBBXXX", bankCodesCSVInitialized.BankDictionaryByLocalBankCode["2100"].BankBIC);
        }

        [TestMethod]
        public void BankDictionaryByBICIsCorrectlyFilledWhenInstantiatingSEPAIdentificatorCodesWithCSVFile()
        {
            BankCodes bankCodesCSVInitialized =
                new BankCodes(@"CSVFiles\SpanishBankCodes.csv", BankCodes.BankCodesFileFormat.CSV);
            Assert.AreEqual("BANKINTER, S.A.", bankCodesCSVInitialized.BankDictionaryByLocalBankCode["0128"].BankName);
            Assert.AreEqual("CAIXESBBXXX", bankCodesCSVInitialized.BankDictionaryByLocalBankCode["2100"].BankBIC);
        }

        [TestMethod]
        public void BankCodesCanBeAddedToTictionariesAfterInitialization()
        {
            BankCodes bankCodesEmpty = new BankCodes();
            List<BankCode> bankCodesList = new List<BankCode>();
            bankCodesList.Add(new BankCode("0128", "BANKINTER, S.A.", "BKBKESMMXXX"));
            bankCodesList.Add(new BankCode("2100", "CAIXABANK, S.A.", "CAIXESBBXXX"));
            bankCodesEmpty.AddBankCodesToDictionaries(bankCodesList);
            Assert.AreEqual(2, bankCodesEmpty.BankDictionaryByBIC.Count);
            Assert.AreEqual("CAIXESBBXXX", bankCodesEmpty.BankDictionaryByLocalBankCode["2100"].BankBIC);
        }

        [TestMethod]
        public void BankCodesWithDuplicatedValuesInLocalBankCodeOrBICAreNotAdded()
        {
            BankCodes bankCodesEmpty = new BankCodes();
            List<BankCode> banksList = new List<BankCode>();
            banksList.Add(new BankCode("0128", "BANKINTER, S.A.", "BKBKESMMXXX"));
            banksList.Add(new BankCode("2100", "CAIXABANK, S.A.", "CAIXESBBXXX"));
            bankCodesEmpty.AddBankCodesToDictionaries(banksList);
            Assert.AreEqual(2, bankCodesEmpty.BankDictionaryByBIC.Count);
            List<BankCode> listOfBanksToAdd = new List<BankCode>();
            BankCode bankWithDuplicatedLocalBankCode = new BankCode("0128", "DUPLICATED LOCAL BANK CODE", "DBNKCMMXXX");
            BankCode bankWithDuplicatedBIC = new BankCode("9009", "DUPLICATED BIC", "CAIXESBBXXX");
            BankCode corrrectBank = new BankCode("0156", "THE ROYAL BANK OF SCOTLAND PLC, SUCURSAL EN ESPAÑA.", "ABNAESMMXXX");
            listOfBanksToAdd.Add(bankWithDuplicatedLocalBankCode);
            listOfBanksToAdd.Add(bankWithDuplicatedBIC);
            listOfBanksToAdd.Add(corrrectBank);
            bankCodesEmpty.AddBankCodesToDictionaries(listOfBanksToAdd);
            Assert.AreEqual(3, bankCodesEmpty.BankDictionaryByBIC.Count);
            Assert.AreEqual("BKBKESMMXXX", bankCodesEmpty.BankDictionaryByLocalBankCode["0128"].BankBIC);
            Assert.AreEqual("ABNAESMMXXX", bankCodesEmpty.BankDictionaryByLocalBankCode["0156"].BankBIC);
        }
    }
}
