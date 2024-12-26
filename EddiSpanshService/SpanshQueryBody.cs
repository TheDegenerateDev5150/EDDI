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
        // Find bodies with specific properties from the Spansh Body Search API.
        [ CanBeNull ]
        public NavWaypoint GetBodyWaypoint ( decimal fromX, decimal fromY, decimal fromZ, [NotNull] Dictionary<string, object> searchFilters )
        {
            var request = GetBodyRestRequest( fromX, fromY, fromZ, searchFilters );
            var response = spanshRestClient.Post( request );
            var data = JToken.Parse( response.Content );
            if ( data[ "error" ] != null )
            {
                Logging.Debug( "Spansh API responded with: " + data[ "error" ], response );
                return null;
            }
            return ParseQuickBody( data[ "results" ]?.FirstOrDefault() );
        }

        private static IRestRequest GetBodyRestRequest ( decimal fromX, decimal fromY, decimal fromZ, [ NotNull ] Dictionary<string, object> filter )
        {
            var request = new RestRequest("bodies/search") { Method = Method.POST };
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
                reference_coords = new Dictionary<string, object> { { "x", fromX }, { "y", fromY }, { "z", fromZ } }
            };
            request.AddJsonBody( JsonConvert.SerializeObject( jsonObject ) );
            return request;
        }

        private static NavWaypoint ParseQuickBody ( JToken bodyData )
        {
            if ( bodyData is null ) { return null; }

            var systemName = bodyData[ "system_name" ]?.ToString();
            var systemAddress = bodyData[ "system_id64" ]?.ToObject<ulong>() ?? 0;
            var systemX = bodyData[ "system_x" ]?.ToObject<decimal>() ?? 0;
            var systemY = bodyData[ "system_y" ]?.ToObject<decimal>() ?? 0;
            var systemZ = bodyData[ "system_z" ]?.ToObject<decimal>() ?? 0;

            return new NavWaypoint(systemName, systemAddress, systemX, systemY, systemZ);
        }
    }
}