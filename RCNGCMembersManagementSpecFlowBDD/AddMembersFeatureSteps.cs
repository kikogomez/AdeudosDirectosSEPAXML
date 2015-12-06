using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementMocks;
using RCNGCMembersManagementAppLogic.MembersManaging;

namespace RCNGCMembersManagementSpecFlowBDD
{
    [Binding, Scope(Feature = "Add Members")]
    public class AddMembersFeatureSteps
    {
        private readonly MembersManagementContextData membersManagementContextData;

        public AddMembersFeatureSteps(MembersManagementContextData membersManagementContextData)
        {
            this.membersManagementContextData = membersManagementContextData;
            MembersSequenceNumberMock clubMemberDataManagerMock = new MembersSequenceNumberMock();
            membersManagementContextData.clubMemberDataManager.SetMembersSequenceNumberCollaborator(clubMemberDataManagerMock);
        }

        [Given(@"These names ""(.*)"", ""(.*)"", ""(.*)""")]
        public void GivenTheseNames(string givenName, string firstSurname, string secondSurname)
        {
            membersManagementContextData.givenName = givenName;
            membersManagementContextData.firstSurname = firstSurname;
            membersManagementContextData.secondSurname = secondSurname;
        }

        [Given(@"The current memberID sequence number is (.*)")]
        public void GivenTheCurrentMemberIDSequenceNumberIs(uint memberIDSequenceNumber)
        {
            membersManagementContextData.clubMemberDataManager.MemberIDSequenceNumber = memberIDSequenceNumber;
        }

        [When(@"I process the names")]
        public void WhenIProcessTheNames()
        {
            ClubMember clubMember;
            membersManagementContextData.clubMemberDataManager.MemberIDSequenceNumber = 1;
            try
            {
                clubMember = new ClubMember(
                    membersManagementContextData.givenName,
                    membersManagementContextData.firstSurname,
                    membersManagementContextData.secondSurname);
                membersManagementContextData.clubMember=clubMember;
            }
            catch
            {
                membersManagementContextData.clubMember = null;
            }     
        }

        [When(@"I add a new member")]
        public void WhenIAddANewMember()
        {
            ClubMember clubMember;
            try
            {
                clubMember = new ClubMember("Francisco","Gomez-Caldito",null);
                membersManagementContextData.clubMember = clubMember;
            }
            catch
            {
                membersManagementContextData.clubMember = null;
            }    
        }

        [Then(@"The name is considered ""(.*)""")]
        public void ThenTheNameIsConsidered(string validity)
        {
            bool valid = (validity == "valid" ? true : false);
            Assert.AreEqual(membersManagementContextData.clubMember != null, valid);
        }

        [Then(@"The current memberID sequence number is (.*)"), Scope(Feature = "Add members")]
        public void ThenTheCurrentMemberIDSequenceNumberIs(uint memberID)
        {
            Assert.AreEqual(memberID, membersManagementContextData.clubMemberDataManager.MemberIDSequenceNumber);
        }


        [Then(@"The new member is not created")]
        public void ThenTheNewMemberIsNotCreated()
        {
            Assert.AreEqual(null, membersManagementContextData.clubMember);
        }
    }
}
