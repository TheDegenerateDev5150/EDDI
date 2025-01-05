﻿using EddiBgsService;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Utilities;

namespace UnitTests
{
    public class FakeBgsRestClient : IBgsRestClient
    {
        public Dictionary<string, string> CannedContent = new Dictionary<string, string>();

        public Uri BuildUri(IRestRequest request)
        {
            return new Uri("fakeBGS://" + request.Resource);
        }

        public IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            var resourceString = $"{request.Resource}";

            // this will throw if given a resource not in the canned dictionaries: that's OK
            try
            {
                resourceString += request.Parameters.Any() ? "?" : string.Empty;
                for ( int i = 0; i < request.Parameters.Count; i++ )
                {
                    var parameter = request.Parameters[ i ];
                    resourceString += i > 0 ? "&" : "";
                    resourceString += $"{parameter.Name}={parameter.Value}";
                }

                string content = CannedContent[resourceString];
                IRestResponse<T> restResponse = new RestResponse<T>
                {
                    Content = content,
                    ResponseStatus = ResponseStatus.Completed,
                    StatusCode = HttpStatusCode.OK,
                };
                return restResponse;
            }
            catch ( KeyNotFoundException knfe )
            {
                 Logging.Error( knfe.Message, knfe );
                throw;
            }

        }

        public void Expect(string resource, string content)
        {
            CannedContent[resource] = content;
        }
    }
}