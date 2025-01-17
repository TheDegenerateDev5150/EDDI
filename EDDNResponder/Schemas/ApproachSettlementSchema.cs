﻿using EddiEddnResponder.Sender;
using EddiEddnResponder.Toolkit;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Utilities;

namespace EddiEddnResponder.Schemas
{
    [UsedImplicitly]
    public class ApproachSettlementSchema : ISchema
    {
        public List<string> edTypes => new List<string> { "ApproachSettlement" };

        public bool Handle(string edType, ref IDictionary<string, object> data, EDDNState eddnState)
        {
            try
            {
                if (!edTypes.Contains(edType)) { return false; }
                if (eddnState?.Location is null || eddnState.GameVersion is null) { return false; }
                if (!eddnState.Location.CheckLocationData(edType, data)) { return false; }
                if (!data.ContainsKey("Latitude") || !data.ContainsKey("Longitude"))
                {
                    // When re-logging at a Planetary Port, the ApproachSettlement event written may be missing the Latitude and Longitude properties.
                    // Silently ignore this FDev issue.
                    return false;
                }

                // Strip any localized properties
                data = PersonalDataStripper.Strip(data, edType);

                // Apply data augments
                data = eddnState.Location.AugmentStarSystemName(data);
                data = eddnState.Location.AugmentSystemAddress(data); // Later version journal events have this but we'll double check before sending to EDDN.
                data = eddnState.Location.AugmentStarPos(data);
                data = eddnState.GameVersion.AugmentVersion(data);

                EDDNSender.SendToEDDN("https://eddn.edcd.io/schemas/approachsettlement/1", data, eddnState);
                return true;
            }
            catch (Exception e)
            {
                Logging.Error($"{GetType().Name} failed to handle journal data.", e);
                return false;
            }
        }
    }
}