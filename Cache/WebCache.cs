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

        public void FillCache(string url)
        {
            using (var task = Task.Run(async () => await FillCacheAsync(url)))
            {
                task.Wait();
            }
        }

        public async Task FillCacheAsync(string url)
        {
            if (!Directory.Exists(CacheFolder))
                Directory.CreateDirectory(CacheFolder);
            string cacheFilePath = GetCacheFilePath(url);

            if (!File.Exists(cacheFilePath))
            {
                await DownloadAndSave(url, cacheFilePath);
            }
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
            string cacheFilePath = GetCacheFilePath(url);

            string contents;
            if (File.Exists(cacheFilePath))
            {
                using (var reader = File.OpenText(cacheFilePath))
                {
                    contents = await reader.ReadToEndAsync();
                }
            }
            else
            {
                contents = await DownloadAndSave(url, cacheFilePath);
            }
            return contents;
        }

        private async Task<string> DownloadAndSave(string url, string cacheFilePath)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent", UserAgent);
                var uri = new Uri(url, UriKind.Absolute);
                string contents = await client.DownloadStringTaskAsync(uri);
                File.WriteAllText(cacheFilePath.ToLowerInvariant(), contents);
                return contents;
            }
        }

        private string GetCacheFilePath(string url)
        {
            var name = url.Replace(":", "COlON").Replace("\"", "QUOT").Replace("/", "FSLASH").Replace("\\", "BSLASH").Replace("#", "HASH");
            var path = Path.Combine(CacheFolder, name);
            return path;
        }

    }
}
