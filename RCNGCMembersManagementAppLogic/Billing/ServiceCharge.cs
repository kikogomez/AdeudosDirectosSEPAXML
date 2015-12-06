using RCNGCMembersManagementAppLogic.ClubServices;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class ServiceCharge: Transaction
    {
        ClubService service;
        string concept;
        int units;
        double unitCost;
        Tax tax;
        double discount;

        public ServiceCharge(ClubService service)
            : this(service, service.Description, 1, service.Cost, service.Tax,0)
        {
        }

        public ServiceCharge(ClubService service, string concept, int units, double discount)
            : this(service,concept,units,service.Cost, service.Tax, discount)
        {
        }

        public ServiceCharge(ClubService service, string concept, int units, double unitCost, Tax tax, double discount)
            :base(concept, units, unitCost, tax, discount)
        {
            this.service = service;
            this.concept = concept;
            this.units = units;
            this.unitCost = unitCost;
            this.tax = tax;
            this.discount = discount;
        }
    }
}
