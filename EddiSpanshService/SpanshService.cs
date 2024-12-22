using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace EddiSpanshService
{
    public interface ISpanshRestClient
    {
        Uri BuildUri ( IRestRequest request );
        IRestResponse<T> Execute<T> ( IRestRequest request );
        IRestResponse Get ( IRestRequest request );
        IRestResponse Post ( IRestRequest request );
    }

    public partial class SpanshService
    {
        private const string baseUrl = "https://spansh.co.uk/api/";
        private readonly ISpanshRestClient spanshRestClient;

        // The default timeout for requests to Spansh. Requests can override this by setting `RestRequest.Timeout`. Both are in milliseconds.
        private const int DefaultTimeoutMilliseconds = 10000;

        private class SpanshRestClient : ISpanshRestClient
        {
            private readonly RestClient restClient;

            public SpanshRestClient(string baseUrl)
            {
                restClient = new RestClient(baseUrl)
                {
                    Timeout = DefaultTimeoutMilliseconds
                };
            }

            public Uri BuildUri ( IRestRequest request ) => restClient.BuildUri( request );

            public IRestResponse<T> Execute<T> ( IRestRequest request )
            {
                var response = restClient.Execute<T>( request );
                return response;
            }

            public IRestResponse Get ( IRestRequest request )
            {
                var response = Execute<object>( request );
                return response;
            }

            /// <summary>
            /// Post a search request with a json payload
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            public IRestResponse Post ( IRestRequest request )
            {
                var initialResponse = Execute<object>( request );

                if ( !IsResponseOk( initialResponse, out var initialData ) )
                {
                    return null;
                }

                var searchReferenceId = initialData[ "search_reference" ]?.ToString();
                if ( string.IsNullOrEmpty( searchReferenceId ) )
                {
                    Logging.Warn( "Spansh API failed to provide a search_reference.", initialResponse );
                    return null;
                }

                var response = SearchResponseAsync( request, searchReferenceId ).Result;

                if ( !IsResponseOk( response, out _ ) )
                {
                    return null;
                }

                return response;
            }

            private static bool IsResponseOk ( IRestResponse response, out JToken data )
            {
                data = null;
                if ( response is null )
                {
                    Logging.Warn( "Spansh API is not responding" );
                    return false;
                }

                if ( string.IsNullOrEmpty( response.Content ) )
                {
                    Logging.Warn( "Spansh API responded without providing any data", response );
                    return false;
                }
                data = JToken.Parse( response.Content );
                if ( data[ "error" ] != null )
                {
                    Logging.Debug( "Spansh API responded with: " + data[ "error" ], response );
                    return false;
                }

                return true;
            }

            private async Task<IRestResponse> SearchResponseAsync ( IRestRequest initialRequest, string searchReferenceId )
            {
                return await Task.Run( () =>
                {
                    var requestGroup = initialRequest.Resource.Split( '/' ).FirstOrDefault();
                    var searchRequest = new RestRequest(requestGroup + "/search/recall/" + searchReferenceId);
                    IRestResponse response = null;
                    while ( response is null )
                    {
                        Thread.Sleep( 500 );
                        response = Execute<object>( searchRequest );

                        if ( response is null )
                        {
                            return null;
                        }

                        if ( response.ResponseStatus == ResponseStatus.TimedOut )
                        {
                            Logging.Warn( response.ErrorMessage, searchRequest );
                            return null;
                        }

                        if ( JToken.Parse( response.Content )[ "status" ]?.ToString() != "queued" )
                        {
                            return response;
                        }
                    }

                    return null;
                } ).ConfigureAwait( false );
            }
        }

        public SpanshService(ISpanshRestClient restClient = null)
        {
            spanshRestClient = restClient ?? new SpanshRestClient(baseUrl);
        }

        private async Task<JToken> GetRouteResponseTask(string data)
        {
            return await Task.Run(() =>
            {
                var jobID = GetJobID(data);
                if (string.IsNullOrEmpty(jobID)) return null;
                
                var jobRequest = new RestRequest("results/" + jobID);
                JObject routeResult = null;
                while (routeResult is null || (routeResult["status"]?.ToString() == "queued"))
                {
                    Thread.Sleep(500);
                    var response = spanshRestClient.Get(jobRequest);

                    if (response.ResponseStatus == ResponseStatus.TimedOut)
                    {
                        Logging.Warn(response.ErrorMessage, jobRequest);
                        return null;
                    }

                    routeResult = JObject.Parse(response.Content);
                    if (routeResult["error"] != null)
                    {
                        Logging.Debug(routeResult["error"].ToString());
                        return null;
                    }
                }

                return routeResult["result"];
            }).ConfigureAwait(false);
        }

        private string GetJobID(string route)
        {
            var routeResponse = JObject.Parse(route);
            if (routeResponse["error"] != null)
            {
                Logging.Debug(routeResponse["error"].ToString());
                return null;
            }
            return routeResponse["job"]?.ToString();
        }
    }
}
