using System;
using AngleSharp.Parser.Html;
using KufarParser.Interfaces;

namespace KufarParser
{
    class ParserWorker<T> where T : class
    {
        private IParserSettings parserSettings;
        private HtmlLoader loader;

        public bool IsActive { get; private set; }
        public IParser<T> Parser { get; set; }
        public IParserSettings Settings
        {
            get => parserSettings;
            set { parserSettings = value; loader = new HtmlLoader(value);}
        }

        public event Action<int, T> OnNewData;
        public event Action<object> OnCompleted;

        public ParserWorker(IParser<T> parser)
        {
            this.Parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }

        public void Start()
        {
            IsActive = true;
            Worker();
        }

        public void Abort()
        {
            IsActive = false;
        }

        private async void Worker()
        {
            for (int i = parserSettings.StartPoint; i <= parserSettings.EndPoint; i++)
            {
                if (!IsActive)
                {
                    OnCompleted?.Invoke(this);
                    return;
                }

                var source = await loader.GetSourceByPageId(i);
                var domParser = new HtmlParser();
                var document = await domParser.ParseAsync(source);
                var result = await Parser.Parse(document);

                OnNewData?.Invoke(i, result);
            }
            OnCompleted?.Invoke(this);
            IsActive = false;
        }

    }
}
