using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass, TestCategory("UnitTests")]
    // this class is pure and doesn't need TestBase.MakeSafe()
    public class CoriolisTests
    {
        [TestMethod]
        public void TestUri()
        {
            var data = "BZ+24 123";
            var uriData = Uri.EscapeDataString(data);
            Assert.AreEqual("BZ%2B24%20123", uriData);
        }
    }
}
