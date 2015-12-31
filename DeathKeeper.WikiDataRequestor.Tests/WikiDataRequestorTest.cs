using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DeathKeeper.WikiData.Tests
{
    [TestClass]
    public class WikiDataRequestorTest
    {
        private WikiDataRequestor requestor;
        private string TestFile = "Q42.json";

        [TestInitialize]
        public void Init()
        {
            requestor = new WikiDataRequestor();
        }

        [TestMethod]
        public void ResultFromStringTest()
        {
            // setup
            string response = File.ReadAllText(TestFile);
            string entityKey = "Q42";

            // test
            var wikiDataResponse = requestor.ResultFromString(response);

            // assert
            Assert.IsTrue(wikiDataResponse.entities.ContainsKey(entityKey), string.Format("Does not contain enity with key '{0}'.", entityKey));
            var entity = wikiDataResponse.entities[entityKey];
            Assert.AreEqual(78, entity.claims.Count);

            var dobValues = entity.claims[WikiDataProperties.DateOfBirth][0].mainsnak.datavalue.ValuesAsDictionary();
            Assert.IsNotNull(dobValues);
            var dobTime = dobValues["time"].AsDateTime();
            Assert.AreEqual(1952, dobTime.Year);
            Assert.AreEqual(3, dobTime.Month);
            Assert.AreEqual(11, dobTime.Day);

            var dodValues = entity.claims[WikiDataProperties.DateOfDeath][0].mainsnak.datavalue.ValuesAsDictionary();
            Assert.IsNotNull(dodValues);
            var dodTime = dodValues["time"].AsDateTime();
            Assert.AreEqual(2001, dodTime.Year);
            Assert.AreEqual(5, dodTime.Month);
            Assert.AreEqual(11, dodTime.Day);

            var enWikiLink = entity.sitelinks["enwiki"];
            Assert.AreEqual("enwiki", enWikiLink.site);
            Assert.AreEqual("Douglas Adams", enWikiLink.title);
            Assert.AreEqual("https://en.wikipedia.org/wiki/Douglas_Adams", enWikiLink.url);

        }
    }
}
