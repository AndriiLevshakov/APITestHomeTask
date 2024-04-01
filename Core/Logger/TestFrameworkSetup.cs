using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;

namespace Core.Logger
{
    public static class TestFrameworkSetup
    {
        public static void InitializeLogging(string configFilePath)
        {
            NLogConfiguration config = NLogConfigLoader.LoadConfig(configFilePath);

            IConfigurationRoot nlogConfig = new ConfigurationBuilder()
                .AddJsonFile(configFilePath)
                .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "NLog:InternalLogLevel",  config.NLog.InternalLogLevel },
                        { "NLog:InternalLogFile", config.NLog.InternalLogFile },
                        { "NLog:extensions:0:Assembly", config.NLog.Extensions[0].Assembly },
                        { "NLog:Targets:file:Type", config.NLog.Targets["file"].Type },
                        { "NLog:Targets:file:FileName", config.NLog.Targets["file"].FileName },
                        { "NLog:Targets:file:Layout", config.NLog.Targets["file"].Layout },
                        { "NLog:Targets:console:Type", config.NLog.Targets["console"].Type },
                        { "NLog:Targets:console:Layout", config.NLog.Targets["console"].Layout },
                        { "NLog:Rules:0:Logger", config.NLog.Rules[0].Logger },
                        { "NLog:Rules:0:MinLevel", config.NLog.Rules[0].MinLevel },
                        { "Nlog:Rules:0:WriteTo", config.NLog.Rules[0].WriteTo }
                }).Build();

            LogManager.Configuration = new NLogLoggingConfiguration(nlogConfig.GetSection("NLog"));
        }
    }
}
