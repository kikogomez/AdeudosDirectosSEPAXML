using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using RCNGCMembersManagementAppLogic.XML;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class XMLValidationUnitTests
    {
        [TestMethod]
        public void AStringContainingAFullXMLDocumentIsWellValidatedThroughXSD()
        {
            //Original valid pain.008.001.02 XML string from ISO20022
            string xMLString = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<Document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
    <CstmrDrctDbtInitn>
		<GrpHdr>
			<MsgId>CAVAY1234</MsgId>
			<CreDtTm>2010-06-25T14:25:00</CreDtTm>
			<NbOfTxs>2</NbOfTxs>
			<CtrlSum>2010</CtrlSum>
			<InitgPty>
				<Nm>Virgay</Nm>
				<PstlAdr>
					<StrtNm>Virginia Lane</StrtNm>
					<BldgNb>36</BldgNb>
					<PstCd>NJ 07311</PstCd>
					<TwnNm>Jersey City</TwnNm>
					<Ctry>US</Ctry>
				</PstlAdr>
				<CtctDtls>
					<Nm>J. Thompson</Nm>
					<EmailAdr>Thompson@virgay.com</EmailAdr>
				</CtctDtls>
			</InitgPty>
		</GrpHdr>
		<PmtInf>
			<PmtInfId>CAVAY/88683</PmtInfId>
			<PmtMtd>DD</PmtMtd>
			<BtchBookg>false</BtchBookg>
			<ReqdColltnDt>2010-07-13</ReqdColltnDt>
			<Cdtr>
				<Nm>Virgay</Nm>
				<PstlAdr>
					<StrtNm>Virginia Lane</StrtNm>
					<BldgNb>36</BldgNb>
					<PstCd>NJ 07311</PstCd>
					<TwnNm>Jersey City</TwnNm>
					<Ctry>US</Ctry>
				</PstlAdr>
			</Cdtr>
			<CdtrAcct>
				<Id>
					<Othr>
						<Id>789123</Id>
					</Othr>
				</Id>
			</CdtrAcct>
			<CdtrAgt>
				<FinInstnId>
					<BIC>AAAAUS29</BIC>
				</FinInstnId>
			</CdtrAgt>
			<DrctDbtTxInf>
				<PmtId>
					<EndToEndId>VA09327/0123</EndToEndId>
				</PmtId>
				<PmtTpInf>
					<InstrPrty>NORM</InstrPrty>
					<SvcLvl>
						<Prtry>VERPA-1</Prtry>
					</SvcLvl>
					<SeqTp>RCUR</SeqTp>
				</PmtTpInf>
				<InstdAmt Ccy=""USD"">1025</InstdAmt>
				<ChrgBr>SHAR</ChrgBr>
				<DrctDbtTx>
					<MndtRltdInf>
						<MndtId>VIRGAY123</MndtId>
						<DtOfSgntr>2008-07-13</DtOfSgntr>
						<FnlColltnDt>2015-07-13</FnlColltnDt>
						<Frqcy>YEAR</Frqcy>
					</MndtRltdInf>
				</DrctDbtTx>
				<DbtrAgt>
					<FinInstnId>
						<BIC>BBBBUS39</BIC>
					</FinInstnId>
				</DbtrAgt>
				<Dbtr>
					<Nm>Jones</Nm>
					<PstlAdr>
						<StrtNm>Hudson Street</StrtNm>
						<BldgNb>19</BldgNb>
						<PstCd>NJ 07302</PstCd>
						<TwnNm>Jersey City</TwnNm>
						<Ctry>US</Ctry>
					</PstlAdr>
				</Dbtr>
				<DbtrAcct>
					<Id>
						<Othr>
							<Id>123456</Id>
						</Othr>
					</Id>
				</DbtrAcct>
				<Purp>
					<Cd>LIFI</Cd>
				</Purp>
				<RmtInf>
					<Ustrd>LIFE INSURANCE PAYMENT/ JULY 2010</Ustrd>
				</RmtInf>
			</DrctDbtTxInf>
			<DrctDbtTxInf>
				<PmtId>
					<EndToEndId>AY090327/456</EndToEndId>
				</PmtId>
				<PmtTpInf>
					<InstrPrty>NORM</InstrPrty>
					<SvcLvl>
						<Prtry>VERPA-1</Prtry>
					</SvcLvl>
					<SeqTp>OOFF</SeqTp>
				</PmtTpInf>
				<InstdAmt Ccy=""USD"">985</InstdAmt>
				<ChrgBr>SHAR</ChrgBr>
				<DrctDbtTx>
					<PreNtfctnId>VIRGAY2435/2010</PreNtfctnId>
					<PreNtfctnDt>2010-06-08</PreNtfctnDt>
				</DrctDbtTx>
				<DbtrAgt>
					<FinInstnId>
						<BIC>CCCCUS27</BIC>
					</FinInstnId>
				</DbtrAgt>
				<Dbtr>
					<Nm>Lee</Nm>
					<PstlAdr>
						<StrtNm>Cross Road</StrtNm>
						<BldgNb>45</BldgNb>
						<PstCd>NJ07399</PstCd>
						<TwnNm>Jersey City</TwnNm>
						<Ctry>US</Ctry>
					</PstlAdr>
				</Dbtr>
				<DbtrAcct>
					<Id>
						<Othr>
							<Id>789101</Id>
						</Othr>
					</Id>
				</DbtrAcct>
				<RmtInf>
					<Ustrd>CAR INSURANCE PREMIUM</Ustrd>
				</RmtInf>
			</DrctDbtTxInf>
		</PmtInf>
	</CstmrDrctDbtInitn>
</Document>";

            //Original valid pain.008.002.01 XSD File from ISO20022
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd";

            string validatingErrors = XMLValidator.ValidateXMLStringThroughXSDFile(xMLString, xSDFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }


        [TestMethod]
        public void AnXMLFileIsWellValidatedThroughXSD()
        {
            //Original valid pain.008.001.02 XML file from ISO20022
            string xMLFilePath = @"XMLFiles\pain.008.001.02.xml";

            //Original valid pain.008.002.01 XSD File from ISO20022
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd";

            string validatingErrors = XMLValidator.ValidateXMLFileThroughXSDFile(xMLFilePath, xSDFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }


        [TestMethod]
        public void AnElementISCorrectlyAddedAsRootInXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022
            string elementName = "GrpHdr";
            string elementType = "GroupHeader39";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";
            XmlQualifiedName elementQualifiedName = new XmlQualifiedName(elementName, xMLNamespace);

            XmlSchema xmlSchema = XmlSchema.Read(new StreamReader(xSDFilePath), XMLValidationEventHandler);
            Assert.IsNull(xmlSchema.Elements[elementQualifiedName]);

            XMLValidatorHelper.AddElementToSchema(xmlSchema, elementName, elementType, xMLNamespace);
            Assert.IsNotNull(xmlSchema.Elements[elementQualifiedName]);
        }

        [TestMethod]
        public void ThisXMLNodeIsWellValidatedThrougModifiedXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<GrpHdr xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
	<MsgId>CAVAY1234</MsgId>
	<CreDtTm>2010-06-25T14:25:00</CreDtTm>
	<NbOfTxs>2</NbOfTxs>
	<CtrlSum>2010</CtrlSum>
	<InitgPty>
		<Nm>Virgay</Nm>
		<PstlAdr>
			<StrtNm>Virginia Lane</StrtNm>
			<BldgNb>36</BldgNb>
			<PstCd>NJ 07311</PstCd>
			<TwnNm>Jersey City</TwnNm>
			<Ctry>US</Ctry>
		</PstlAdr>
		<CtctDtls>
			<Nm>J. Thompson</Nm>
			<EmailAdr>Thompson@virgay.com</EmailAdr>
		</CtctDtls>
	</InitgPty>
</GrpHdr>";
            string elementName = "GrpHdr";
            string elementType = "GroupHeader39";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void ThisXMLStringDoesNotValidateBecauseInitgPtIsNotCompilantToXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<GrpHdr xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
	<MsgId>CAVAY1234</MsgId>
	<CreDtTm>2010-06-25T14:25:00</CreDtTm>
	<NbOfTxs>2</NbOfTxs>
	<CtrlSum>2010</CtrlSum>
	<InitgPt>
		<Nm>Virgay</Nm>
		<PstlAdr>
			<StrtNm>Virginia Lane</StrtNm>
			<BldgNb>36</BldgNb>
			<PstCd>NJ 07311</PstCd>
			<TwnNm>Jersey City</TwnNm>
			<Ctry>US</Ctry>
		</PstlAdr>
		<CtctDtls>
			<Nm>J. Thompson</Nm>
			<EmailAdr>Thompson@virgay.com</EmailAdr>
		</CtctDtls>
	</InitgPt>
</GrpHdr>";
            string elementName = "GrpHdr";
            string elementType = "GroupHeader39";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreNotEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void ThisInternalXMLNodeIsWellValidatedThrougModifiedXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<InitgPty xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
	<Nm>Virgay</Nm>
	<PstlAdr>
		<StrtNm>Virginia Lane</StrtNm>
		<BldgNb>36</BldgNb>
		<PstCd>NJ 07311</PstCd>
		<TwnNm>Jersey City</TwnNm>
		<Ctry>US</Ctry>
	</PstlAdr>
	<CtctDtls>
		<Nm>J. Thompson</Nm>
		<EmailAdr>Thompson@virgay.com</EmailAdr>
	</CtctDtls>
</InitgPty>";
            string elementName = "InitgPty";
            string elementType = "PartyIdentification32";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void ThisInternalXMLNodeUsingClassNameInsteadXMLTagIsWellValidatedThrougModifiedXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<PartyIdentification32 xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
	<Nm>Virgay</Nm>
	<PstlAdr>
		<StrtNm>Virginia Lane</StrtNm>
		<BldgNb>36</BldgNb>
		<PstCd>NJ 07311</PstCd>
		<TwnNm>Jersey City</TwnNm>
		<Ctry>US</Ctry>
	</PstlAdr>
	<CtctDtls>
		<Nm>J. Thompson</Nm>
		<EmailAdr>Thompson@virgay.com</EmailAdr>
	</CtctDtls>
</PartyIdentification32>";
            string elementName = "PartyIdentification32";
            string elementType = "PartyIdentification32";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void ThisInternalXMLNodeUsingCustomRootTagInsteadDefinedXMLTagIsWellValidatedThrougModifiedXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<Pepe xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
	<Nm>Virgay</Nm>
	<PstlAdr>
		<StrtNm>Virginia Lane</StrtNm>
		<BldgNb>36</BldgNb>
		<PstCd>NJ 07311</PstCd>
		<TwnNm>Jersey City</TwnNm>
		<Ctry>US</Ctry>
	</PstlAdr>
	<CtctDtls>
		<Nm>J. Thompson</Nm>
		<EmailAdr>Thompson@virgay.com</EmailAdr>
	</CtctDtls>
</Pepe>";
            string elementName = "Pepe";
            string elementType = "PartyIdentification32";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void ThisXMLStringDoesNotValidateBecauseNIsNotCompilantToXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<PartyIdentification32 xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
	<N>Virgay</N>
	<PstlAdr>
		<StrtNm>Virginia Lane</StrtNm>
		<BldgNb>36</BldgNb>
		<PstCd>NJ 07311</PstCd>
		<TwnNm>Jersey City</TwnNm>
		<Ctry>US</Ctry>
	</PstlAdr>
	<CtctDtls>
		<Nm>J. Thompson</Nm>
		<EmailAdr>Thompson@virgay.com</EmailAdr>
	</CtctDtls>
</PartyIdentification32>";
            string elementName = "PartyIdentification32";
            string elementType = "PartyIdentification32";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreNotEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void ThisSerializedInternalXMLNodeIsWellValidatedThrougModifiedXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""utf-16""?>
<PartyIdentification32 xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
  <Nm>Real Club Náutico de Gran Canaria</Nm>
  <Id>
    <OrgId>
      <Othr>
        <Id>ES90011G35008770</Id>
        <SchmeNm>
          <Prtry>SEPA</Prtry>
        </SchmeNm>
      </Othr>
    </OrgId>
  </Id>
</PartyIdentification32>";
            string elementName = "PartyIdentification32";
            string elementType = "PartyIdentification32";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        public void ThisModifiedSerializedInternalXMLStringDoesNotValidateBecauseOrgIIsNotCompilantToXSD()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""utf-16""?>
<PartyIdentification32 xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
  <Nm>Real Club Náutico de Gran Canaria</Nm>
  <Id>
    <OrgI>
      <Othr>
        <Id>ES90011G35008770</Id>
        <SchmeNm>
          <Prtry>SEPA</Prtry>
        </SchmeNm>
      </Othr>
    </OrgI>
  </Id>
</PartyIdentification32>";
            string elementName = "PartyIdentification32";
            string elementType = "PartyIdentification32";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreNotEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void ThisXMLStringIsDoesNotValidateBecauseSomeElementClosingTagAreMissing()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<GrpHdr xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
	<MsgId>CAVAY1234</MsgId>
	<CreDtTm>2010-06-25T14:25:00</CreDtTm>
	<NbOfTxs>2</NbOfTxs>
	<CtrlSum>2010</CtrlSum>
	<InitgPty>
		<Nm>Virgay</Nm>
		<PstlAdr>
			<StrtNm>Virginia Lane</StrtNm>
			<BldgNb>36</BldgNb>
			<PstCd>NJ 07311</PstCd>
			<TwnNm>Jersey City</TwnNm>
			<Ctry>US</Ctry>
		<CtctDtls>
			<Nm>J. Thompson</Nm>
			<EmailAdr>Thompson@virgay.com</EmailAdr>
		</CtctDtls>
	</InitgPty>
</GrpHdr>";
            string elementName = "GrpHdr";
            string elementType = "GroupHeader39";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreNotEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void ThisXMLStringIsDoesNotValidateBecauseSomeElementTagAreErroneous()
        {
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd"; //Original valid pain.008.002.01 XSD File from ISO20022

            string xMLNode = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<GrpHdr xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"">
	<MsgId>CAVAY1234</MsgId>
	<CreDtTm>2010-06-25T14:25:00</CreDtTm>
	<NbOfTxs>2</NbOfTxs>
	<CtrlSum>2010</CtrlSum>
	<InitgPty>
		<Nm>Virgay</Nm>
		<PstlAdr>
			<StrtNm>Virginia Lane</StrtNm>
			<BldgNb>36</BldgNb>
			<PstCd>NJ 07311</PstCd>
			<TwnNm>Jersey City</TwnNm>
			<Ctry>US</Ctry>
		</PstlAd>
		<CtctDtls>
			<Nm>J. Thompson</Nm>
			<EmailAdr>Thompson@virgay.com</EmailAdr>
		</CtctDtls>
	</InitgPty>
</GrpHdr>";
            string elementName = "GrpHdr";
            string elementType = "GroupHeader39";
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

            string validatingErrors = XMLValidator.ValidateXMLNodeThroughModifiedXSD(
                elementName,
                elementType,
                xMLNamespace,
                xMLNode,
                xSDFilePath);
            Assert.AreNotEqual(String.Empty, validatingErrors);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void TheXMLValidationEventHandlerWorksOK()
        {
            string xSDFilePath = @"XML Test Files\wrongXSDFile.xsd";
            try
            {
                XmlSchema xmlSchema = XmlSchema.Read(new StreamReader(xSDFilePath), XMLValidationEventHandler);
            }
            catch (System.Xml.XmlException e)
            {
                Assert.IsNotNull(e);
                throw e;
            }
        }

        private void XMLValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw e.Exception;
        }
    }
}
