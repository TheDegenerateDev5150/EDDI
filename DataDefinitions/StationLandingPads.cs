namespace EddiDataDefinitions
{
    public class StationLandingPads
    {
        public int Small { get; set; }
        public int Medium { get; set; }
        public int Large { get; set; }

        public StationLandingPads ( int small = 0, int medium = 0, int large = 0 )
        {
            Small = small;
            Medium = medium;
            Large = large;
        }

        public LandingPadSize LargestPad ()
        {
            if ( Large > 0 )
            {
                return LandingPadSize.Large;
            }

            if ( Medium > 0 )
            {
                return LandingPadSize.Medium;
            }

            if ( Small > 0 )
            {
                return LandingPadSize.Small;
            }

            return LandingPadSize.None;
        }

        public bool LandingPadCheck(LandingPadSize shipSize)
        {
            return LargestPad().sizeIndex >= shipSize.sizeIndex;
        }
    }
}