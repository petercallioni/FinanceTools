namespace FinanceTools.Data.UserItems
{
    public class EventBuy
    {
        // Navigation
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;
        public int BrokerId { get; set; }
        public Broker Broker { get; set; } = default!;

        public ICollection<EventSellAllocation> SellAllocations { get; set; } = new List<EventSellAllocation>();


        // Properties
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }

        /// <summary>
        /// The average purchase price of the asset at the time of the buy event.
        /// This excludes brokerage fees.
        /// </summary>
        public decimal AveragePurchasePrice { get; set; }
        public decimal BrokerageFee { get; set; } = 0;
        public DateTime ContractDate { get; set; }

        // Calculated properties
        public decimal TotalCost => (AveragePurchasePrice * Quantity) + BrokerageFee;
        public decimal AverageCostPerUnitIncBrokerage => Quantity != 0 ? TotalCost / Quantity : 0;
    }
}
