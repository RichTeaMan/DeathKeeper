﻿using System;
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}: {1}", nameof(BirthName), BirthName);
            sb.AppendLine();

            sb.AppendFormat("{0}:", nameof(CountryOfCitizenshipIds));
            sb.AppendLine();
            foreach(var countryOfCitizenshipId in CountryOfCitizenshipIds)
            {
                sb.AppendFormat("\t{0}", countryOfCitizenshipId);
                sb.AppendLine();
            }

            sb.AppendFormat("{0}:", nameof(OccupationIds));
            sb.AppendLine();
            foreach (var occupationId in OccupationIds)
            {
                sb.AppendFormat("\t{0}", occupationId);
                sb.AppendLine();
            }

            sb.AppendFormat("{0}: {1}", nameof(DateOfBirth), DateOfBirth);
            sb.AppendLine();

            sb.AppendFormat("{0}: {1}", nameof(DateOfDeath), DateOfDeath);
            sb.AppendLine();

            sb.AppendFormat("{0}: {1}", nameof(WikiLink), WikiLink);
            sb.AppendLine();

            return sb.ToString();
        }
    }
}