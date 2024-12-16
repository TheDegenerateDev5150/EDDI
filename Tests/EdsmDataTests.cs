using EddiDataDefinitions;
using EddiStarMapService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using Tests.Properties;

namespace UnitTests
{
    // Tests for the EDSM Service
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

    [TestClass]
    public class EdsmDataTests : TestBase
    {
        FakeEdsmRestClient fakeEdsmRestClient;
        StarMapService fakeEdsmService;

        [TestInitialize]
        public void start()
        {
            fakeEdsmRestClient = new FakeEdsmRestClient();
            fakeEdsmService = new StarMapService(fakeEdsmRestClient);
            MakeSafe();
        }

        [TestMethod]
        public void TestTraffic()
        {
            // Test pilot traffic data
            JObject response = DeserializeJsonResource<JObject>(Resources.edsmTraffic);

            Traffic traffic = fakeEdsmService.ParseStarMapTraffic(response);

            Assert.IsNotNull(traffic);
            Assert.AreEqual(9631, traffic.total);
            Assert.AreEqual(892, traffic.week);
            Assert.AreEqual(193, traffic.day);
        }

        [TestMethod]
        public void TestDeaths()
        {
            // Test pilot mortality data
            JObject response = DeserializeJsonResource<JObject>(Resources.edsmDeaths);

            Traffic deaths = fakeEdsmService.ParseStarMapDeaths(response);

            Assert.IsNotNull(deaths);
            Assert.AreEqual(1068, deaths.total);
            Assert.AreEqual(31, deaths.week);
            Assert.AreEqual(4, deaths.day);
        }

        [TestMethod]
        public void TestTrafficUnknown()
        {
            // Setup
            JObject response = new JObject();

            // Act
            Traffic traffic = fakeEdsmService.ParseStarMapTraffic(response);

            // Assert
            Assert.IsNotNull(traffic);
            Assert.AreEqual(0, traffic.total);
            Assert.AreEqual(0, traffic.week);
            Assert.AreEqual(0, traffic.day);
        }
    }
}
