using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using KufarParser.Interfaces;

namespace KufarParser.Kufar
{
    class KufarParser : IParser<List<Lot>>
    {
        public async Task<List<Lot>> Parse(IHtmlDocument document)
        {
            var lots = new List<Lot>();

            var articles = document.QuerySelectorAll("article")
                .Where(item => item.ClassName != null && item.ClassName.Contains("list_ads__item "));

            foreach (var article in articles)
            {
                IElement name;
                IElement date;
                IElement location;
                IElement price;
                IAttr link;
                string image = null;

                var infoContainer = article.QuerySelectorAll("div")
                    .FirstOrDefault(item => item.ClassName != null && item.ClassName.Contains("list_ads__info_container"));

                if (infoContainer == null) continue;
                {
                    name = infoContainer?.QuerySelectorAll("a")
                        .FirstOrDefault(item => item.ClassName != null && item.ClassName.Contains("list_ads__title"));

                    //delete <span> "скидка" "акция" "бомба" "хит продаж"
                    name?.Children
                        .FirstOrDefault(item => item.ClassName != null && item.ClassName.Contains("list__ribbons"))?
                        .Remove();

                    link = name?.Attributes["href"];

                    date = infoContainer?.QuerySelectorAll("time")
                        .FirstOrDefault(item => item.ClassName != null && item.ClassName.Contains("list_ads__date"));

                    location = infoContainer?.QuerySelectorAll("a")
                        .FirstOrDefault(item =>
                            item.ClassName != null && item.ClassName.Contains("list_ads__location"));

                    price = infoContainer?.QuerySelectorAll("b")
                        .FirstOrDefault(item => item.ClassName != null && item.ClassName.Contains("list_ads__price"))?
                        .QuerySelectorAll("span").FirstOrDefault();
                }

                #region GetImage
                var client = new HttpClient();
                var parser = new HtmlParser();
                var response = await client.GetAsync(link?.Value);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var source = await response.Content.ReadAsStringAsync();
                    var productDocument = await parser.ParseAsync(source);
                    image = productDocument.QuerySelectorAll("img")
                        .FirstOrDefault(item => item.ClassName != null && item.ClassName.Contains("adview_image__base"))?
                        .GetAttribute("data-src");
                }
                #endregion


                lots.Add(new Lot
                {
                    Name = name?.TextContent.Trim(),
                    DateOfUpdate = date?.TextContent.Trim(),
                    Location = location?.TextContent.Trim(),
                    Price = price?.TextContent.Trim(),
                    Link = link?.Value,
                    Image = image
                });
            }

            return lots;
        }
    }
}