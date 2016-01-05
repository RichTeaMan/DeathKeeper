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
            cache = new WebCache(GetCacheFolder());
        }

        public string GetCacheFolder()
        {
            var cacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DeathKeeperCache");
            return cacheFolder;
        }

        public string GetEntityUrl(int entityId)
        {
            var instanceUrl = string.Format(url, entityId);
            return instanceUrl;
        }

        public WikiDataResponse GetEntity(int entityId)
        {
            var instanceUrl = GetEntityUrl(entityId);

            var body = cache.GetPage(instanceUrl);
            var result = ResultFromString(body);
            return result;
        }

        public WikiDataResponse ResultFromString(string body)
        {
            var result = JsonConvert.DeserializeObject<WikiDataResponse>(body, new JsonSerializerSettings
            {
                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                {
                    var member = args.ErrorContext.Member as string;
                    if (member == "aliases" || member == "descriptions")
                    {
                        args.ErrorContext.Handled = true;
                    }
                }
            });
            return result;
        }
    }
}
