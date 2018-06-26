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
            url = $"{settings.BaseUrl}/{settings.Prefix}";
        }

        public async Task<string> GetSourceByPageId(int page)
        {
            var currentUrl = url.Replace("{currentPage}", page.ToString());
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
