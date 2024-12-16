using EddiDataDefinitions;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace EddiDataProviderService
{
    public class StarSystemCache
    {
        private readonly CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
        private readonly ObjectCache starSystemCache = new MemoryCache( "StarSystemCache" );

        // Store deserialized star systems in short term memory for this amount of time.
        // Storage time is reset whenever the cached value is accessed.
        public StarSystemCache ( int expirationSeconds )
        {
            cacheItemPolicy.SlidingExpiration = TimeSpan.FromSeconds( expirationSeconds );
        }

        public void AddOrUpdate ( StarSystem starSystem )
        {
            var systemAddress = starSystem.systemAddress.ToString();
            if ( starSystemCache.Contains( systemAddress ) )
            {
                starSystemCache.Remove( systemAddress );
            }
            starSystemCache.Add( systemAddress, starSystem, cacheItemPolicy );
        }

        public bool TryGet ( ulong systemAddress, out StarSystem result )
        {
            if ( starSystemCache.Contains(systemAddress.ToString()) )
            {
                result = starSystemCache.Get( systemAddress.ToString() ) as StarSystem;
                return true;
            }

            result = null;
            return false;
        }

        public List<StarSystem> GetRange ( ulong[] systemAddresses )
        {
            var results = new List<StarSystem>();
            foreach ( var systemAddress in systemAddresses )
            {
                if ( TryGet( systemAddress, out var cachedStarSystem ) )
                {
                    results.Add( cachedStarSystem );
                }
            }

            return results;
        }

        public void Remove ( ulong systemAddress )
        {
            starSystemCache.Remove( systemAddress.ToString() );
        }
    }
}
