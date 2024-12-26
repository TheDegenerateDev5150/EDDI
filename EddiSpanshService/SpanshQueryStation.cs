using EddiDataDefinitions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace EddiSpanshService
{
    public partial class SpanshService
    {
        // Find stations with specific station services from the Spansh Station Search API.
        [ CanBeNull ]
        public NavWaypoint GetStationWaypoint ( decimal fromX, decimal fromY, decimal fromZ, [NotNull] Dictionary<string, object> searchFilters )
        {
            var request = GetServiceRestRequest( fromX, fromY, fromZ, searchFilters );
            var response = spanshRestClient.Post( request );
            var data = JToken.Parse( response.Content );
            if ( data[ "error" ] != null )
            {
                Logging.Debug( "Spansh API responded with: " + data[ "error" ], response );
                return null;
            }
            return ParseQuickStation( data[ "results" ]?.FirstOrDefault() );
        }

        private static IRestRequest GetServiceRestRequest ( decimal fromX, decimal fromY, decimal fromZ, [NotNull] Dictionary<string, object> filter )
        {
            var request = new RestRequest("stations/search") { Method = Method.POST };
            var jsonObject = new
            {
                filters = filter,
                sort = new List<object>
                {
                    new { distance = new { direction = "asc" } },
                    new { distance_to_arrival = new { direction = "asc" } }
                },
                size = 10,
                page = 0,
                reference_coords = new Dictionary<string, object>
                {
                    { "x", fromX }, { "y", fromY }, { "z", fromZ }
                }
            };
            request.AddJsonBody( JsonConvert.SerializeObject( jsonObject ) );
            return request;
        }

        private static NavWaypoint ParseQuickStation ( JToken stationData )
        {
            if ( stationData is null ) { return null; }

            var systemName = stationData[ "system_name" ]?.ToString();
            var systemAddress = stationData[ "system_id64" ]?.ToObject<ulong>() ?? 0;
            var systemX = stationData[ "system_x" ]?.ToObject<decimal>() ?? 0;
            var systemY = stationData[ "system_y" ]?.ToObject<decimal>() ?? 0;
            var systemZ = stationData[ "system_z" ]?.ToObject<decimal>() ?? 0;

            return new NavWaypoint(systemName, systemAddress, systemX, systemY, systemZ)
            {
                stationName = stationData[ "name" ]?.ToString(),
                marketID = stationData[ "market_id"]?.ToObject<long>()
            };
        }
    }
}