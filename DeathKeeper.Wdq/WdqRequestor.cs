using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.Wdq
{
    public class WdqRequestor
    {
        private readonly string url = "http://wdq.wmflabs.org/api?q=claim[31:(tree[{0}][][279])]";

        public WdqResult GetInstancesOf(int subclassId)
        {
            // claim[31:(tree[12280][][279])] gives a list of all instances (P31)of subclasses(P279) of bridges Q12280.
            // see https://wdq.wmflabs.org/api_documentation.html

            var instanceUrl = string.Format(url, subclassId);

            using (var webclient = new WebClient())
            {
                string body = webclient.DownloadString(instanceUrl);
                var result = ResultFromString(body);
                return result;
            }
            
        }

        public WdqResult ResultFromString(string body)
        {
            var result = JsonConvert.DeserializeObject<WdqResult>(body);
            return result;
        }
    }
}
