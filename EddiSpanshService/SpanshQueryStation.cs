using EddiDataDefinitions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace EddiSpanshService
{
    public partial class SpanshService
    {
        // Find stations with specific station services from the Spansh Station Search API.
        [ CanBeNull ]
        public NavWaypoint GetStationWaypoint ( ulong fromSystemAddress, [NotNull] Dictionary<string, object> searchFilters )
        {
            var request = GetServiceRestRequest( fromSystemAddress, searchFilters );
            var response = spanshRestClient.Post( request );
            var data = JToken.Parse( response.Content );
            return data[ "results" ]?.Select( ParseQuickStation ).FirstOrDefault();
        }

        private static IRestRequest GetServiceRestRequest ( ulong fromSystemAddress, [NotNull] Dictionary<string, object> filter )
        {
            var request = new RestRequest("stations/search/save") { Method = Method.POST };
            var jsonObject = new
            {
                filters = filter,
                sort = @"[ { ""distance"": { ""direction"": ""asc"" } }, { ""distance_to_arrival"": { ""direction"": ""asc"" } } ]",
                size = 10,
                page = 0,
                reference_id64 = fromSystemAddress
            };
            request.AddJsonBody( JsonConvert.SerializeObject( jsonObject ) );
            return request;
        }

        private static NavWaypoint ParseQuickStation ( JToken stationData )
        {
            var systemName = stationData[ "system_name" ]?.ToString();
            var systemAddress = stationData[ "system_id64" ]?.ToObject<ulong>() ?? 0;
            var systemX = stationData[ "system_x" ]?.ToObject<decimal>() ?? 0;
            var systemY = stationData[ "system_y" ]?.ToObject<decimal>() ?? 0;
            var systemZ = stationData[ "system_z" ]?.ToObject<decimal>() ?? 0;

            return new NavWaypoint(systemName, systemAddress, systemX, systemY, systemZ)
            {
                stationName = stationData[ "name" ]?.ToString()
            };
        }
    }
}