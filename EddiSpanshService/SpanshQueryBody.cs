using EddiDataDefinitions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace EddiSpanshService
{
    public partial class SpanshService
    {
        // Find bodies with specific properties from the Spansh Body Search API.
        [ CanBeNull ]
        public NavWaypoint GetBodyWaypoint ( ulong fromSystemAddress, Dictionary<string, object> searchFilters )
        {
            var request = GetBodyRestRequest( fromSystemAddress, searchFilters );
            if ( request is null )
            {
                throw new ArgumentException( "Unable to generate RestRequest from arguments" );
            }

            var initialResponse = spanshRestClient.Get(request);
            if ( string.IsNullOrEmpty( initialResponse.Content ) )
            {
                Logging.Warn( "Spansh API is not responding" );
                return null;
            }

            var searchTask = BodySearchResultsAsync(initialResponse.Content);
            Task.WaitAll( searchTask );

            if ( searchTask.Result is null )
            {
                Logging.Warn( $"Spansh API returned no route to a body matching filters {JsonConvert.SerializeObject(searchFilters)}." );
                return null;
            }

            return searchTask.Result?.Select( ParseQuickBody ).FirstOrDefault();
        }

        private IRestRequest GetBodyRestRequest ( ulong fromSystemAddress, Dictionary<string, object> filter )
        {
            var request = new RestRequest("bodies/search/save") { Method = Method.POST };
            if ( filter is null ) { return null;  }

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

        private async Task<JToken> BodySearchResultsAsync ( string data )
        {
            return await Task.Run( () =>
            {
                var searchResponse = JToken.Parse(data);
                if ( searchResponse[ "error" ] != null )
                {
                    Logging.Debug( searchResponse[ "error" ].ToString() );
                    return null;
                }

                var searchReferenceID = searchResponse[ "search_reference" ]?.ToString();
                if ( string.IsNullOrEmpty( searchReferenceID ) ) return null;

                var searchRequest = new RestRequest("bodies/search/recall/" + searchReferenceID);
                JToken bodyResult = null;
                while ( bodyResult is null || ( bodyResult[ "status" ]?.ToString() == "queued" ) )
                {
                    Thread.Sleep( 500 );
                    var response = spanshRestClient.Get(searchRequest);

                    if ( response.ResponseStatus == ResponseStatus.TimedOut )
                    {
                        Logging.Warn( response.ErrorMessage, searchRequest );
                        return null;
                    }

                    bodyResult = JToken.Parse( response.Content );
                    if ( bodyResult[ "error" ] != null )
                    {
                        Logging.Debug( bodyResult[ "error" ].ToString() );
                        return null;
                    }
                }

                return bodyResult[ "results" ];
            } ).ConfigureAwait( false );
        }

        private NavWaypoint ParseQuickBody ( JToken bodyData )
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