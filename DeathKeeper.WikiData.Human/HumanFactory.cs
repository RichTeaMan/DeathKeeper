using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.WikiData.Human
{
    public class HumanFactory
    {
        private WikiDataRequestor WikiDataRequestor;

        public HumanFactory()
        {
            WikiDataRequestor = new WikiDataRequestor();
        }

        private string GetClaimValue(Entity entity, string claim, string valueKey)
        {
            string result = null;
            var values = GetClaimValues(entity, claim, valueKey);
            if (values != null)
            {
                result = values.FirstOrDefault();
            }
            return result;
        }

        private IEnumerable<string> GetClaimValues(Entity entity, string claim, string valueKey)
        {
            IEnumerable<string> result = new string[] { };
            Claim[] claims;
            if (entity.claims.TryGetValue(claim, out claims))
            {
                result = claims.Select(c => c.mainsnak?.datavalue?.GetDictValue(valueKey)).Where(c => c != null);
            }
            return result;
        }

        public Human FromWikiDataResponse(WikiDataResponse wikiDataResponse)
        {
            var entity = wikiDataResponse.entities.First().Value;

            Language label;
            if (entity.labels.TryGetValue("en", out label))
            {
                label = entity.labels.First().Value;
            }
            string labelValue = label.value;
            string birthName = GetClaimValue(entity, WikiDataProperties.BirthName, "text");
            string[] countryOfCitizenship = GetClaimValues(entity, WikiDataProperties.CountryOfCitizenship, "numeric-id").ToArray();
            string[] occupation = GetClaimValues(entity, WikiDataProperties.Occupation, "numeric-id").ToArray();
            NodaTime.Instant? dateOfBirth = null;
            var dateOfBirthStr = GetClaimValue(entity, WikiDataProperties.DateOfBirth, "time");
            if (dateOfBirthStr != null)
            {
                dateOfBirth = dateOfBirthStr.AsDateTime();
            }
            NodaTime.Instant? dateOfDeath = null;
            var dateOfDeathStr = GetClaimValue(entity, WikiDataProperties.DateOfDeath, "time");
            if (dateOfDeathStr != null)
            {
                dateOfDeath = dateOfDeathStr.AsDateTime();
            }

            string url = null;
            SiteLink enSiteLink = null;
            if (entity.sitelinks.TryGetValue("enwiki", out enSiteLink))
            {
                url = enSiteLink.url;
            }

            var human = new Human(labelValue, birthName, countryOfCitizenship, occupation, dateOfBirth, dateOfDeath, url);
            return human;
    }

        public Human FromEntityId(int id)
        {
            var wikiDataResponse = WikiDataRequestor.GetEntity(id);
            var human = FromWikiDataResponse(wikiDataResponse);
            return human;
        }
    }
}
