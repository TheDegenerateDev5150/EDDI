using EddiDataDefinitions;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace EddiSpanshService
{
    public partial class SpanshService
    {
        // Uses the Spansh star system quick API (brief star system data), e.g. https://spansh.co.uk/api/search?q=3932277478106
        // Useful for getting system coordinates and a few details not available from the `dump` endpoint.
        public StarSystem GetQuickStarSystem(ulong systemAddress)
        {
            if ( systemAddress == 0 ) { return null; }
            var request = new RestRequest($"search?q={systemAddress}");
            if (TryGetQuickSystem(request, out var quickStarSystem))
            {
                return quickStarSystem;
            }
            return null;
        }

        public StarSystem GetQuickStarSystem ( string systemName )
        {
            if ( systemName == null || string.IsNullOrEmpty( systemName ) ) { return null; }
            var typeAheadStarSystem = GetTypeAheadStarSystems( systemName ).FirstOrDefault();
            if ( typeAheadStarSystem.Value != null && typeAheadStarSystem.Value.Equals(systemName, StringComparison.InvariantCultureIgnoreCase) )
            {
                var systemAddress = typeAheadStarSystem.Key;
                return GetQuickStarSystem( systemAddress );
            }

            return null;
        }

        public IList<StarSystem> GetQuickStarSystems ( ulong[] systemAddresses )
        {
            var starSystems = new ConcurrentBag<StarSystem>();
            Parallel.ForEach( systemAddresses, systemAddress =>
            {
                if ( systemAddress > 0 )
                {
                    var starSystem = GetQuickStarSystem( systemAddress );
                    if ( starSystem != null )
                    {
                        starSystems.Add( starSystem );
                    }
                }
            } );
            return starSystems.ToList();
        }

        public IList<StarSystem> GetQuickStarSystems ( string[] systemNames )
        {
            var starSystems = new ConcurrentBag<StarSystem>();
            Parallel.ForEach( systemNames, systemName =>
            {
                if ( !string.IsNullOrEmpty( systemName ) )
                {
                    var typeAheadStarSystem = GetTypeAheadStarSystems( systemName ).FirstOrDefault();
                    if ( typeAheadStarSystem.Value != null &&
                         typeAheadStarSystem.Value.Equals( systemName, StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        var systemAddress = typeAheadStarSystem.Key;
                        var starSystem = GetQuickStarSystem( systemAddress );
                        if ( starSystem != null )
                        {
                            starSystems.Add( starSystem );
                        }
                    }
                }
            } );
            return starSystems.ToList();
        }

        private bool TryGetQuickSystem ( IRestRequest request, out StarSystem quickStarSystem )
        {
            var clientResponse = spanshRestClient.Get(request);
            quickStarSystem = null;
            if (clientResponse.IsSuccessful)
            {
                if ( string.IsNullOrEmpty( clientResponse.Content ) )
                {
                    Logging.Warn( "Unable to handle server response." );
                }
                try
                {
                    var jResponse = JToken.Parse( clientResponse.Content );
                    if ( jResponse.Contains( "error" ) )
                    {
                        Logging.Debug( "Spansh responded with: " + jResponse["error"] );
                    }

                    var systemJToken = jResponse[ "results" ]?.FirstOrDefault( r =>
                        r?[ "type" ]?.ToString() == "system" && 
                        r[ "record" ]?[ "id64" ]?.ToObject<ulong?>() == jResponse[ "query" ]?.ToObject<ulong?>() );
                    if ( systemJToken != null )
                    {
                        quickStarSystem = ParseQuickSystem( systemJToken[ "record" ] );
                    }
                }
                catch ( Exception e )
                {
                    Logging.Error( "Failed to parse Spansh response", e );
                }
            }
            else
            {
                Logging.Debug( "Spansh responded with: " + clientResponse.ErrorMessage, clientResponse.ErrorException );
            }

            return quickStarSystem != null;
        }

        private static StarSystem ParseQuickSystem ( JToken data )
        {
            try
            {
                var starSystem = new StarSystem
                {
                    systemname = data[ "name" ].ToString(),
                    systemAddress = data[ "id64" ].ToObject<ulong>(),
                    x = data[ "x" ]?.ToObject<decimal>(),
                    y = data[ "y" ]?.ToObject<decimal>(),
                    z = data[ "z" ]?.ToObject<decimal>(),
                    updatedat = Dates.fromDateTimeToSeconds( JsonParsing.getDateTime("updated_at", data) )
                };

                // Skip parsing star systems lacking essential data - a system name, address, and coordinates
                if ( string.IsNullOrEmpty(starSystem.systemname) || 
                     starSystem.systemAddress == 0 || 
                     starSystem.x is null || 
                     starSystem.y is null || 
                     starSystem.z is null )
                {
                    return null;
                }

                starSystem.requirespermit = data[ "needs_permit" ]?.ToObject<bool?>() ?? false;
                var thargoidWarStateStr =  data[ "thargoid_war_state" ]?.ToString();
                if ( !string.IsNullOrEmpty( thargoidWarStateStr ) && thargoidWarStateStr != "None" )
                {
                    starSystem.ThargoidWar = new ThargoidWar
                    {
                        CurrentState = FactionState.FromName( thargoidWarStateStr ),
                        SuccessState = FactionState.FromName( data[ "thargoid_war_success_state" ]?.ToString() ),
                        FailureState = FactionState.FromName( data[ "thargoid_war_failure_state" ]?.ToString() )
                    };
                }

                starSystem.lastupdated = DateTime.UtcNow;
                return starSystem;
            }
            catch ( Exception e )
            {
                Logging.Error( $"Failed to parse Spansh star system: {e.Message}", e );
            }
            return null;
        }
    }
}