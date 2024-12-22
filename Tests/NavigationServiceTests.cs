using EddiCore;
using EddiDataDefinitions;
using EddiNavigationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UnitTests;

namespace IntegrationTests
{
    [TestClass]
    public class NavigationServiceTests : TestBase
    {
        private NavigationService navigationService;

        [TestInitialize]
        public void Start()
        {
            navigationService = new NavigationService();
            MakeSafe();
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
            EDDI.Instance.DataProvider = ConfigureTestDataProvider();

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