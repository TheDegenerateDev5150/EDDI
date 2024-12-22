using EddiStarMapService;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace UnitTests
{
    internal class FakeEdsmRestClient : IEdsmRestClient
    {
        public Dictionary<string, string> CannedContent = new Dictionary<string, string>();
        public Dictionary<string, object> CannedData = new Dictionary<string, object>();

        public Uri BuildUri(IRestRequest request)
        {
            return new Uri("fakeEDSM://" + request.Resource);
        }

        public IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            // this will throw if given a resource not in the canned dictionaries: that's OK
            string content = CannedContent[request.Resource];
            T data = (T)CannedData[request.Resource];
            IRestResponse<T> restResponse = new RestResponse<T>
            {
                Content = content,
                Data = data,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = HttpStatusCode.OK,
            };
            return restResponse;
        }

        public void Expect(string resource, string content, object data)
        {
            CannedContent[resource] = content;
            CannedData[resource] = data;
        }
    }
}