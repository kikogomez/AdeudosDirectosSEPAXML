using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;

namespace AdeudosDirectosSEPAXMLSpecFlowBDD
{
    [Binding, Scope(Feature = "Add Debtors")]
    public class AddDebtorsFeatureSteps
    {
        private readonly DebtorsManagementContextData debtorsManagementContextData;

        public AddDebtorsFeatureSteps(DebtorsManagementContextData debtorsManagementContextData)
        {
            this.debtorsManagementContextData = debtorsManagementContextData;
        }

        [Given(@"These names ""(.*)"", ""(.*)"", ""(.*)""")]
        public void GivenTheseNames(string givenName, string firstSurname, string secondSurname)
        {
            debtorsManagementContextData.givenName = givenName;
            debtorsManagementContextData.firstSurname = firstSurname;
            debtorsManagementContextData.secondSurname = secondSurname;
        }

        [When(@"I process the names")]
        public void WhenIProcessTheNames()
        {
            Debtor debtor;
            try
            {
                debtor = new Debtor(
                    "0001",
                    debtorsManagementContextData.givenName,
                    debtorsManagementContextData.firstSurname,
                    debtorsManagementContextData.secondSurname);
                debtorsManagementContextData.debtor = debtor; ;
            }
            catch
            {
                debtorsManagementContextData.debtor = null;
            }     
        }

        [Then(@"The name is considered ""(.*)""")]
        public void ThenTheNameIsConsidered(string validity)
        {
            bool valid = (validity == "valid" ? true : false);
            Assert.AreEqual(debtorsManagementContextData.debtor != null, valid);
        }
    }
}
