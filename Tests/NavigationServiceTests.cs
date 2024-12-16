using EddiCore;
using EddiDataDefinitions;
using EddiDataProviderService;
using EddiNavigationService;
using EddiSpanshService;
using EddiStarMapService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.Properties;
using UnitTests;

namespace IntegrationTests
{
    [TestClass]
    public class NavigationServiceTests : TestBase
    {
        private FakeEdsmRestClient fakeEdsmRestClient;
        private FakeSpanshRestClient fakeSpanshRestClient;
        private StarMapService fakeEdsmService;
        private SpanshService fakeSpanshService;
        private NavigationService navigationService;

        [TestInitialize]
        public void Start()
        {
            fakeEdsmRestClient = new FakeEdsmRestClient();
            fakeSpanshRestClient = new FakeSpanshRestClient();
            fakeEdsmService = new StarMapService( fakeEdsmRestClient );
            fakeSpanshService = new SpanshService( fakeSpanshRestClient );
            EDDI.Instance.DataProvider = new DataProviderService( fakeEdsmService, fakeSpanshService );
            navigationService = new NavigationService();
            MakeSafe();

            // Use a standard cube search around Sol for our service queries 
            var resource = "api-v1/cube-systems";
            var json = Encoding.UTF8.GetString(Resources.cubeSystemsAroundSol);
            var data = new List<JObject>();
            fakeEdsmRestClient.Expect(resource, json, data);

            // Use a standard cube search around Sol for our service queries 
            var resource2 = "api-v1/sphere-systems";
            var json2 = Encoding.UTF8.GetString(Resources.sphereAroundSol);
            var data2 = new List<JObject>();
            fakeEdsmRestClient.Expect(resource2, json2, data2);
        }

        [DataTestMethod, DoNotParallelize]
        [DataRow(QueryType.encoded, null, null, 10000.0, true, "EZ Aquarii", "Magnus Gateway")]
        [DataRow(QueryType.manufactured, null, null, 10000.0, true, "Sirius", "Patterson Enterprise")]
        [DataRow(QueryType.raw, null, null, 10000.0, true, "61 Cygni", "Broglie Terminal")]
        [DataRow(QueryType.guardian, null, null, 10000.0, true, "EZ Aquarii", "Magnus Gateway")]
        [DataRow(QueryType.human, null, null, 10000.0, true, "WISE 1506+7027", "Dobrovolskiy Enterprise")]
        [DataRow(QueryType.scorpion, null, null, 10000.0, true, "Gendalla", "Aksyonov Installation")]
        [DataRow(QueryType.scoop, null, null, 10.0, true, "Sol", null)]
        [DataRow(QueryType.facilitator, null, null, 10000.0, true, "Barnard's Star", "Miller Depot")]
        public void TestNavQuery(QueryType query, string stringArg0, string stringArg1, double numericArg, bool prioritizeOrbitalStations, string expectedStarSystem, string expectedStationName)
        {
            // Setup
            var sol = new StarSystem { systemname = "Sol", systemAddress = 10477373803, x = 0.0M, y = 0.0M, z = 0.0M };
            EDDI.Instance.CurrentStarSystem = sol;
            EDDI.Instance.CurrentShip = ShipDefinitions.FromEDModel( "Anaconda" );

            var result = navigationService.NavQuery(query, stringArg0, stringArg1, Convert.ToDecimal(numericArg), prioritizeOrbitalStations);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedStarSystem, result.system);
            Assert.AreEqual(expectedStationName, result.station);
        }
    }
}