using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KufarParser.Interfaces;

namespace KufarParser
{
    class HtmlLoader
    {
        private readonly HttpClient client;
        private readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            url = $"{settings.BaseUrl}/{settings.Region}/{settings.Filter}--{settings.Category}/{settings.Prefix}";
        }

        public async Task<string> GetSourceByPageId(int id)
        {
            var currentUrl = url.Replace("{currentId}", id.ToString());
            var response = await client.GetAsync(currentUrl);
            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}
