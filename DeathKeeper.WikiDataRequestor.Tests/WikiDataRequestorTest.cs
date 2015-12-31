using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DeathKeeper.WikiData.Tests
{
    [TestClass]
    public class WikiDataRequestorTest
    {
        private WikiDataRequestor requestor;

        [TestInitialize]
        public void Init()
        {
            requestor = new WikiDataRequestor();
        }

        [TestMethod]
        public void ResultFromStringTest()
        {
            // setup
            string response = File.ReadAllText("Q42.json");
            string entityKey = "Q42";

            // test
            var wikiDataResponse = requestor.ResultFromString(response);

            // assert
            Assert.IsTrue(wikiDataResponse.entities.ContainsKey(entityKey), string.Format("Does not contain enity with key '{0}'.", entityKey));
            var entity = wikiDataResponse.entities[entityKey];
            Assert.AreEqual(78, entity.claims.Count);

            var dobValues = entity.claims[WikiDataProperties.DateOfBirth][0].mainsnak.datavalue.ValuesAsDictionary();
            Assert.IsNotNull(dobValues);
            var dobTime = dobValues["time"].AsDateTime().Value.ToDateTimeUtc();
            Assert.AreEqual(1952, dobTime.Year);
            Assert.AreEqual(3, dobTime.Month);
            Assert.AreEqual(11, dobTime.Day);

            var dodValues = entity.claims[WikiDataProperties.DateOfDeath][0].mainsnak.datavalue.ValuesAsDictionary();
            Assert.IsNotNull(dodValues);
            var dodTime = dodValues["time"].AsDateTime().Value.ToDateTimeUtc();
            Assert.AreEqual(2001, dodTime.Year);
            Assert.AreEqual(5, dodTime.Month);
            Assert.AreEqual(11, dodTime.Day);

            var enWikiLink = entity.sitelinks["enwiki"];
            Assert.AreEqual("enwiki", enWikiLink.site);
            Assert.AreEqual("Douglas Adams", enWikiLink.title);
            Assert.AreEqual("https://en.wikipedia.org/wiki/Douglas_Adams", enWikiLink.url);

        }

        [TestMethod]
        public void ResultFromStringTest2()
        {
            // setup
            string response = File.ReadAllText("Q272.json");
            string entityKey = "Q272";

            // test
            var wikiDataResponse = requestor.ResultFromString(response);

            // assert
            Assert.IsTrue(wikiDataResponse.entities.ContainsKey(entityKey), string.Format("Does not contain enity with key '{0}'.", entityKey));
            var entity = wikiDataResponse.entities[entityKey];
        }

        [TestMethod]
        public void ResultFromStringTest3()
        {
            // setup
            string response = File.ReadAllText("Q7995.json");
            string entityKey = "Q7995";

            // test
            var wikiDataResponse = requestor.ResultFromString(response);

            // assert
            Assert.IsTrue(wikiDataResponse.entities.ContainsKey(entityKey), string.Format("Does not contain enity with key '{0}'.", entityKey));
            var entity = wikiDataResponse.entities[entityKey];
        }
    }
}
