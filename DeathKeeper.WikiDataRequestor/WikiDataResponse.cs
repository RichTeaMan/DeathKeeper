using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.WikiData
{

    public class WikiDataResponse
    {
        public Dictionary<string, Entity> entities { get; set; }
    }

    public class Entity
    {
        public int pageid { get; set; }
        public int ns { get; set; }
        public string title { get; set; }
        public int lastrevid { get; set; }
        public DateTime modified { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public Dictionary<string, Language> labels { get; set; }
        public Dictionary<string, Language> descriptions { get; set; }
        public Dictionary<string, Language[]> aliases { get; set; }
        public Dictionary<string, Claim[]> claims { get; set; }
        public Dictionary<string, SiteLink> sitelinks { get; set; }
    }
    //"P1368": [
    //  {
    //    "mainsnak": {
    //      "snaktype": "value",
    //      "property": "P1368",
    //      "datavalue": {
    //        "value": "000057405",
    //        "type": "string"
    //      },
    //      "datatype": "string"
    //    },
    //    "type": "statement",
    //    "id": "Q42$11725e9f-4f81-e0fd-b00a-b885fe7a75ac",
    //    "rank": "normal",
    //    "references": [
    //      {
    //        "hash": "a51d6594fee36c7452eaed2db35a4833613a7078",
    //        "snaks": {
    //          "P143": [
    //            {
    //              "snaktype": "value",
    //              "property": "P143",
    //              "datavalue": {
    //                "value": {
    //                  "entity-type": "item",
    //                  "numeric-id": 54919
    //                },
    //                "type": "wikibase-entityid"
    //              },
    //              "datatype": "wikibase-item"
    //            }
    //          ]
    //        },
    //        "snaks-order": [ "P143" ]
    //      }
    //    ]
    //  }
    //],

    public class Claim
    {
        public Mainsnak mainsnak { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string rank { get; set; }
        public Reference[] references { get; set; }
    }

    public class Mainsnak
    {
        public string snaktype { get; set; }
        public string property { get; set; }
        public Value datavalue { get; set; }
        public string datatype { get; set; }
    }

    public class Value
    {
        public object value { get; set; }

        public Dictionary<string, string> ValuesAsDictionary()
        {
            Dictionary<string, string> dictionary = null;
            var jsonObject = value as JObject;
            if (jsonObject != null)
            {
                dictionary = jsonObject.Properties().ToDictionary(p => p.Name.ToString(), v => v.Value.ToString());
            }
            return dictionary;
        }
    }

    public class Reference
    {
        public string hash { get; set; }
        public Dictionary<string, Snak[]> snaks { get; set; }
        public string[] snaksorder { get; set; }
    }

    public class Snak
    {
        public string snaktype { get; set; }
        public string property { get; set; }
        public Value datavalue { get; set; }
        public string datatype { get; set; }
    }

    public class Language
    {
        public string language { get; set; }
        public string value { get; set; }
    }

    public class SiteLink
    {
        public string site { get; set; }
        public string title { get; set; }
          //"badges": [ ],
          public string url { get; set; }
    }
}
