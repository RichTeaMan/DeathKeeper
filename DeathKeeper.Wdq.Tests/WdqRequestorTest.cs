using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DeathKeeper.Wdq.Tests
{
    [TestClass]
    public class WdqRequestorTest
    {
        private WdqRequestor requestor;
        private string TestFile = "response.json";

        [TestInitialize]
        public void Init()
        {
            requestor = new WdqRequestor();
        }

        [TestMethod]
        public void ResultFromStringTest()
        {
            // setup
            string response = File.ReadAllText(TestFile);

            // test
            var wdqResponse = requestor.ResultFromString(response);

            // assert
            Assert.AreEqual("OK", wdqResponse.status.error);
            Assert.AreEqual(14505, wdqResponse.status.items);
            Assert.AreEqual("3719ms", wdqResponse.status.querytime);
            Assert.AreEqual(14505, wdqResponse.items.Length);

        }
    }
}
