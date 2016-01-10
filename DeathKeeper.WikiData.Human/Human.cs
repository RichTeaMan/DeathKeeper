using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.WikiData.Human
{
    public class Human
    {
        public string Label { get; private set; }
        public string BirthName { get; private set; }
        public string[] CountryOfCitizenshipIds { get; private set; }
        public string[] OccupationIds { get; private set; }
        public NodaTime.Instant? DateOfBirth { get; private set; }
        public NodaTime.Instant? DateOfDeath { get; private set; }
        public string WikiLink { get; private set; }

        public Human(string label, string birthName, string[] countryOfCitizenshipIds, string[] occupationIds, NodaTime.Instant? dateOfBirth, NodaTime.Instant? dateOfDeath, string wikiLink)
        {
            Label = label;
            BirthName = birthName;
            CountryOfCitizenshipIds = countryOfCitizenshipIds;
            OccupationIds = occupationIds;
            DateOfBirth = dateOfBirth;
            DateOfDeath = dateOfDeath;
            WikiLink = wikiLink;
        }

        public int Age()
        {
            if (!DateOfBirth.HasValue)
            {
                return -1;
            }
            else
            {
                Instant end;
                if (DateOfDeath.HasValue)
                {
                    end = DateOfDeath.Value;
                }
                else
                {
                    end = SystemClock.Instance.Now;
                }
                var age = end - DateOfBirth.Value;
                var days = age.ToTimeSpan().TotalDays;
                return (int)(days / 365.25);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}: {1}", nameof(Label), Label);
            sb.AppendLine();

            sb.AppendFormat("{0}: {1}", nameof(BirthName), BirthName);
            sb.AppendLine();

            sb.AppendFormat("{0}:", nameof(CountryOfCitizenshipIds));
            sb.AppendLine();
            foreach (var countryOfCitizenshipId in CountryOfCitizenshipIds)
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
