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
        private const string cachePath = @"D:\Projects\WikiDataDumpProcessor\WikiDataDumpProcessor\DeathKeeperCache";
        public WebCache WebCache { get; protected set; }
        private JsonSerializerSettings JsonSerializerSettings;
        private readonly string url = "https://www.wikidata.org/wiki/Special:EntityData/Q{0}.json";

        public static WikiDataRequestor Create()
        {
            var webCache = new WebCache(cachePath);
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                {
                    var member = args.ErrorContext.Member as string;
                    if (member == "aliases" || member == "descriptions")
                    {
                        args.ErrorContext.Handled = true;
                    }
                }
            };
            var wikiDataRequestor = new WikiDataRequestor(webCache, jsonSerializerSettings);
            return wikiDataRequestor;
        }

        public WikiDataRequestor(WebCache webCache) : this(webCache, new JsonSerializerSettings()) { }

        public WikiDataRequestor(WebCache webCache, JsonSerializerSettings jsonSerializerSettings)
        {
            WebCache = webCache;
            JsonSerializerSettings = jsonSerializerSettings;
        }

        public string GetEntityUrl(int entityId)
        {
            var instanceUrl = string.Format(url, entityId);
            return instanceUrl;
        }

        public WikiDataResponse GetEntity(int entityId)
        {
            var instanceUrl = GetEntityUrl(entityId);

            var body = WebCache.GetPage(instanceUrl);
            var result = ResultFromString(body);
            return result;
        }

        public WikiDataResponse ResultFromString(string body)
        {
            var result = JsonConvert.DeserializeObject<WikiDataResponse>(body, JsonSerializerSettings);
            return result;
        }
    }
}
