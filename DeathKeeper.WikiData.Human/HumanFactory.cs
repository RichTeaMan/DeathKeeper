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
            IEnumerable<string> result = null;
            var claims = entity.claims[claim];
            if (claims != null)
            {
                result = claims.Select(c => c.mainsnak.datavalue.GetDictValue(valueKey));
            }
            return result;
        }

        public Human FromWikiDataResponse(WikiDataResponse wikiDataResponse)
        {
            var entity = wikiDataResponse.entities.First().Value;

            string birthName = GetClaimValue(entity, WikiDataProperties.BirthName, "text");
            string[] countryOfCitizenship = GetClaimValues(entity, WikiDataProperties.CountryOfCitizenship, "numeric-id").ToArray();
            string[] occupation = GetClaimValues(entity, WikiDataProperties.Occupation, "numeric-id").ToArray();
            DateTime dateOfBirth = GetClaimValue(entity, WikiDataProperties.DateOfBirth, "time").AsDateTime();
            DateTime dateOfDeath = GetClaimValue(entity, WikiDataProperties.DateOfDeath, "time").AsDateTime();

            string url = null;
            SiteLink enSiteLink = null;
            if (entity.sitelinks.TryGetValue("enwiki", out enSiteLink))
            {
                url = enSiteLink.url;
            }

            var human = new Human(birthName, countryOfCitizenship, occupation, dateOfBirth, dateOfDeath, url);
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
