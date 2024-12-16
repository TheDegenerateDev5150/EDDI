using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace EddiSpanshService
{
    public partial class SpanshService
    {
        /// <summary> Partial of system name is required. </summary>
        public Dictionary<ulong, string> GetTypeAheadStarSystems (string partialSystemName)
        {
            if (string.IsNullOrEmpty(partialSystemName)) { return new Dictionary<ulong, string>(); }

            var request = TypeAheadRequest(partialSystemName);
            var clientResponse = spanshRestClient.Get(request);

            if (clientResponse.IsSuccessful)
            {
                if ( string.IsNullOrEmpty( clientResponse.Content ) )
                {
                    Logging.Warn( "Unable to handle server response." );
                    return new Dictionary<ulong, string>();
                }

                Logging.Debug("Spansh responded with " + clientResponse.Content);
                var response = JToken.Parse(clientResponse.Content);
                if (response is JObject responses && 
                    responses.ContainsKey("values"))
                {
                    var starSystems = ParseTypeAheadSystems(responses);
                    return starSystems
                        .OrderByDescending(s => s.Value.StartsWith(partialSystemName, StringComparison.InvariantCultureIgnoreCase))
                        .ThenBy(s => s.Value)
                        .ToDictionary(k => k.Key, v => v.Value);
                }
            }
            else
            {
                Logging.Debug("Spansh responded with " + clientResponse.ErrorMessage, clientResponse.ErrorException);
            }
            return new Dictionary<ulong, string>();
        }

        private IRestRequest TypeAheadRequest(string partialSystemName)
        {
            var request = new RestRequest("systems/field_values/system_names");
            request.AddParameter("q", partialSystemName);
            return request;
        }

        private Dictionary<ulong, string> ParseTypeAheadSystems ( JToken responses )
        {
            return responses[ "min_max" ]?
                .AsParallel()
                .ToDictionary( k => k["id64"].ToObject<ulong>(), v => v["name"].ToString() );
        }
    }
}