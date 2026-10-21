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
                new PredefinedBroker {
                    Name = "CommSec",
                    LogoFileName = "commsec.jpg",
                    BrokerageFee = (decimal tradeValue) => {
                        switch (tradeValue)
                        {
                            case <= 1000:
                                return 5m;
                            case <= 3000:
                                return 10m;
                            case <= 10_000:
                                return 19.95m;
                            case <= 25_000:
                                return 29.95m;
                            default:
                                return tradeValue * 0.012m;
                        }
                    }},
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
        public Func<decimal, decimal> BrokerageFee { get; set; } = (decimal tradeValue) => 0;
    }
}
