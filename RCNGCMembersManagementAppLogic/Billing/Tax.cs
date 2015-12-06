namespace RCNGCMembersManagementAppLogic.Billing
{
    public class Tax
    {
        string taxName;
        double taxValue;

        public Tax(string taxName, double taxValue)
        {
            if (taxValue < 0) throw new System.ArgumentOutOfRangeException("taxValue", "Tax percentages can't be negative");
            if (taxValue!=0 && (taxName ?? "").Trim() == "") throw new System.ArgumentException("Tax name can't be empty or null", "taxName");
            this.taxName = taxName;
            this.taxValue = taxValue;
        }

        public double TaxPercentage
        {
            get { return taxValue; }
        }
    }
}
