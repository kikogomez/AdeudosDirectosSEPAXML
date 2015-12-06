using RCNGCMembersManagementAppLogic.Billing;

namespace RCNGCMembersManagementAppLogic.ClubServices
{
    public class ClubService
    {
        string description;
        double cost;
        Tax tax;

        public ClubService(string description, double cost, Tax tax)
        {
            if (cost < 0) throw new System.ArgumentException("Service cost can't be negative", "taxName");
            if ((description ?? "").Trim() == "") throw new System.ArgumentException("Service name can't be empty or null", "taxName");
            this.description = description;
            this.cost = cost;
            this.tax = tax;
        }

        public string Description
        {
            get { return description; }
        }
        
        public double Cost
        {
            get { return cost; }
        }

        public Tax Tax
        {
            get { return tax; }
        }
    }
}
