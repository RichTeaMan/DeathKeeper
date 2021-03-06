﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using DeathKeeper.WikiData.Human;
using System.Linq;

namespace DeathKeeper.WikiData.Tests
{
    [TestClass]
    public class HumanFactoryTest
    {
        private HumanFactory humanFactory;
        private WikiDataResponse wikiDataResponse;
        private string TestFile = "Q42.json";

        [TestInitialize]
        public void Init()
        {
            humanFactory = new HumanFactory();
            var requestor = WikiDataRequestor.Create();
            string response = File.ReadAllText(TestFile);
            wikiDataResponse = requestor.ResultFromString(response);
        }

        [TestMethod]
        public void ResultFromWikiDataResponse()
        {
            // test
            var human = humanFactory.FromWikiDataResponse(wikiDataResponse);

            // assert
            Assert.AreEqual("Douglas Adams", human.Label);
            Assert.AreEqual("Douglas Noël Adams", human.BirthName);

            var dobTime = human.DateOfBirth.Value;
            Assert.AreEqual(1952, dobTime.ToDateTimeUtc().Year);
            Assert.AreEqual(3, dobTime.ToDateTimeUtc().Month);
            Assert.AreEqual(11, dobTime.ToDateTimeUtc().Day);

            var dodTime = human.DateOfDeath.Value;
            Assert.AreEqual(2001, dodTime.ToDateTimeUtc().Year);
            Assert.AreEqual(5, dodTime.ToDateTimeUtc().Month);
            Assert.AreEqual(11, dodTime.ToDateTimeUtc().Day);

            Assert.AreEqual(49, human.Age());

            var expectedOccupation = new int[] {28389,
                6625963,
                4853732,
                18844224,
                15978466,
                214917,
                36180 };

            CollectionAssert.AreEqual(expectedOccupation.Select(id => id.ToString()).ToArray(), human.OccupationIds);

            var expectedCountries = new int[] { 145 };

            CollectionAssert.AreEqual(expectedCountries.Select(id => id.ToString()).ToArray(), human.CountryOfCitizenshipIds);
            Assert.AreEqual("https://en.wikipedia.org/wiki/Douglas_Adams", human.WikiLink);

        }

    }
}
