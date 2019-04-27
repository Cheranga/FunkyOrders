using Microsoft.Extensions.Configuration;

namespace API.Tests.Util
{
    public class ConfigurationHelper
    {
        private static Settings _settings;

        public static Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    var root = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables()
                        .Build();

                    var settings = new Settings();
                    root.Bind(settings);

                    _settings = settings;
                }

                return _settings;
            }
        }
    }
}