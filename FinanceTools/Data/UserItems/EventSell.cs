namespace FinanceTools.Data.UserItems
{
    public class EventSell
    {
        // Navigation
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;
        public ICollection<EventSellAllocation> SellAllocations { get; set; } = new List<EventSellAllocation>();


        // Properties   
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public DateTime ContractDate { get; set; }
        public int BrokerId { get; set; }
        public Broker Broker { get; set; } = default!;

        /// <summary>
        /// The average sale price of the asset at the time of the sell event.
        /// This excludes brokerage fees.
        /// </summary>
        public decimal AverageSalePrice { get; set; }
        public decimal BrokerageFee { get; set; } = 0;

        // Calculated properties
        public decimal TotalCapital => (AverageSalePrice * Quantity) + BrokerageFee;
        public decimal AverageCostPerUnitIncBrokerage => Quantity != 0 ? TotalCapital / Quantity : 0;


        //Navigation Shortcuts
        /// <summary>
        /// EventSells must be loaded with                 
        /// .Include(sellEvent => sellEvent.SellAllocations).ThenInclude(allocation => allocation.EventBuy)
        /// </summary>
        public string Symbol => SellAllocations.FirstOrDefault()?.EventBuy?.Symbol ?? string.Empty;

        /// <summary>
        /// EventSells must be loaded with                 
        /// .Include(sellEvent => sellEvent.SellAllocations).ThenInclude(allocation => allocation.EventBuy)
        /// </summary>
        public string CompanyName => SellAllocations.FirstOrDefault()?.EventBuy?.CompanyName ?? string.Empty;
    }
}
