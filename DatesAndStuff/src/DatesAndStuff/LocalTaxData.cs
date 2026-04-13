namespace DatesAndStuff
{
    public class LocalTaxData
    {
        /// <summary>
        /// Administrative territorial unit identifier.
        /// </summary>
        public string UAT { get; private set; }

        public List<TaxItem> TaxItems { get; set; }

        public double DiscountPercentage { get; set; }

        public double YearlyTax
        {
            get
            {
                return 0;
            }
        }

        public LocalTaxData(string UAT)
        {
            this.UAT = UAT;
        }
    }
}