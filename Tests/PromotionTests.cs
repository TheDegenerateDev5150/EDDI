using EddiEvents;
using EddiJournalMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass, TestCategory( "UnitTests" )]
    public class PromotionTests : TestBase
    {
        [TestInitialize]
        public void start()
        {
            MakeSafe();
        }

        private void ParseSampleByName(string sampleName)
        {
            var sample = Events.SampleByName(sampleName) as string;
            var sampleEvents = JournalMonitor.ParseJournalEntry(sample);
            Assert.AreEqual(1, sampleEvents.Count, $"Expected one event, got {sampleEvents.Count}");
        }

        [TestMethod]
        public void TestCommanderPromotion()
        {
            ParseSampleByName("Commander promotion");
        }
    }
}
