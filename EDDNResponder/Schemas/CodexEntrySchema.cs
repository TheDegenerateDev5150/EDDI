﻿using EddiEddnResponder.Sender;
using EddiEddnResponder.Toolkit;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Utilities;

namespace EddiEddnResponder.Schemas
{
    [UsedImplicitly]
    public class CodexEntrySchema : ISchema
    {
        public List<string> edTypes => new List<string> { "CodexEntry" };

        public bool Handle(string edType, ref IDictionary<string, object> data, EDDNState eddnState)
        {
            try
            {
                if (!edTypes.Contains(edType)) { return false; }
                if (eddnState?.Location is null || eddnState.GameVersion is null) { return false; }
                if (!eddnState.Location.CheckLocationData(edType, data)) { return false; }

                // No personal data to remove
                data = PersonalDataStripper.Strip(data, edType);

                // Apply data augments
                data = eddnState.Location.AugmentStarPos(data);
                data = eddnState.Location.AugmentBody(data);
                data = eddnState.GameVersion.AugmentVersion(data);

                EDDNSender.SendToEDDN("https://eddn.edcd.io/schemas/codexentry/1", data, eddnState);
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