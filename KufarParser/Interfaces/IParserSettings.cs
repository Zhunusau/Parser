namespace KufarParser.Interfaces
{
    interface IParserSettings
    {
        string BaseUrl { get; set; }

        string Region { get; set; }

        string Filter { get; set; }

        string Category { get; set; }

        string Prefix { get; set; }

        int StartPoint { get; set; }

        int EndPoint { get; set; }
    }
}
