using System;

namespace RCNGCMembersManagementAppLogic.MembersManaging
{
    public sealed class ClubMemberDataManager
    {
        private static readonly ClubMemberDataManager instance = new ClubMemberDataManager();

        static IMembersSequenceNumberManager membersSequenceNumberCollaborator;

        static uint memberIDLowerLimit = 1;
        static uint memberIDUpperLimit = 100000;

        private ClubMemberDataManager()
        {
        }

        public static ClubMemberDataManager Instance
        {
            get { return instance; }
        }

        public uint MemberIDSequenceNumber
        {
            get { return GetMemberIDSequenceNumber(); }
            set { SetMemberIDSequenceNumber(value); }
        }

        public bool AvailableMembersIDAreExhausted
        {
            get { return (MemberIDSequenceNumber == memberIDUpperLimit); }
        }

        public uint AssingnMemberIDSequenceNumber()
        {
            if (MemberIDSequenceNuberIsInRange(MemberIDSequenceNumber))
            {
                return MemberIDSequenceNumber++;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    "memberIDSequenceNumber", 
                    "Member ID out of range (" + memberIDLowerLimit.ToString() + "-" + (memberIDUpperLimit-1).ToString() + ")"); 
            }
        }

        public void SetMembersSequenceNumberCollaborator(IMembersSequenceNumberManager membersSequenceNumberCollaborator)
        {
            ClubMemberDataManager.membersSequenceNumberCollaborator = membersSequenceNumberCollaborator;
        }

        private uint GetMemberIDSequenceNumber()
        {
            uint memberSequenceNumber=membersSequenceNumberCollaborator.GetMemberIDSequenceNumber();
            return memberSequenceNumber;
        }

        private void SetMemberIDSequenceNumber(uint memberIDSequenceNumber)
        {
            if (MemberIDSequenceNuberIsInRange(memberIDSequenceNumber) || memberIDSequenceNumber == memberIDUpperLimit)
            {
                membersSequenceNumberCollaborator.SetMemberIDSequenceNumber(memberIDSequenceNumber);
                return;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    "memberIDSequenceNumber",
                    "Member ID out of range (" + memberIDLowerLimit.ToString() + "-" + (memberIDUpperLimit - 1).ToString() + ")");
            }
        }

        private bool MemberIDSequenceNuberIsInRange(uint memberID)
        {
            return (memberIDLowerLimit <= memberID && memberID < (memberIDUpperLimit));
        }
    }
}
