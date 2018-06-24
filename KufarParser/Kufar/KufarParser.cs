using System.Collections.Generic;
using System.Linq;
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
        public List<Lot> Parse(IHtmlDocument document)
        {
            var lots = new List<Lot>();

            var articles = document.QuerySelectorAll("article")
                .Where(item => item.ClassName != null && item.ClassName.Contains("list_ads__item "));

            var img = document.QuerySelectorAll("img");

            foreach (var article in articles)
            {
                IElement name;
                IElement date;
                IElement location;
                IElement price;
                //string image;
                IAttr link;

                var imageContainer = article.QuerySelectorAll("div")
                    .FirstOrDefault(item => item.ClassName != null && item.ClassName.Contains("list_ads__image_container"));

                //if (imageContainer == null) continue;
                {
                    //image = imageContainer.GetElementsByTagName("img")[0].GetAttribute("src");
                    //var dataImages = imageContainer?.QuerySelectorAll("a").FirstOrDefault();

                    //var images = dataImages?.Children.Attr("src");
                    //.Where(item => item.Attributes["src"] != null)
                    //.Select(item => item.Attributes["src"]);

                    //image = images?.First();
                }

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

                lots.Add(new Lot
                {
                    Name = name?.TextContent.Trim(),
                    DateOfUpdate = date?.TextContent.Trim(),
                    Location = location?.TextContent.Trim(),
                    Price = price?.TextContent.Trim(),
                    Link = link?.Value,
                    //Image = image
                });
            }

            return lots;
        }
    }
}