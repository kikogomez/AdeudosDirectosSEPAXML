namespace RCNGCMembersManagementAppLogic
{
    public interface IMembersSequenceNumberManager
    {
        uint GetMemberIDSequenceNumber();
        void SetMemberIDSequenceNumber(uint memberIDSequenceNumber);
    }
}
