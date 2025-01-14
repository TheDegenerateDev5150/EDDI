using System.Collections.Generic;

namespace EddiEddnResponder.Toolkit
{
    public class PowerPlayAugmenter
    {
        private bool? IsPledged;

        public void GetInfo ( string edType )
        {
            switch ( edType )
            {
                case "LoadGame":
                    {
                        IsPledged = null;
                        break;
                    }

                case "Powerplay":
                case "PowerplayJoin":
                    {
                        IsPledged = true;
                        break;
                    }

                case "PowerplayLeave":
                    {
                        IsPledged = null;
                        break;
                    }
            }
        }

        public IDictionary<string, object> AugmentPledgeState ( IDictionary<string, object> data )
        {
            if ( IsPledged != null )
            {
                data.Add( "pledged", IsPledged );
            }
            return data;
        }
    }
}
