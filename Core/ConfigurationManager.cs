using Microsoft.Extensions.Configuration;

namespace Core
{
    public class ConfigurationManager
    {
        private static AppConfiguration? _appConfiguration { get; set; }
        public static string BaseUrl => _appConfiguration.BaseUrl;
        public static string RequestBody => _appConfiguration.RequestBody;
        // endpoint data should not be part of configuration
        // endpoint is the specific resource route after baseUrl
        // baseUrl can change between different testing environments
        // but the endpoint itself should not
        public static string InvalidEndpoint => _appConfiguration.InvalidEndpoint;

        public ConfigurationManager()
        {
            _appConfiguration = GetConfiguration();
        }

        public AppConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var appConfiguration = new AppConfiguration();
            configuration.Bind(appConfiguration);

            return appConfiguration;
        }
    }
}

