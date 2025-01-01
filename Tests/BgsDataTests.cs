using EddiBgsService;
using EddiDataDefinitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Properties;
using Utilities;

namespace UnitTests
{
    // Tests for the EliteBGS Service
    [TestClass]
    public class BgsDataTests : TestBase
    {
        [TestInitialize]
        public void start()
        {
            MakeSafe();
        }

        [TestMethod]
        public void TestFaction()
        {
            // Test faction data
            var response = DeserializeJsonResource<JObject>(Resources.bgsFaction);
            var faction = fakeBgsService.ParseFaction(response);

            Assert.IsNotNull(faction);

            // Test The Dark Wheel core data
            Assert.AreEqual("Independent", faction.Allegiance.invariantName);
            Assert.AreEqual("Democracy", faction.Government.invariantName);
            Assert.AreEqual("2019-04-13T03:37:17Z", Dates.FromDateTimeToString(faction.updatedAt));

            var factionPresence = faction.presences.FirstOrDefault( p => p.systemName == "Shinrarta Dezhra" );
            Assert.AreEqual(28.9M, factionPresence?.influence);
            Assert.AreEqual("Boom", factionPresence?.FactionState?.invariantName);
            Assert.AreEqual("Happy", factionPresence?.Happiness.invariantName);
            Assert.AreEqual(1, factionPresence?.ActiveStates.Count());
            Assert.AreEqual("Boom", factionPresence?.ActiveStates[0]?.invariantName);
            Assert.AreEqual(0, factionPresence?.PendingStates.Count());
            Assert.AreEqual(0, factionPresence?.RecoveringStates.Count());
            Assert.AreEqual("2019-04-13T03:37:17Z", Dates.FromDateTimeToString(factionPresence?.updatedAt));

            factionPresence = faction.presences.FirstOrDefault( p => p.systemName == "LFT 926" );
            Assert.AreEqual(11.2983M, factionPresence?.influence);
            Assert.AreEqual("Boom", factionPresence?.FactionState?.invariantName);
            Assert.AreEqual("Happy", factionPresence?.Happiness.invariantName);
            Assert.AreEqual(0, factionPresence?.ActiveStates.Count());
            Assert.AreEqual(0, factionPresence?.PendingStates.Count());
            Assert.AreEqual(1, factionPresence?.RecoveringStates.Count());
            Assert.AreEqual("War", factionPresence?.RecoveringStates[0]?.factionState.invariantName);
            Assert.AreEqual(0, factionPresence?.RecoveringStates[0]?.trend);
            Assert.AreEqual("2019-04-13T03:27:28Z", Dates.FromDateTimeToString(factionPresence?.updatedAt));
        }

        [TestMethod]
        public void TestBgsFactionFromName()
        {
            // Setup
            fakeBgsRestClient.Expect( "v5/factions?name=The Dark Wheel&page=1", Encoding.UTF8.GetString( Resources.bgsFactionResponse ) );
            fakeBgsRestClient.Expect( "v5/factions?name=No such faction&page=1", @"{""docs"":[],""total"":0,""limit"":10,""page"":1,""pages"":1,""pagingCounter"":1,""hasPrevPage"":false,""hasNextPage"":false,""prevPage"":null,""nextPage"":null}" );
           
            // Act
            var faction1 = fakeBgsService.GetFactionByName( "The Dark Wheel" );
            var faction2 = fakeBgsService.GetFactionByName( "No such faction" );
            var faction3 = fakeBgsService.GetFactionByName( null );

            // Assert
            Assert.IsNotNull(faction1);
            Assert.AreEqual("The Dark Wheel", faction1.name);
            Assert.AreEqual("Democracy", faction1.Government.invariantName);
            Assert.AreEqual("Independent", faction1.Allegiance.invariantName);
            Assert.AreNotEqual(DateTime.MinValue, faction1.updatedAt);
            Assert.AreEqual(14, faction1.presences.Count);
            var presence = faction1.presences.FirstOrDefault(p => p.systemName == "Shinrarta Dezhra");
            Assert.IsNotNull(presence);
            Assert.AreEqual(FactionState.CivilUnrest, presence.FactionState);
            Assert.AreEqual(28.0719M, presence.influence);
            Assert.AreEqual(Happiness.HappinessBand1, presence.Happiness);
            Assert.AreEqual(1, presence.ActiveStates.Count);
            Assert.AreEqual(FactionState.CivilUnrest, presence.ActiveStates[0]);
            Assert.AreEqual(0, presence.PendingStates.Count);
            Assert.AreEqual(0, presence.RecoveringStates.Count);

            // Return null if the faction cannot be found
            Assert.IsNull(faction2);
            Assert.IsNull( faction3 );
        }

        [TestMethod]
        public void TestParseNoFactions()
        {
            // Setup
            var endpoint = "v5/factions";
            var json = "";
            fakeBgsRestClient.Expect(endpoint, json);
            var queryList = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>(BgsService.FactionParameters.factionName, "")
            };

            // Act
            var factions = fakeBgsService.GetFactions(endpoint, queryList);

            // Assert
            Assert.IsNull(factions);
        }
    }
}
