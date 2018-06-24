using System.Threading.Tasks;
using AngleSharp.Dom.Html;

namespace KufarParser.Interfaces
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}