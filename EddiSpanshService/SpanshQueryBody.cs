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
        // Find bodies with specific properties from the Spansh Body Search API.
        [ CanBeNull ]
        public NavWaypoint GetBodyWaypoint ( ulong fromSystemAddress, [NotNull] Dictionary<string, object> searchFilters )
        {
            var request = GetBodyRestRequest( fromSystemAddress, searchFilters );
            var response = spanshRestClient.Post( request );
            var data = JToken.Parse( response.Content );
            return data[ "results" ]?.Select( ParseQuickBody ).FirstOrDefault();
        }

        private static IRestRequest GetBodyRestRequest ( ulong fromSystemAddress, [NotNull] Dictionary<string, object> filter )
        {
            var request = new RestRequest("bodies/search/save") { Method = Method.POST };
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

        private static NavWaypoint ParseQuickBody ( JToken bodyData )
        {
            var systemName = bodyData[ "system_name" ]?.ToString();
            var systemAddress = bodyData[ "system_id64" ]?.ToObject<ulong>() ?? 0;
            var systemX = bodyData[ "system_x" ]?.ToObject<decimal>() ?? 0;
            var systemY = bodyData[ "system_y" ]?.ToObject<decimal>() ?? 0;
            var systemZ = bodyData[ "system_z" ]?.ToObject<decimal>() ?? 0;

            return new NavWaypoint(systemName, systemAddress, systemX, systemY, systemZ);
        }
    }
}