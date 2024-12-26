using EddiCore;
using EddiDataDefinitions;
using EddiNavigationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using Tests.Properties;
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

            EDDI.Instance.DataProvider = ConfigureTestDataProvider();

            fakeSpanshRestClient.Expect(
                @"bodies/search?={""filters"":{""type"":{""value"":[""Star""]},""subtype"":{""value"":[""A (Blue-White super giant) Star"",""A (Blue-White) Star"",""B (Blue-White super giant) Star"",""B (Blue-White) Star"",""F (White super giant) Star"",""F (White) Star"",""G (White-Yellow super giant) Star"",""G (White-Yellow) Star"",""K (Yellow-Orange giant) Star"",""K (Yellow-Orange) Star"",""M (Red dwarf) Star"",""M (Red giant) Star"",""M (Red super giant) Star"",""O (Blue-White) Star""]}},""sort"":[{""distance"":{""direction"":""asc""}},{""distance_to_arrival"":{""direction"":""asc""}}],""size"":10,""page"":0,""reference_coords"":{""x"":0.0,""y"":0.0,""z"":0.0}}",
                Encoding.UTF8.GetString( Resources.SpanshQueryBodyScoopableStar ) );
            fakeSpanshRestClient.Expect(
                @"stations/search?={""filters"":{""material_trader"":{""value"":[""Encoded""]},""is_planetary"":{""value"":false},""has_large_pad"":{""value"":true},""distance_to_arrival"":{""comparison"":""<=>"",""value"":[0,10000]}},""sort"":[{""distance"":{""direction"":""asc""}},{""distance_to_arrival"":{""direction"":""asc""}}],""size"":10,""page"":0,""reference_coords"":{""x"":0.0,""y"":0.0,""z"":0.0}}",
                Encoding.UTF8.GetString(Resources.SpanshQueryStationEncodedMtrl) );
            fakeSpanshRestClient.Expect(
                @"stations/search?={""filters"":{""material_trader"":{""value"":[""Manufactured""]},""is_planetary"":{""value"":false},""has_large_pad"":{""value"":true},""distance_to_arrival"":{""comparison"":""<=>"",""value"":[0,10000]}},""sort"":[{""distance"":{""direction"":""asc""}},{""distance_to_arrival"":{""direction"":""asc""}}],""size"":10,""page"":0,""reference_coords"":{""x"":0.0,""y"":0.0,""z"":0.0}}",
                Encoding.UTF8.GetString( Resources.SpanshQueryStationManufacturedMtrl ) );
            fakeSpanshRestClient.Expect(
                @"stations/search?={""filters"":{""material_trader"":{""value"":[""Raw""]},""is_planetary"":{""value"":false},""has_large_pad"":{""value"":true},""distance_to_arrival"":{""comparison"":""<=>"",""value"":[0,10000]}},""sort"":[{""distance"":{""direction"":""asc""}},{""distance_to_arrival"":{""direction"":""asc""}}],""size"":10,""page"":0,""reference_coords"":{""x"":0.0,""y"":0.0,""z"":0.0}}",
                Encoding.UTF8.GetString( Resources.SpanshQueryStationRawMtrl ) );
            fakeSpanshRestClient.Expect(
                @"stations/search?={""filters"":{""technology_broker"":{""value"":[""Guardian""]},""is_planetary"":{""value"":false},""has_large_pad"":{""value"":true},""distance_to_arrival"":{""comparison"":""<=>"",""value"":[0,10000]}},""sort"":[{""distance"":{""direction"":""asc""}},{""distance_to_arrival"":{""direction"":""asc""}}],""size"":10,""page"":0,""reference_coords"":{""x"":0.0,""y"":0.0,""z"":0.0}}",
                Encoding.UTF8.GetString( Resources.SpanshQueryStationGuardianTechBroker ) );
            fakeSpanshRestClient.Expect(
                @"stations/search?={""filters"":{""technology_broker"":{""value"":[""Human""]},""is_planetary"":{""value"":false},""has_large_pad"":{""value"":true},""distance_to_arrival"":{""comparison"":""<=>"",""value"":[0,10000]}},""sort"":[{""distance"":{""direction"":""asc""}},{""distance_to_arrival"":{""direction"":""asc""}}],""size"":10,""page"":0,""reference_coords"":{""x"":0.0,""y"":0.0,""z"":0.0}}",
                Encoding.UTF8.GetString( Resources.SpanshQueryStationHumanTechBroker ) );
            fakeSpanshRestClient.Expect(
                @"stations/search?={""filters"":{""services"":{""value"":[""Interstellar Factors Contact""]},""is_planetary"":{""value"":false},""has_large_pad"":{""value"":true},""distance_to_arrival"":{""comparison"":""<=>"",""value"":[0,10000]}},""sort"":[{""distance"":{""direction"":""asc""}},{""distance_to_arrival"":{""direction"":""asc""}}],""size"":10,""page"":0,""reference_coords"":{""x"":0.0,""y"":0.0,""z"":0.0}}",
                Encoding.UTF8.GetString( Resources.SpanshQueryStationFacilitator ) );
            fakeSpanshRestClient.Expect(
                @"stations/search?={""filters"":{""system_primary_economy"":{""value"":[""Military""]},""type"":{""value"":[""Planetary Port""]},""services"":{""value"":[""Outfitting""]},""has_large_pad"":{""value"":true},""distance_to_arrival"":{""comparison"":""<=>"",""value"":[0,10000]}},""sort"":[{""distance"":{""direction"":""asc""}},{""distance_to_arrival"":{""direction"":""asc""}}],""size"":10,""page"":0,""reference_coords"":{""x"":0.0,""y"":0.0,""z"":0.0}}",
                Encoding.UTF8.GetString( Resources.SpanshQueryStationScorpionSRV ) );

            fakeSpanshRestClient.Expect( "dump/1109989017963",
                Encoding.UTF8.GetString( Resources.SpanshStarSystemDumpAlioth ) );
            fakeSpanshRestClient.Expect( "dump/306253399220",
                Encoding.UTF8.GetString( Resources.SpanshStarSystemDumpAltair ) );
            fakeSpanshRestClient.Expect( "dump/18263140541865",
                Encoding.UTF8.GetString( Resources.SpanshStarSystemDumpBarnards_Star ) );
            fakeSpanshRestClient.Expect( "dump/22661186987433",
                Encoding.UTF8.GetString( Resources.SpanshStarSystemDumpEZ_Aquarii ) );
            fakeSpanshRestClient.Expect( "dump/4717761530219",
                Encoding.UTF8.GetString( Resources.SpanshStarSystemDumpGendalla ) );
            fakeSpanshRestClient.Expect( "dump/121569805492",
                Encoding.UTF8.GetString( Resources.SpanshStarSystemDumpSirius ) );
            fakeSpanshRestClient.Expect( "dump/10477373803",
                Encoding.UTF8.GetString( Resources.SpanshStarSystemDumpSol ) );
            fakeSpanshRestClient.Expect( "dump/5856288576210",
                Encoding.UTF8.GetString( Resources.SpanshStarSystemDump61_Cyngi ) );
        }

        [DataTestMethod, DoNotParallelize]
        [DataRow(QueryType.encoded, null, null, 10000.0, true, "EZ Aquarii", "Magnus Gateway")]
        [DataRow(QueryType.manufactured, null, null, 10000.0, true, "Sirius", "Patterson Enterprise")]
        [DataRow(QueryType.raw, null, null, 10000.0, true, "61 Cygni", "Broglie Terminal")]
        [DataRow(QueryType.guardian, null, null, 10000.0, true, "EZ Aquarii", "Magnus Gateway")]
        [DataRow(QueryType.human, null, null, 10000.0, true, "Altair", "Solo Orbiter")]
        [DataRow(QueryType.scorpion, null, null, 10000.0, true, "Gendalla", "Aksyonov Installation")]
        [DataRow(QueryType.scoop, null, null, 10.0, true, "Sol", null)]
        [DataRow(QueryType.facilitator, null, null, 10000.0, true, "Barnard's Star", "Levi-Strauss Installation" )]
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