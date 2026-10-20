namespace FinanceTools.Static
{
    public static class BrokerLookup
    {
        public static List<PredefinedBroker> Brokers()
        {
            return new List<PredefinedBroker>
            {
                new PredefinedBroker { Name = "Betashares Direct", LogoFileName = "betashares_direct.png" },
                new PredefinedBroker { Name = "CMC Invest", LogoFileName = "cmc_invest.png" },
                new PredefinedBroker { Name = "CommSec", LogoFileName = "commsec.jpg" },
                new PredefinedBroker { Name = "eToro", LogoFileName = "etorro.jpg" },
                new PredefinedBroker { Name = "Interactive Brokers Australia", LogoFileName = "interactive_brokers.jpg" },
                new PredefinedBroker { Name = "Moomoo", LogoFileName = "moomoo.png" },
                new PredefinedBroker { Name = "Pearler", LogoFileName = "pearler.png" },
                new PredefinedBroker { Name = "Selfwealth", LogoFileName = "selfwealth.png" },
                new PredefinedBroker { Name = "Stake", LogoFileName = "stake.jpg" },
                new PredefinedBroker { Name = "Superhero", LogoFileName = "superhero.png" },
                new PredefinedBroker { Name = "Webull Australia", LogoFileName = "webull.png" },
            };
        }
    }

    public class PredefinedBroker
    {
        public string Name { get; set; } = string.Empty;
        public string LogoFileName { get; set; } = string.Empty;
        public decimal BrokerageFee(decimal tradeValue) => 0;
    }
}
