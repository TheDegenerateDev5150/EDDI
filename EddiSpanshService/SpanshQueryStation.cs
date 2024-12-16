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
        // Find stations with specific station services from the Spansh Station Search API.
        [ CanBeNull ]
        public NavWaypoint GetStationWaypoint ( ulong fromSystemAddress, Dictionary<string, object> searchFilters )
        {
            var request = GetServiceRestRequest( fromSystemAddress, searchFilters );
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

            var searchTask = StationSearchResultsAsync(initialResponse.Content);
            Task.WaitAll( searchTask );

            if ( searchTask.Result is null )
            {
                Logging.Warn( $"Spansh API returned no route to a station matching filters {JsonConvert.SerializeObject(searchFilters)}." );
                return null;
            }

            return searchTask.Result?.Select( ParseQuickStation ).FirstOrDefault();
        }

        private IRestRequest GetServiceRestRequest ( ulong fromSystemAddress, Dictionary<string, object> filter )
        {
            var request = new RestRequest("stations/search/save") { Method = Method.POST };
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

        private async Task<JToken> StationSearchResultsAsync ( string data )
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

                var searchRequest = new RestRequest("stations/search/recall/" + searchReferenceID);
                JToken stationResult = null;
                while ( stationResult is null || ( stationResult[ "status" ]?.ToString() == "queued" ) )
                {
                    Thread.Sleep( 500 );
                    var response = spanshRestClient.Get(searchRequest);

                    if ( response.ResponseStatus == ResponseStatus.TimedOut )
                    {
                        Logging.Warn( response.ErrorMessage, searchRequest );
                        return null;
                    }

                    stationResult = JToken.Parse( response.Content );
                    if ( stationResult[ "error" ] != null )
                    {
                        Logging.Debug( stationResult[ "error" ].ToString() );
                        return null;
                    }
                }

                return stationResult[ "results" ];
            } ).ConfigureAwait( false );
        }

        private NavWaypoint ParseQuickStation ( JToken stationData )
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