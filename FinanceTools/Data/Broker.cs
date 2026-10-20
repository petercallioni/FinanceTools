using FinanceTools.Data.UserItems;

namespace FinanceTools.Data
{
    public class Broker
    {
        // Reverse navigation
        public ICollection<EventBuy> EventBuys { get; set; } = new List<EventBuy>();
        public ICollection<EventSell> EventSells { get; set; } = new List<EventSell>();

        // Properties
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
