﻿using EddiDataDefinitions;
using EddiEvents;
using System;
using System.Collections.Generic;
using Utilities;

namespace EddiShipMonitor
{
    public class ModuleTransferEvent : Event
    {
        public const string NAME = "Module transfer";
        public const string DESCRIPTION = "Triggered when you transfer a module from storage at another station";
        public const string SAMPLE = "{ \"timestamp\":\"2017-09-20T07:05:41Z\", \"event\":\"FetchRemoteModule\", \"StorageSlot\":25, \"StoredItem\":\"$hpt_cloudscanner_size0_class1_name;\", \"StoredItem_Localised\":\"Wake Scanner\", \"ServerId\":128662525, \"TransferCost\":322, \"TransferTime\":120, \"Ship\":\"diamondbackxl\", \"ShipID\":38 }";

        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static ModuleTransferEvent()
        {
            VARIABLES.Add("ship", "The ship you are in when you request the transfer");
            VARIABLES.Add("shipid", "The ID of the ship you are in when you request the transfer");
            // VARIABLES.Add("storageslot", "The storage slot"); // This probably isn't useful to the end user
            // VARIABLES.Add("serverid", "The frontier ID of the item being sold"); // This probably isn't useful to the end user
            VARIABLES.Add("module", "The module (object) being transferred");
            VARIABLES.Add("transfercost", "The cost for the module transfer");
            VARIABLES.Add("transfertime", "The time until the module arrives (in seconds)");
        }

        [PublicAPI]
        public string ship { get; private set; }

        [PublicAPI]
        public int? shipid { get; private set; }

        [PublicAPI]
        public Module module { get; private set; }

        [PublicAPI]
        public long transfercost { get; private set; }

        [PublicAPI]
        public long? transfertime { get; private set; }

        //Not intended to be user facing

        public int storageslot { get; private set; }

        public long serverid { get; private set; }

        public ModuleTransferEvent(DateTime timestamp, string ship, int? shipid, int storageslot, long serverid, Module module, long transfercost, long? transfertime) : base(timestamp, NAME)
        {
            this.ship = ShipDefinitions.FromEDModel(ship).model;
            this.shipid = shipid;
            this.storageslot = storageslot;
            this.serverid = serverid;
            this.module = module;
            this.transfercost = transfercost;
            this.transfertime = transfertime;
        }
    }
}
