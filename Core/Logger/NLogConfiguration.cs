namespace Core.Logger
{
    public class NLogConfiguration
    {
        public NLogSettings? NLog { get; set; }
    }

    public class NLogSettings
    {
        public string? InternalLogLevel { get; set; }

        public string? InternalLogFile { get; set; }

        public List<Extension>? Extensions { get; set; }

        public Dictionary<string, Target>? Targets { get; set; }

        public List<Rule>? Rules { get; set; }
    }

    public class Extension
    {
        public string? Assembly { get; set; }
    }

    public class Target
    {
        public string? Type { get; set; }

        public string? FileName { get; set; }

        public string? Layout { get; set; }
    }

    public class Rule
    {
        public string? Logger { get; set; }

        public string? MinLevel { get; set; }

        public string? WriteTo { get; set; }
    }
}
