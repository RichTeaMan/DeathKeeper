using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cache
{
    public class WebCache
    {
        public string UserAgent { get; set; } = "TomCo Caching Agent";
        public string CacheFolder { get; private set; }

        public WebCache(string cacheFolder)
        {
            CacheFolder = cacheFolder;
        }

        public string GetPage(string url)
        {
            using (var task = Task.Run(async () => await GetPageAsync(url)))
            {
                task.Wait();
                var page = task.Result;
                return page;
            }
        }

        public async Task<string> GetPageAsync(string url)
        {
            if (!Directory.Exists(CacheFolder))
                Directory.CreateDirectory(CacheFolder);

            string cacheFileName = string.Format("{0}\\{1}", CacheFolder, url.Replace(":", "COlON").Replace("\"", "QUOT").Replace("/", "FSLASH").Replace("\\", "BSLASH").Replace("#", "HASH"));

            string contents;
            if (File.Exists(cacheFileName))
            {
                using (var reader = File.OpenText(cacheFileName))
                {
                    contents = await reader.ReadToEndAsync();
                }
            }
            else
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", UserAgent);
                    var uri= new Uri(url, UriKind.Absolute);
                    contents = await client.DownloadStringTaskAsync(uri);
                }

                File.WriteAllText(cacheFileName.ToLowerInvariant(), contents);
            }
            return contents;
        }
       

    }
}
