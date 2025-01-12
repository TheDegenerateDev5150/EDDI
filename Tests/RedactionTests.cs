using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utilities;

namespace Tests
{
    [TestClass, TestCategory("UnitTests")]
    public class RedactionTests : TestBase
    {
        private void TestRoundTrip(string source)
        {
            var rawPath = source != null ? Environment.ExpandEnvironmentVariables(source) : null;
            var redacted = Redaction.RedactEnvironmentVariables(rawPath);
            var expected = source?.Replace("%TMP%", "%TEMP%"); // these are exact synonyms and we normalise on %TEMP%
            Assert.AreEqual(expected, redacted);
        }

        [TestMethod]
        public void TestNullRedaction()
        {
            string source = null;
            TestRoundTrip(source);
        }

        [TestMethod]
        public void TestEmptyRedaction()
        {
            var source = "";
            TestRoundTrip(source);
        }

        [TestMethod]
        public void TestAppdataRedaction()
        {
            var source = @"%APPDATA%\EDDI\eddi.json";
            TestRoundTrip(source);
        }

        [TestMethod]
        public void TestLocalappdataRedaction()
        {
            var source = @"%LOCALAPPDATA%\EDDI\eddi.json";
            TestRoundTrip(source);
        }

        [TestMethod]
        public void TestMedleyRedaction()
        {
            var source = @"ice cream %USERNAME% foo %TMP% bar %TEMP% baz %APPDATA% quux %USERNAME% womble";
            TestRoundTrip(source);
        }

        [TestMethod]
        public void TestMissingEnvVarRedaction()
        {
            var oldVal = Environment.GetEnvironmentVariable("HOMEPATH");
            Environment.SetEnvironmentVariable("HOMEPATH", null);
            var source = @"C:\EDDI\eddi.json";
            var redacted = Redaction.RedactEnvironmentVariables(source);
            var expected = source;
            Assert.AreEqual(expected, redacted);
            Environment.SetEnvironmentVariable("HOMEPATH", oldVal);
        }

        [TestMethod]
        public void TestEmptyEnvVarRedaction()
        {
            var oldVal = Environment.GetEnvironmentVariable("HOMEPATH");
            Environment.SetEnvironmentVariable("HOMEPATH", "");
            var source = @"C:\EDDI\eddi.json";
            var redacted = Redaction.RedactEnvironmentVariables(source);
            var expected = source;
            Assert.AreEqual(expected, redacted);
            Environment.SetEnvironmentVariable("HOMEPATH", oldVal);
        }

        [TestMethod]
        public void TestBackslashEnvVarRedaction()
        {
            var oldVal = Environment.GetEnvironmentVariable("HOMEPATH");
            Environment.SetEnvironmentVariable("HOMEPATH", @"\");
            var source = @"C:\EDDI\eddi.json";
            var redacted = Redaction.RedactEnvironmentVariables(source);
            var expected = source;
            Assert.AreEqual(expected, redacted);
            Environment.SetEnvironmentVariable("HOMEPATH", oldVal);
        }
    }
}
