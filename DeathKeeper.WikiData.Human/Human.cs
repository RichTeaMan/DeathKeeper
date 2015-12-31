using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.WikiData.Human
{
    public class Human
    {
        public string BirthName { get; private set; }
        public string[] CountryOfCitizenshipIds { get; private set; }
        public string[] OccupationIds { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public DateTime? DateOfDeath { get; private set; }
        public string WikiLink { get; private set; }

        public Human(string birthName, string[] countryOfCitizenshipIds, string[] occupationIds, DateTime dateOfBirth, DateTime? dateOfDeath, string wikiLink)
        {
            BirthName = birthName;
            CountryOfCitizenshipIds = countryOfCitizenshipIds;
            OccupationIds = occupationIds;
            DateOfBirth = dateOfBirth;
            DateOfDeath = dateOfDeath;
            WikiLink = wikiLink;
        }
    }
}
