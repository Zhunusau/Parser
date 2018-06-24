using KufarParser.Interfaces;

namespace KufarParser.Kufar
{
    class KufarSettings : IParserSettings
    {
        public KufarSettings(int start, int end, string filter)
        {
            StartPoint = start;
            EndPoint = end;
            Filter = filter;
        }

        public string BaseUrl { get; set; } = "https://www.kufar.by";

        public string Region { get; set; } = "беларусь";

        public string Filter { get; set; }

        public string Category { get; set; } = "продается";

        public string Prefix { get; set; } = "cu=BYR&o={currentId}";

        public int StartPoint { get; set; }

        public int EndPoint { get; set; }
    }
}
