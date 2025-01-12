using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass, TestCategory( "CredentialTests" )]
    public class OAuthClientIDTests
    {
        [TestMethod]
        public void TestClientIDNotNull()
        {
            var clientIDClass = new PrivateType(typeof(EddiCompanionAppService.ClientId));
            var clientID = clientIDClass.GetStaticField("ID");
            Assert.IsInstanceOfType(clientID, typeof(string));
            Assert.IsNotNull( clientID );
        }
    }
}
