using Newtonsoft.Json;

namespace EddiDataDefinitions
{
    /// <summary> Station's largest landing pad size </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class LandingPadSize : ResourceBasedLocalizedEDName<LandingPadSize>
    {
        static LandingPadSize()
        {
            resourceManager = Properties.StationLargestPad.ResourceManager;
            resourceManager.IgnoreCase = true;
            missingEDNameHandler = (edname) => new LandingPadSize(edname, 0);

            None = new LandingPadSize( "None", 0 );
            Small = new LandingPadSize( "Small", 1 );
            Medium = new LandingPadSize( "Medium", 2 );
            Large = new LandingPadSize( "Large", 3 );
        }

        public static readonly LandingPadSize None;
        public static readonly LandingPadSize Large;
        public static readonly LandingPadSize Medium;
        public static readonly LandingPadSize Small;

        public int sizeIndex { get; private set; }

        // dummy used to ensure that the static constructor has run
        public LandingPadSize() : this("", 0)
        { }

        private LandingPadSize ( string edname, int sizeIndex ) : base( edname, edname )
        {
            this.sizeIndex = sizeIndex;
        }
    }
}
