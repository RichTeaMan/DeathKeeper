using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using DeathKeeper.WikiData.Human;
using System.Linq;
using NodaTime;
using System;

namespace DeathKeeper.WikiData.Tests
{
    [TestClass]
    public class HumanTest
    {
        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void AncientHumanAge()
        {
            int year = -1000;
            var birth = Instant.FromUtc(year, 1, 1, 0, 0);

            // test
            var human = new Human.Human("Test", "Test", null, null, birth, null, null);

            // assert
            int age = DateTime.Now.Year - year - 1;
            Assert.AreEqual(age, human.Age());
        }

    }
}
