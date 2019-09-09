namespace PlusValuesFifo.Models
{
    /// <summary>
    /// Communication contract with Crypto-Data service
    /// </summary>
    public class CryptoPriceModel
    {
        public CryptoPriceModel(decimal price, bool volatilEventHappened)
        {
            Price = price;
            VolatilEventHappened = volatilEventHappened;
        }

        public decimal Price { get; set; }
        public bool VolatilEventHappened { get; set; }
    }
}
