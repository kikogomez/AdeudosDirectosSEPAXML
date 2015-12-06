using System.Collections.Generic;
using System.Linq;
using System.IO;
using RCNGCMembersManagementAppLogic.XML;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class BankCodes
    {
        Dictionary<string, BankCode> bankDictionaryByLocalBankCode;
        Dictionary<string, BankCode> bankDictionaryByBIC;

        public BankCodes()
        {
            bankDictionaryByLocalBankCode = new Dictionary<string, BankCode>();
            bankDictionaryByBIC = new Dictionary<string, BankCode>();
        }

        public BankCodes(string bankBICCodesFilePath, BankCodesFileFormat bankCodeFileFormat)
            : this()
        {
            LoadBankCodes(bankBICCodesFilePath, bankCodeFileFormat);
        }

        public enum BankCodesFileFormat { XML, CSV };

        public Dictionary<string, BankCode> BankDictionaryByLocalBankCode
        {
            get { return bankDictionaryByLocalBankCode; }
        }

        public Dictionary<string, BankCode> BankDictionaryByBIC
        {
            get { return bankDictionaryByBIC; }
        }

        public void AddBankCodesToDictionaries(List<BankCode> bankCodes)
        {
            foreach (BankCode bankCode in bankCodes)
            {
                if (bankDictionaryByLocalBankCode.Keys.Contains(bankCode.LocalBankCode) || bankDictionaryByBIC.Keys.Contains(bankCode.BankBIC)) continue;
                bankDictionaryByLocalBankCode.Add(bankCode.LocalBankCode, bankCode);
                bankDictionaryByBIC.Add(bankCode.BankBIC, bankCode);
            }
        }

        public BankCode[] ReadBankBicCodesTabCSVFile(string bankBICCodesCSVFilePath)
        {
            string[] lines = File.ReadAllLines(bankBICCodesCSVFilePath);
            BankCode[] spanishBankCodes =
                lines.Select(line =>
                    new BankCode(
                        line.Split('\t').ToArray()[2],
                        line.Split('\t').ToArray()[1],
                        line.Split('\t').ToArray()[0])
                        ).ToArray();
            return spanishBankCodes;
        }

        public BankCode[] ReadBankBicCodesXMLFile(string bankBICCodesXMLFilePath)
        {
            BankCode[] spanishBankCodes =
                XMLSerializer.XMLDeserializeFromFile<BankCode[]>(bankBICCodesXMLFilePath, null, null);
            return spanishBankCodes;
        }

        private void LoadBankCodes(string bankBICCodesFilePath, BankCodesFileFormat bankCodeFileFormat)
        {
            switch (bankCodeFileFormat)
            {
                case BankCodesFileFormat.CSV:
                    AddBankCodesToDictionaries(ReadBankBicCodesTabCSVFile(bankBICCodesFilePath).ToList());
                    break;
                case BankCodesFileFormat.XML:
                    AddBankCodesToDictionaries(ReadBankBicCodesXMLFile(bankBICCodesFilePath).ToList());
                    break;
            }
        }
    }
}
