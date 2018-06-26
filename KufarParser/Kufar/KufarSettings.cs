using KufarParser.Interfaces;

namespace KufarParser.Kufar
{
    class KufarSettings : IParserSettings
    {
        public KufarSettings(string filter)
        {
            Filter = filter;
            Prefix = $"{Region}/{Filter}--{Category}?{Param}";
        }

        public string BaseUrl { get; set; } = "https://www.kufar.by";

        public string Region { get; set; } = "беларусь";

        public string Filter { get; set; }

        public string Category { get; set; } = "продается";

        public string Param { get; set; } = "cu=BYR&o={currentPage}";

        public string Prefix { get; set; }
    }
}
