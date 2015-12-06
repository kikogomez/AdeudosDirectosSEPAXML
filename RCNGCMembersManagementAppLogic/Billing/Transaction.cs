using System;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class Transaction
    {
        string description;
        int units;
        double unitCost;
        Tax tax;
        double discount;

        public Transaction(string description, int units, double unitCost, Tax tax, double discount)
        {
            if (unitCost < 0) throw new System.ArgumentOutOfRangeException("uniCost", "Transactions units cost can't be negative");
            if (units == 0) throw new System.ArgumentOutOfRangeException("units", "A transaction can't have zero units");
            this.description = description;
            this.units = units;
            this.unitCost = unitCost;
            this.tax = tax;
            this.discount = discount;
        }

        public string Description
        {
            get { return description; }
        }

        public int Units
        {
            get { return units; }
        }

        public double UnitCost
        {
            get { return unitCost; }
        }

        public Tax Tax
        {
            get { return tax; }
        }

        public double Discount
        {
            get { return discount; }
        }

        public decimal GrossAmount
        {
            get { return CalculateGrossAmount(); }
        }

        public decimal NetAmount
        {
            get { return CalculateNetAmount(); }
        }

        public decimal TaxAmount
        {
            get { return CalculateTaxAmount(); }
        }

        public bool CompareTo(Transaction otherTransaction)
        {
            return
                Description == otherTransaction.Description &&
                Units == otherTransaction.Units &&
                UnitCost == otherTransaction.UnitCost &&
                Discount == otherTransaction.Discount &&
                Tax.TaxPercentage == otherTransaction.Tax.TaxPercentage;
        }

        private decimal unitCostWithDiscount()
        {
            return Math.Round((decimal)unitCost * ((decimal)(1 - discount / 100)), 2, MidpointRounding.AwayFromZero);
        }

        private decimal unitCostWithDiscountThenTax()
        {
            return Math.Round(unitCostWithDiscount() * ((decimal)(1 + tax.TaxPercentage / 100)), 2, MidpointRounding.AwayFromZero);
        }

        private decimal CalculateGrossAmount()
        {
            return Math.Round(unitCostWithDiscount() * units, 2, MidpointRounding.AwayFromZero);
        }

        private decimal CalculateNetAmount()
        {
            return Math.Round(unitCostWithDiscountThenTax() * units, 2, MidpointRounding.AwayFromZero);
        }

        private decimal CalculateTaxAmount()
        {
            return CalculateTaxAmountPerUnit() * units;
        }

        private decimal CalculateTaxAmountPerUnit()
        {
            return Math.Round(unitCostWithDiscount() * ((decimal)(tax.TaxPercentage / 100)), 2, MidpointRounding.AwayFromZero);
        }
    }
}
