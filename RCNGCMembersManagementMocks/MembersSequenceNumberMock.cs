using RCNGCMembersManagementAppLogic;

namespace RCNGCMembersManagementMocks
{
    public class MembersSequenceNumberMock: IMembersSequenceNumberManager
    {
        static uint memberIDSequenceNumber = 0;

        public MembersSequenceNumberMock()
        {
        }

        public uint GetMemberIDSequenceNumber()
        {
            return memberIDSequenceNumber;
        }

        public void SetMemberIDSequenceNumber(uint memberIDSequenceNumber)
        {
            MembersSequenceNumberMock.memberIDSequenceNumber = memberIDSequenceNumber;
        }
    }
}
