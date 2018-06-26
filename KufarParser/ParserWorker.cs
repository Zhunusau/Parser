using System;
using System.Collections.Generic;
using AngleSharp;
using AngleSharp.Parser.Html;
using KufarParser.Interfaces;
using KufarParser.Kufar;
using AngleSharp.Scripting.JavaScript;

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
            //Can use while(true), however may be a loop
            for (int page = 1; page <= 100; page++)
            {
                if (!IsActive)
                {
                    OnCompleted?.Invoke(this);
                    return;
                }
                var config = Configuration.Default.WithJavaScript().WithCss();
                var source = await loader.GetSourceByPageId(page);
                var domParser = new HtmlParser(config);
                var document = await domParser.ParseAsync(source);
                var result = await Parser.Parse(document);

                if(result == null) break;

                OnNewData?.Invoke(page, result);
            }
            OnCompleted?.Invoke(this);
            IsActive = false;
        }

    }
}
