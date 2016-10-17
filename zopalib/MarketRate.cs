namespace zopalib
{
    /// <summary>
    /// Encapsulates the data from a single lender
    /// </summary>
    public class MarketRate
    {
        public string Lender { get; set; }
        public float Rate { get; set; }
        public int AmountAvailable { get; set; }
    }
}
