﻿using EddiEddnResponder.Sender;
using EddiEddnResponder.Toolkit;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Utilities;

namespace EddiEddnResponder.Schemas
{
    [UsedImplicitly]
    public class JournalSchema : ISchema
    {
        public List<string> edTypes => new List<string>
        {
            "CarrierJump",
            "Docked", 
            "FSDJump", 
            "Scan", 
            "Location", 
            "SAASignalsFound",
            "Scan"
        };

        public bool Handle(string edType, ref IDictionary<string, object> data, EDDNState eddnState)
        {
            try
            {
                if (!edTypes.Contains(edType)) { return false; }
                if (eddnState?.Location is null || eddnState.GameVersion is null) { return false; }
                if (!eddnState.Location.CheckLocationData(edType, data) || !CheckSanity(edType, data)) return false;

                // Remove personal data
                data = PersonalDataStripper.Strip(data, edType);

                // Apply data augments
                data = eddnState.GameVersion.AugmentVersion( data );
                data = eddnState.Location.AugmentStarPos( data );
                data = eddnState.Location.AugmentStarSystemName( data );
                data = eddnState.PowerPlay.AugmentPledgeState( data );

                EDDNSender.SendToEDDN("https://eddn.edcd.io/schemas/journal/1", data, eddnState);
                return true;
            }
            catch (Exception e)
            {
                Logging.Error($"{GetType().Name} failed to handle journal data.", e);
                return false;
            }
        }

        private bool CheckSanity(string edType, IDictionary<string, object> data)
        {
            // We've already vetted location data via the CheckLocationData method.
            // Perform any additional quality checks we think we need here.
            var passed = true;
            switch (edType)
            {
                case "Docked":
                    // Identify and catch a possible FDev bug that can allow incomplete `Docked` messages
                    // missing a MarketID and many other properties.
                    if (!data.ContainsKey("MarketID")) { passed = false; }

                    // Don't allow messages with a missing StationName.
                    if (data.ContainsKey("StationName") && string.IsNullOrEmpty(JsonParsing.getString(data, "StationName"))) { passed = false; }

                    break;
                case "SAASignalsFound":
                    if (!data.ContainsKey("Signals")) { passed = false; }
                    break;
                case "Scan":
                    if (!data.ContainsKey("ScanType")) { passed = false; }
                    break;
            }
            return passed;
        }
    }
}