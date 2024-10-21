


using Microsoft.Extensions.Configuration;

namespace TestApis.Utils
{
    public class Configuration
    {

        static IConfigurationRoot config;

        static Configuration()
        {
            config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
        }

        public static string GetParameter(string parameter)
        {
            return config.GetSection(parameter).Value.ToString();
        }

    }
}
