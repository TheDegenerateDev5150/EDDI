﻿using EddiEddnResponder.Toolkit;

namespace EddiEddnResponder
{
    public class EDDNState
    {
        public readonly GameVersionAugmenter GameVersion;

        public readonly LocationAugmenter Location;

        public readonly PersonalDataStripper PersonalData;

        public EDDNState()
        {
            GameVersion = new GameVersionAugmenter();
            Location = new LocationAugmenter();
            PersonalData = new PersonalDataStripper();
        }

        public EDDNState ( GameVersionAugmenter gameVersion, LocationAugmenter locationAugmenter, PersonalDataStripper personalDataStripper )
        {
            GameVersion = gameVersion;
            Location = locationAugmenter;
            PersonalData = personalDataStripper;
        }
    }
}