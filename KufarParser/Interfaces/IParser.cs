using System.Threading.Tasks;
using AngleSharp.Dom.Html;

namespace KufarParser.Interfaces
{
    interface IParser<T> where T : class
    {
        Task<T> Parse(IHtmlDocument document);
    }
}