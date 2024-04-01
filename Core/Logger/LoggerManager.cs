using NLog;

namespace Core.Logger
{
    public static class LoggerManager
    {
        private static ILogger? _logger;

        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    try
                    {
                        string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "NLog.json");
                        TestFrameworkSetup.InitializeLogging(configFilePath);
                        _logger = LogManager.GetCurrentClassLogger();
                    }
                    catch (Exception)
                    {
                        throw new Exception("File is missing");
                    }
                }

                return _logger;
            }
        }
    }
}
