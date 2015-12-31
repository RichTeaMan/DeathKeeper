using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cache;
using System.IO;

namespace DeathKeeper.WikiData
{
    public class WikiDataRequestor
    {
        private WebCache cache;
        private readonly string url = "https://www.wikidata.org/wiki/Special:EntityData/Q{0}.json";

        public WikiDataRequestor()
        {
            cache = new WebCache(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DeathKeeperCache"));
        }

        public WikiDataResponse GetEntity(int entityId)
        {
            var instanceUrl = string.Format(url, entityId);

            var body = cache.GetPage(instanceUrl);
            var result = ResultFromString(body);
            return result;
        }

        public WikiDataResponse ResultFromString(string body)
        {
            var result = JsonConvert.DeserializeObject<WikiDataResponse>(body);
            return result;
        }
    }
}
