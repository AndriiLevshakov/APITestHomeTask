using Microsoft.Extensions.Configuration;

namespace Core
{
    public static class ConfigurationManager
    {
        private static AppConfiguration? _appConfiguration;
        public static string BaseUrl => AppConfiguration.BaseUrl;

        public static AppConfiguration AppConfiguration 
        { 
            get
            {
                if (_appConfiguration == null)
                {
                    _appConfiguration = GetConfiguration();
                }

                return _appConfiguration;
            }
        }        

        private static AppConfiguration GetConfiguration()
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

