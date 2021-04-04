﻿using EddiDataDefinitions;
using EddiEvents;
using System;
using System.Collections.Generic;
using Utilities;

namespace EddiShipMonitor
{
    public class ModuleSoldEvent : Event
    {
        public const string NAME = "Module sold";
        public const string DESCRIPTION = "Triggered when selling a module to outfitting";
        public const string SAMPLE = "{ \"timestamp\":\"2016-06-10T14:32:03Z\", \"event\":\"ModuleSell\", \"MarketID\": 128666762, \"Slot\":\"Slot06_Size2\", \"SellItem\":\"int_cargorack_size1_class1\", \"SellPrice\":877, \"Ship\":\"asp\", \"ShipID\":1 }";
        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static ModuleSoldEvent()
        {
            VARIABLES.Add("ship", "The ship from which the module was sold");
            VARIABLES.Add("shipid", "The ID of the ship from which the module was sold");
            VARIABLES.Add("slot", "The outfitting slot");
            VARIABLES.Add("module", "The module (object) being sold");
            VARIABLES.Add("price", "The price of the module being sold");
        }

        [PublicAPI]
        public string ship => shipDefinition?.model;

        [PublicAPI]
        public int? shipid { get; private set; }

        [PublicAPI]
        public string slot { get; private set; }

        [PublicAPI]
        public Module module { get; private set; }

        [PublicAPI]
        public long price { get; private set; }

        // Not intended to be user facing

        public long marketId { get; private set; }

        public Ship shipDefinition { get; private set; }

        public ModuleSoldEvent(DateTime timestamp, string ship, int? shipid, string slot, Module module, long price, long marketId) : base(timestamp, NAME)
        {
            this.shipDefinition = ShipDefinitions.FromEDModel(ship);
            this.shipid = shipid;
            this.slot = slot;
            this.module = module;
            this.price = price;
            this.marketId = marketId;
        }
    }
}