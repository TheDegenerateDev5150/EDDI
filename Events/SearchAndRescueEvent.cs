using EddiDataDefinitions;
using System;
using System.Collections.Generic;
using Utilities;

namespace EddiEvents
{
    public class SearchAndRescueEvent : Event
    {
        public const string NAME = "Search and rescue";
        public const string DESCRIPTION = "Triggered when delivering items to a Search and Rescue contact";
        public const string SAMPLE = "{ \"timestamp\":\"2017-08-26T01:58:24Z\", \"event\":\"SearchAndRescue\", \"MarketID\": 128666762, \"Name\":\"occupiedcryopod\", \"Count\":2, \"Reward\":5310 }";
        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static SearchAndRescueEvent()
        {
            VARIABLES.Add("commodity", "The commodity (object) recovered");
            VARIABLES.Add("localizedcommodityname", "The localized name of the commodity recovered");
            VARIABLES.Add("amount", "The amount of the item recovered");
            VARIABLES.Add("reward", "The monetary reward for completing the search and rescue");
        }

        [PublicAPI]
        public int? amount { get; }

        [PublicAPI]
        public long reward { get; }

        [PublicAPI]
        public string localizedcommodityname => Commodity.localizedName;

        [PublicAPI]
        public CommodityDefinition Commodity { get; }

        // Not intended to be user facing

        public long marketId { get; private set; }

        public SearchAndRescueEvent(DateTime timestamp, CommodityDefinition commodity, int? amount, long reward, long marketId) : base(timestamp, NAME)
        {
            this.amount = amount;
            this.reward = reward;
            this.Commodity = commodity;
            this.marketId = marketId;
        }
    }
}
