using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.WikiData
{
    public class WikiDataRequestor
    {
        private readonly string url = "https://www.wikidata.org/wiki/Special:EntityData/Q{0}.json";

        public WikiDataResponse GetEntity(int entityId)
        {
            var instanceUrl = string.Format(url, entityId);

            using (var webclient = new WebClient())
            {
                string body = webclient.DownloadString(instanceUrl);
                var result = ResultFromString(body);
                return result;
            }

        }

        public WikiDataResponse ResultFromString(string body)
        {
            var result = JsonConvert.DeserializeObject<WikiDataResponse>(body);
            return result;
        }
    }
}
