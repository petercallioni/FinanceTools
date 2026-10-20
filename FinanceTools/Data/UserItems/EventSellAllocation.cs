namespace FinanceTools.Data.UserItems
{
    public class EventSellAllocation
    {
        public int EventBuyId { get; set; }
        public EventBuy EventBuy { get; set; } = default!;

        public int EventSellId { get; set; }
        public EventSell EventSell { get; set; } = default!;

        public decimal Quantity { get; set; }
    }
}
