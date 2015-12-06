using RCNGCMembersManagementAppLogic.ClubStore;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class Sale: Transaction
    {
        Product product;
        string concept;
        int units;
        double unitCost;
        Tax tax;
        double discount;

        public Sale(Product product)
            : this(product, product.Description, 1, product.Cost, product.Tax, 0)
        {
        }

        public Sale(Product product, string concept, int units, double discount)
            : this(product, concept, units, product.Cost, product.Tax, discount)
        {
        }

        public Sale(Product product, string concept, int units, double unitCost, Tax tax, double discount)
            :base(concept, units, unitCost, tax, discount)
        {
            this.product = product;
            this.concept = concept;
            this.units = units;
            this.unitCost = unitCost;
            this.tax = tax;
            this.discount = discount;
        }
    }
}
